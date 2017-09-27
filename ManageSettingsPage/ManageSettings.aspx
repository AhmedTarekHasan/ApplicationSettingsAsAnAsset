<%@ Page Language="C#" AutoEventWireup="true" enableSessionState="True" Inherits="DevelopmentSimplyPut.Pages.ManageSettings, DevelopmentSimplyPut,  Version=1.0.0.0, Culture=neutral, PublicKeyToken=5b3b2dbf31f780b4" %>

<%@ Import Namespace="DevelopmentSimplyPut.CommonUtilities" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI.WebControls" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35" %>
<%@ Register TagPrefix="SPSWC" Namespace="Microsoft.SharePoint.Portal.WebControls"
    Assembly="Microsoft.SharePoint.Portal, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="wssawc" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="PublishingWebControls" Namespace="Microsoft.SharePoint.Publishing.WebControls"
    Assembly="Microsoft.SharePoint.Publishing, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Nav" Namespace="Microsoft.SharePoint.Publishing.Navigation"
    Assembly="Microsoft.SharePoint.Publishing, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormSection" Src="~/_controltemplates/InputFormSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="InputFormControl" Src="~/_controltemplates/InputFormControl.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="ButtonSection" Src="~/_controltemplates/ButtonSection.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="Welcome" Src="~/_controltemplates/Welcome.ascx" %>
<%@ Register TagPrefix="wssuc" TagName="DesignModeConsole" Src="~/_controltemplates/DesignModeConsole.ascx" %>
<%@ Register TagPrefix="PublishingVariations" TagName="VariationsLabelMenu" Src="~/_controltemplates/VariationsLabelMenu.ascx" %>
<%@ Register TagPrefix="PublishingConsole" TagName="Console" Src="~/_controltemplates/PublishingConsole.ascx" %>
<%@ Register TagPrefix="PublishingSiteAction" TagName="SiteActionMenu" Src="~/_controltemplates/PublishingActionMenu.ascx" %>
<asp:content id="PageTitle" runat="server" contentplaceholderid="PlaceHolderPageTitle">
    Manage Settings
</asp:content>
<asp:content id="PageTitleInTitleArea" runat="server" contentplaceholderid="PlaceHolderPageTitleInTitleArea">
</asp:content>
<asp:content id="Main" runat="server" contentplaceholderid="PlaceHolderMain">
    <script>
    </script>

	<table cellspacing="0" cellpadding="3" width="100%">
	    <tr>
	        <td>
	            <div id="SettingsGridDiv" runat="server" class="grid">
	                <DevelopmentSimplyPutWebControls:EnhancedDataGrid runat="server"
	                ID="grd_Settings"
	                AutoGenerateColumns="False"
	                AllowPaging="false"
	                AllowSorting="false"
	                PageSize="200"
	                CurrentPageIndex="0"
	                VirtualItemCount="0"
	                ExportToExcel="False"
	                BorderStyle="None"
	                Width="100%"
	                GridLines="None"
	                HorizontalAlign="Center"
	                HorizontalScrollBarVisibility="Hidden"
	                SortingUpImageRelativePath="tri-up.gif"
	                SortingDownImageRelativePath="tri.gif"
	                PagingNextImageRelativePath="rmc/pager_next_arrow.png"
	                PagingPrevImageRelativePath="rmc/pager_perv_arrow.png"
	                CssClass="gridStyle-table">
	                <RowStyle CssClass="gridStyle-tr-data" Wrap="False" />
	                <AlternatingRowStyle CssClass="gridStyle-tr-alt-data" Wrap="False" />
	                <HeaderStyle CssClass="gridStyle-tr-header" />
		                <Columns>
			                <asp:TemplateField HeaderText="Category">
				                <ItemTemplate>
					                <asp:Label  ID="lbl_Category" Text='<%# Eval("SettingDefinition.Category") %>'
						                runat="server" />
				                </ItemTemplate>
				                <ItemStyle CssClass="gridStyle-item-td Category-Css" />
				                <HeaderStyle CssClass="gridStyle-header-th Category-Css" Wrap="true" Width="10%"/>
			                </asp:TemplateField>
			                <asp:TemplateField HeaderText="Key">
				                <ItemTemplate>
					                <asp:Label  ID="lbl_Key" Text='<%# Eval("SettingDefinition.Key") %>'
						                runat="server" />
				                </ItemTemplate>
				                <ItemStyle CssClass="gridStyle-item-td Key-Css" />
				                <HeaderStyle CssClass="gridStyle-header-th Key-Css" Wrap="true" Width="25%"/>
			                </asp:TemplateField>
			                <asp:TemplateField HeaderText="Description">
				                <ItemTemplate>
					                <asp:Label  ID="lbl_Description" Text='<%# Eval("SettingDefinition.Description") %>'
						                runat="server" />
				                </ItemTemplate>
				                <ItemStyle CssClass="gridStyle-item-td Description-Css" />
				                <HeaderStyle CssClass="gridStyle-header-th Description-Css" Wrap="true" Width="35%"/>
			                </asp:TemplateField>
			                <asp:TemplateField HeaderText="IIS Reset?">
				                <ItemTemplate>
					                <asp:Label  ID="lbl_IISReset" Text='<%# ((bool)Eval("SettingDefinition.RequiresIISReset"))? "Yes" : "No" %>'
						                runat="server" />
				                </ItemTemplate>
				                <ItemStyle CssClass="gridStyle-item-td IISReset-Css" />
				                <HeaderStyle CssClass="gridStyle-header-th IISReset-Css" Wrap="true" Width="5%"/>
			                </asp:TemplateField>
			                <asp:TemplateField HeaderText="Value">
				                <ItemTemplate>
					                <asp:TextBox id="txt_Value" TextMode="Multiline" runat="server" Width="95%" Text='<%# Eval("Value") %>'/>
                                    <asp:RequiredFieldValidator ID="vld_txt_Value_NotEmpty" Text="Field is required"  ControlToValidate="txt_Value"
                                    runat="server" Display="Dynamic"/>
                                    <asp:CustomValidator ID="vld_txt_Value" runat="server"
                                    CssClass="ErrorMessage" ControlToValidate="txt_Value" Display="Dynamic"/>
				                </ItemTemplate>
				                <ItemStyle CssClass="gridStyle-item-td Value-Css" />
				                <HeaderStyle CssClass="gridStyle-header-th Value-Css" Wrap="true" Width="25%"/>
			                </asp:TemplateField>
		                </Columns>
	                 </DevelopmentSimplyPutWebControls:EnhancedDataGrid>
	            </div>
	        </td>
	    </tr>
	    <tr>
	        <td style="text-align:right">
	            <asp:HiddenField ID="hdn_SubmitClicked" Value="0" runat="server" />
	            <asp:Button class="form-button" Text="Submit Changes" ID="btn_Submit" OnClick="btn_Submit_Click" runat="server" />
	        </td>
	    </tr>
	</table>
</asp:content>