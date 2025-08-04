using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Company.Apps.Utility;
using Company.Apps.WsDataPIPS;
using Company.Apps.WsWorkflowEngine;
using System.Configuration;
using System.Web.Script.Serialization;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Company.Apps.WsEmail;
using System.Net;
using ExcelLibrary.BinaryFileFormat;
using DevExpress.XtraRichEdit.Layout.Export;
using DevExpress.XtraRichEdit.Fields;
using DevExpress.Xpo.Logger;

public partial class Master_SyncEmployeeUser : System.Web.UI.Page
{

    ClsUtility ClsUtil = new ClsUtility();
    WsDataPIPS WsData = new WsDataPIPS();
    WsWorkflowEngine WsWF = new WsWorkflowEngine();
    WsEmail wsemail = new WsEmail();

    string DELETE_CONFIRM_MSG = ClsUtility.MessageDeleteConfirm();

    protected void Page_Load(object sender, EventArgs e)
    {
        ClsUtil.CheckSession();
        if (!IsPostBack)
        {
            /*
            string strEncryptedString = Request.QueryString[ConfigurationManager.AppSettings["ParamEncrypt"].ToString()];
            string strDecryptedString = Server.UrlDecode(ClsCryptoEngine.Decrypt(strEncryptedString.Replace(" ", "+")).Replace("&amp;", "&"));
            Response.Write(strDecryptedString);
            */
            
            //ViewData(1);
            pnlGrid.Visible = false;
            pnlPaging.Visible = false;
            txtOrganizationName.Attributes.Add("readonly", "readonly");

            ViewData(1);
        }
    }
    

    protected void ViewData(int intCurrPage)
    {
        pnlGrid.Visible = true;
        pnlFilter.Visible = true;
        pnlPaging.Visible = true;

        DataSet dsData = null;
        DataTable dtData0 = null;
        DataTable dtData1 = null;

        dsData = WsData.RunSQL("begin " +
                            "declare " +
                            "@rowamount numeric, " +
                            "@pagecount numeric; " +
                            "exec pips.stp_GetEmployeeUser_fetch " +
                            "'" + txtUserName.Text.Trim() + "', " +
                            "'" + txtSearch.Text.Trim() + "', " +
                            "'" + txtOrganizationName.Text.Trim() + "', " +
                            "'" + ClsAuthSession.USERNAME + "', " +
                            "" + intCurrPage + ", " +
                            "50, " +
                            "@rowamount out, " +
                            "@pagecount out; " +
                            "end;");
        dtData0 = dsData.Tables[0];
        dtData1 = dsData.Tables[1];
        dgvList.DataSource = dtData0;
        dgvList.DataBind();

        long intRowAmount = Convert.ToInt64(dtData1.Rows[0]["RowAmount"]);
        long intPageCount = Convert.ToInt64(dtData1.Rows[0]["PageCount"]);
        ViewState["pageCount"] = intPageCount;
        ViewState["currPage"] = intCurrPage;

        if (dtData0.Rows.Count == 0)
        {
            lblNoRow.Text = ClsUtility.MessageNoData();
            dgvList.Visible = false;
            lblNoRow.Visible = true;
        }
        else
        {
            dgvList.Visible = true;
            lblNoRow.Text = "";
            lblNoRow.Visible = false;
        }

        ClsUtil.LoadImagePaging(intCurrPage, 50, intPageCount, imbFirst, imbPrev, imbNext, imbLast, lblPaging);
        
    }


    protected void dgvList_RowDataBound(Object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string strUserId = e.Row.Cells[0].Text;
            string strEmpNo = e.Row.Cells[3].Text;
            Label lblChk = (Label)e.Row.Cells[1].FindControl("lblChk");

            lblChk.Text = "<input type='checkbox' value='" + strEmpNo + "' name='chkEmp' id='chkEmp'>";
        }

