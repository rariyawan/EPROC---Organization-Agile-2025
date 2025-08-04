<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LovOrganizationName.aspx.cs" Inherits="lov_LovOrganizationName" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../Styles/pips/Style_doctype.css"/>
</head>
<script language="javascript" type="text/javascript">
    function clickReturn(retVal) {
        strObj = document.getElementById('LOVOBJ').value;
        strForm = document.getElementById('LOVFORM').value;
        //alert(strObj + " " + strForm);
        arrVal = retVal.split('#~#');
        arrForm = strObj.split(',');
        for (x = 0; x < arrForm.length; x++) {
            parent.document.getElementById(strForm + arrForm[x]).value = arrVal[x];
        }
        parent.modalWindow.close();
    }

    function clickBlank() {
        clickReturn('#~##~#');
    }
</script>
<body style="BACKGROUND-COLOR: #ffffff" bottommargin="3" leftmargin="3" rightmargin="3" topmargin="3">
    <form id="form1" runat="server">
    <table cellspacing="0" cellpadding="0" width="100%" border="0">
      <tr>
        <td><img border="0" src="../images/tab_s_on.gif"></td> 
        <td class="TabOn" background="../images/tab_on.gif" nowrap align="center">Organisasi</td> 
          <td><img border="0" src="../images/tab_e_on.gif"></td> 
        <td background="../images/tab_back.gif" width="100%"></td>
      </tr>
    </table>
    <table cellspacing="0" cellpadding="0" width="100%" border="0">
      <tr>
        <td valign="top">
            <table cellspacing="1" cellpadding="0">
            <tr class="Controls">
                <td class="th">Find&nbsp;&nbsp; </td> 
                <td><asp:TextBox ID="txtSearch" runat="server" Columns="32" 
                        CssClass="ControlsIN"></asp:TextBox>
          &nbsp;<asp:TextBox ID="LOVOBJ" runat="server" Columns="32" CssClass="ControlsIN" style="display:none"></asp:TextBox>
                <asp:TextBox ID="LOVFORM" runat="server" Columns="32" CssClass="ControlsIN" style="display:none"></asp:TextBox>
                <asp:Button ID="btnFilter" runat="server" CssClass="Button" Text="Search" 
                        onclick="btnFilter_Click"/>
                </td> 
            </tr>
            </table>
        </td>
      </tr>  
      <tr>
        <td valign="top">
        <asp:DataGrid ID="dgvData" runat="server" AutoGenerateColumns="false" 
        CellPadding="5" CellSpacing="0" AllowPaging="true" PageSize="20" PagerStyle-Mode="NextPrev"
        UseAccessibleHeader="true" Width="100%" BorderStyle="None" GridLines="None">
        <HeaderStyle CssClass="Caption" />
        <ItemStyle CssClass="Row"/>
        <AlternatingItemStyle CssClass="Row"/>
        <Columns>
            <asp:TemplateColumn HeaderText="<input type='button' class='Button' value='Blank' onclick='javascript:clickBlank()'>" HeaderStyle-Width="2%" HeaderStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Top" ItemStyle-HorizontalAlign="Center">
                <ItemTemplate>
                    <asp:Label ID="lblChoose" runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateColumn>
            <asp:BoundColumn DataField="OrganizationName" HeaderText="Nama Organisasi" Visible="true"></asp:BoundColumn>
            <asp:BoundColumn DataField="OrgLevel" HeaderText="Level" Visible="true"></asp:BoundColumn>
        </Columns>
            
        </asp:DataGrid>
        </td>
      </tr>
    </table>
    </form>
</body>
</html>
