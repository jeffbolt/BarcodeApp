<%@ Page Title="Barcode" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Barcode.aspx.cs" Inherits="BarcodeApp.Barcode" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
	<h2><%: Title %></h2>
	<h3>Barcode and QR code demos.</h3>
	<h4>Create a test barcode</h4>
	<div class="float-left" style="width: 425px">
		<table class="form-table">
			<tr>
				<td>Encode Type:</td>
				<td>
					<asp:DropDownList ID="ddlEncodeType" runat="server" Width="300"></asp:DropDownList></td>
			</tr>
			<tr>
				<td>Value:</td>
				<td>
					<asp:TextBox ID="txtValue" runat="server" TextMode="MultiLine" Width="300"></asp:TextBox></td>
			</tr>
		</table>
		<div class="padb4">
			<asp:Button ID="btnCreate" runat="server" Text="Create" Width="100" OnClick="btnCreate_Click" />
		</div>
	</div>
	<div class="float-left" style="width: 425px">
		<asp:Panel ID="pnlResult" runat="server" Width="300" Visible="false">
			<asp:Image ID="imgResult" runat="server" Width="300" BorderWidth="1" />
		</asp:Panel>
	</div>
	<div class="clear-both"></div>
</asp:Content>
