using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Company.Apps.WsDataPIPS;
using Company.Apps.Utility;

public partial class lov_LovOrganizationName : System.Web.UI.Page
{

    WsDataPIPS WsData = new WsDataPIPS();
    ClsUtility ClsUtil = new ClsUtility();

    protected void Page_Load(object sender, EventArgs e)
    {
        //ClsUtil.CheckSession();

        LOVOBJ.Text = GetRequest.GetQueryString("OBJ");
        LOVFORM.Text = GetRequest.GetQueryString("FORM");
        dgvData.ItemDataBound += new DataGridItemEventHandler(this.dgvData_ItemDataBound);
        dgvData.PageIndexChanged += new DataGridPageChangedEventHandler(this.dgvData_PageIndexChanged);
        if (!IsPostBack)
        {   
            LoadData();
        }
    }

    protected void LoadData()
    {
        DataSet dsData = null;
        DataTable dtData = null;

        dsData = WsData.RunSQL("select OrganizationName, (case OrganizationLevelId_FK when 4 then 'BAGIAN' when 8 then 'SUB BAGIAN' end) as OrgLevel from pips.tblM_Organization where OrganizationLevelId_FK in (4,8) and OrganizationName like '%" + txtSearch.Text + "%'");
        dtData = dsData.Tables[0];
        dgvData.DataSource = dtData;
        dgvData.DataBind();

        if (dgvData.PageCount > 1)
        {
            dgvData.PagerStyle.Visible = true;
        }
        else
        {
            dgvData.PagerStyle.Visible = false;
        }
    }

    void dgvData_PageIndexChanged(Object sender, DataGridPageChangedEventArgs e)
    {
        dgvData.CurrentPageIndex = e.NewPageIndex;
        LoadData();
    }

    void dgvData_ItemDataBound(Object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {

        int idx = e.Item.ItemIndex;
        if (idx > -1)
        {
            string strOrgName = e.Item.Cells[1].Text;

            System.Web.UI.WebControls.Label lblChooseVal = (System.Web.UI.WebControls.Label)(e.Item.FindControl("lblChoose"));

            lblChooseVal.Text = "<input class='Button' value='Select' type=button name='btnChoose' id='btnChoose' onclick=\"javascript:clickReturn('" + strOrgName + "')\">";

        }

    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        dgvData.CurrentPageIndex = 0;
        LoadData();
    }
}