        e.Row.Cells[0].Visible = false;

    }

    protected void imbNext_Click(object sender, ImageClickEventArgs e)
    {
        int intCurrPage = Convert.ToInt32(ViewState["currPage"]) + 1;
        ViewState["currPage"] = intCurrPage;
        ViewData(intCurrPage);
    }
    protected void imbLast_Click(object sender, ImageClickEventArgs e)
    {
        int intCurrPage = Convert.ToInt32(ViewState["pageCount"]);
        ViewState["currPage"] = intCurrPage;
        ViewData(intCurrPage);
    }
    protected void imbPrev_Click(object sender, ImageClickEventArgs e)
    {
        int intCurrPage = Convert.ToInt32(ViewState["currPage"]) - 1;
        ViewState["currPage"] = intCurrPage;
        ViewData(intCurrPage);
    }
    protected void imbFirst_Click(object sender, ImageClickEventArgs e)
    {
        int intCurrPage = 1;
        ViewState["currPage"] = intCurrPage;
        ViewData(intCurrPage);
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        int intCurrPage = 1;
        ViewState["currPage"] = intCurrPage;
        ViewData(intCurrPage);
    }


    protected void btnSync_Click(object sender, EventArgs e)
    {

        try
        {

            string strFilter_username = txtUserName.Text.Trim();
            string strFilter_search = txtSearch.Text.Trim();
            string strFilter_organization = txtOrganizationName.Text.Trim();

            DataSet dsEmp;
            DataTable dtEmp;

            DataSet dsLead;
            DataTable dtLead;

            string strEmpList = Request["chkEmp"];
            string[] arrEmp = strEmpList.Split(',');
            int i;
            string strEmp = string.Empty;

            string strUsername = string.Empty;
            string strEmployeeNo = string.Empty;
            string strEmployeeName = string.Empty;
            string strMailAddress = string.Empty;
            string strOrganizationCode = string.Empty;
            string strOrganizationName = string.Empty;
            string strJobPositionCode = string.Empty;
            string strJobPositionName = string.Empty;

            string strLeader = string.Empty;
            string strSM = string.Empty;
            string strGM = string.Empty;
            string strDir = string.Empty;

            for (i = 0; i < arrEmp.Length; i++)
            {
                strEmp = arrEmp[i].Trim();

                dsEmp = WsData.RunSQL("begin exec pips.stp_APIToHCM_GetEmployee " +
                                    "'" + strEmp + "', " +
                                    "'" + ClsAuthSession.USERNAME + "'; end;");
                dtEmp = dsEmp.Tables[0];

                dsLead = WsData.RunSQL("begin exec pips.stp_APIToHCM_GetAtasanFull " +
                                    "'" + strEmp + "', " +
                                    "'" + ClsAuthSession.USERNAME + "'; end;");
                dtLead = dsLead.Tables[0];

                strUsername = dtEmp.Rows[0]["userName"].ToString();
                strEmployeeNo = dtEmp.Rows[0]["employeeNo"].ToString();
                strEmployeeName = dtEmp.Rows[0]["employeeName"].ToString();
                strMailAddress = dtEmp.Rows[0]["mailAddress"].ToString();
                strOrganizationCode = dtEmp.Rows[0]["organizationCode"].ToString();
                strOrganizationName = dtEmp.Rows[0]["organizationName"].ToString();
                strJobPositionCode = dtEmp.Rows[0]["jobPositionCode"].ToString();
                strJobPositionName = dtEmp.Rows[0]["jobPositionName"].ToString();

                strLeader = dtLead.Rows[0]["Leader_employeeNo"].ToString();
                strSM = dtLead.Rows[0]["SM_employeeNo"].ToString();
                strGM = dtLead.Rows[0]["GM_employeeNo"].ToString();
                strDir = dtLead.Rows[0]["Director_employeeNo"].ToString();

                string strExec = WsData.ExecuteScalar("begin " +
                                                "declare " +
                                                "@intOutStatus numeric, " +
                                                "@strOutMessage varchar(max); " +
                                                "exec pips.stp_EmployeeUserSyncUpdate " +
                                                "  '" + strUsername + "', " +
                                                "  '" + strEmployeeNo + "', " +
                                                "  '" + strEmployeeName + "', " +
                                                "  '" + strMailAddress + "', " +
                                                "  '" + strOrganizationCode + "', " +
                                                "  '" + strOrganizationName + "', " +
                                                "  '" + strJobPositionCode + "', " +
                                                "  '" + strJobPositionName + "', " +
                                                "  '" + strLeader + "', " +
                                                "  '" + strSM + "', " +
                                                "  '" + strGM + "', " +
                                                "  '" + strDir + "', " +
                                                "  '" + ClsAuthSession.USERNAME + "', " +
                                                "  @intOutStatus out, " +
                                                "  @strOutMessage out; " +
                                                "select pips.to_char(@intOutStatus,null)+'#'+@strOutMessage; " +
                                                "end;");

                string[] arrExec = strExec.Split('#');
                long intStatus = Convert.ToInt64(arrExec[0].ToString());
                string strMsg = arrExec[1].ToString();

                if (intStatus == 0)
                {
                    int intLog = WsData.ExecuteNonQuery("begin " +
                                                "exec pips.stp_APILogger " +
                                                  "'SyncEmployee_HCM_EPROC', " +
                                                  "'{\"employeeNo\":\"" + strEmployeeNo + "\"}', " +
                                                  "'0', " +
                                                  "'" + strMsg + "', " +
                                                  "'" + ClsAuthSession.USERNAME + "'; " +
                                                "end;");
                }

            }

            txtUserName.Text = strFilter_username;
            txtSearch.Text = strFilter_search;
            txtOrganizationName.Text = strFilter_organization;

            ViewData(1);

            ClientScriptManager cs = Page.ClientScript;
            Type cstype = this.GetType();
            cs.RegisterStartupScript(cstype, null, "<script language='javascript'>alert('Sinkronisasi telah dilakukan. Silakan periksa kembali data');</script>");
            return;
        }
        catch (Exception ex)
        {

            ClientScriptManager cs = Page.ClientScript;
            Type cstype = this.GetType();
            cs.RegisterStartupScript(cstype, null, "<script language='javascript'>alert('Sinkronisasi GAGAL dilakukan. Silakan periksa kembali Sumber Data');</script>");
            return;

        }


    }
}
