<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SyncEmployeeUser.aspx.cs" Inherits="Master_SyncEmployeeUser" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
<link rel="stylesheet" type="text/css" href="../Styles/pips/Style_doctype.css"/>
<script type="text/javascript" src="../js/calendarWidget.js"></script>
<script language="javascript" type="text/javascript" src="../js/jquery-1.2.3.js"></script>
<script language="javascript" type="text/javascript" src="../js/modal-window.js"></script>
<script language="javascript" type="text/javascript" src="../js/utils.js"></script>
<script language="javascript" type="text/javascript">
    function clickInfo(strParam) {
        openMyModal('../procurement/PRMonitoringInfo.aspx?' + strParam, 1000, 450, "auto");
    }

    function clickFlow(strParam) {
        openMyModal('../procurement/FlowControlmonitoring_Fetch.aspx?' + strParam, 1000, 450, "auto");
    }

    function lov_organization() {
        getFromLOV('', 'txtOrganizationName', 'LovOrganizationName.aspx');
    }

    function clickAll(obj) {
        objdtl = document.getElementsByName("chkEmp");
        for (d = 0; d < objdtl.length; d++) {
            document.getElementsByName("chkEmp")[d].checked = obj.checked;
        }
    }

    function clickSync() {
        objdtl = document.getElementsByName("chkEmp");
        var intSelect = 0;
        for (d = 0; d < objdtl.length; d++) {
            if (document.getElementsByName("chkEmp")[d].checked) {
                intSelect = 1;
                break;
            }
        }

        if (intSelect == 0) {
            alert("No Employee(s) selected");
            return false;
        } else {
            if (confirm('Are you sure to sync. selected employee ?')) {
                document.getElementById("btnSync").style.display = "none";
                document.getElementById("textLoading").style.display = "";
            }
        }
        
    }
</script>
</head>
<body style="MARGIN: 0px" marginheight="0" marginwidth="0">
<form id="form1" runat="server">
<table align="left" width="100%">
  <tr>
    <td>
        <table cellspacing="0" cellpadding="0" width="100%" border="0">
        <tr>
          <td><img border="0" src="../images_tab/tab_s_on.gif"></td> 
          <td background="../images_tab/tab_on.gif" nowrap align="center"><b>Sync. Employee User</b></td> 
          <td><img border="0" src="../images_tab/tab_e_on.gif"></td> 
          <td background="../images_tab/tab_back.gif" width="100%">&nbsp;</td> 
          <td><img border="0" src="../images_tab/tab_end.gif"></td>
        </tr>
      </table>
         <table border="0" cellspacing="0" cellpadding="0">
          <tr>
            <td valign="top">
              <asp:Panel id="pnlFilter" runat="server">
              <table class="Record" cellspacing="0" cellpadding="0">
                <tr class="Controls">
                  <td class="th">Username&nbsp;&nbsp; </td> 
                  <td><asp:TextBox ID="txtUserName" runat="server" Columns="15" 
                          CssClass="ControlsIN"></asp:TextBox>
                      &nbsp;</td> 
                  <td class="th">NIK / Nama&nbsp;&nbsp; </td> 
                  <td><asp:TextBox ID="txtSearch" runat="server" Columns="32" 
                        CssClass="ControlsIN"></asp:TextBox>
                    &nbsp;</td> 
                  <td class="th">Organisasi&nbsp;&nbsp; </td> 
                  <td>
                     <asp:TextBox id="txtOrganizationName" CssClass="ControlsIN" maxlength="64" Columns="35" runat="server"/>
                     <img onclick="lov_organization()" border="0" src="../images/view.gif">&nbsp;
                  </td> 
                </tr>
                <tr class="Controls">
                  <td colspan="6" style="text-align: right">&nbsp; 
                    <asp:Button ID="btnFilter" runat="server" CssClass="Button" Text="Find" 
                          onclick="btnFilter_Click" />
                      &nbsp;&nbsp;</td>
                </tr>
              </table>
              </asp:Panel>
            </td>
          </tr>
        </table>
        <br /><br />
      <asp:Panel id="pnlGrid" runat="server">
          <table class="Header" border="0" cellspacing="0" cellpadding="0" width="100%">
            <tr>
            <td class="HeaderLeft"><img border="0" alt="" src="../Styles/pips/Images/Spacer.gif"></td> 
            <td class="th">
                    Sync Employee
            </td> 
            <td class="HeaderRight"><img border="0" alt="" src="../Styles/pips/Images/Spacer.gif"></td>
            </tr>
        </table>
        <asp:GridView ID="dgvList" runat="server" AutoGenerateColumns="false" 
        CellPadding="5" CellSpacing="0" PageSize="20" AllowPaging="false" PagerSettings-Visible="false" AllowCustomPaging="false"
        UseAccessibleHeader="true" Width="100%" OnRowDataBound="dgvList_RowDataBound">
        <HeaderStyle CssClass="Caption" />
        <RowStyle CssClass="Row"/>
        <AlternatingRowStyle CssClass="AltRow"/>
        <Columns>
            <asp:BoundField DataField="UserId_PK" HeaderText="ID"></asp:BoundField>
            <asp:TemplateField HeaderText="<input type='checkbox' name='chkAll' id='chkAll' onclick='javascript:clickAll(this)'>" HeaderStyle-Width="2%" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:Label ID="lblChk" runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="UserName" HeaderText="Username"></asp:BoundField>
            <asp:BoundField DataField="EmployeeNo" HeaderText="NIK"></asp:BoundField>
            <asp:BoundField DataField="FullName" HeaderText="Nama"></asp:BoundField>
            <asp:BoundField DataField="OrganizationName" HeaderText="Organisasi"></asp:BoundField>
            <asp:BoundField DataField="JobPositionName" HeaderText="Jabatan"></asp:BoundField>
            <asp:BoundField DataField="Leader" HeaderText="Atasan Langsung"></asp:BoundField>
            <asp:BoundField DataField="SM" HeaderText="SM"></asp:BoundField>
            <asp:BoundField DataField="GM" HeaderText="GM"></asp:BoundField>
            <asp:BoundField DataField="Director" HeaderText="Direksi"></asp:BoundField>
            
        </Columns>
        </asp:GridView>
      <asp:Label ID="lblNoRow" runat="server"></asp:Label>
      <br />
      </asp:Panel>
        <asp:Panel ID="pnlPaging" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" width="99%" align="center">
        <tr>
        <td>
            <asp:ImageButton ID="imbFirst" runat="server" onclick="imbFirst_Click"/>
            <asp:ImageButton ID="imbPrev" runat="server" onclick="imbPrev_Click"/>
            &nbsp;
            <asp:Label ID="lblPaging" runat="server"></asp:Label>
            &nbsp;
            <asp:ImageButton ID="imbNext" runat="server" onclick="imbNext_Click"/>
            <asp:ImageButton ID="imbLast" runat="server" onclick="imbLast_Click"/>
            &nbsp; <span id="PRFormHolder" runat="server">
            <asp:Button ID="btnSync" runat="server" CssClass="Button" OnClick="btnSync_Click" OnClientClick="return clickSync()" Text="Sync. from HCMS" />
            <a id="textLoading" name="textLoading" style="display:none">Please wait while loading ...</a>
            </span>
        </td>
        </tr>
        </table>
        </asp:Panel>
      
    </td>
  </tr>
</table>
    
</form>
</body>
</html>