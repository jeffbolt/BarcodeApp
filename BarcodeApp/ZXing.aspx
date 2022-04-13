<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ZXing.aspx.cs" Inherits="BarcodeApp.BarcodeZXing" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
	<script type="text/javascript" src="/Scripts/Barcode.js"></script>

	<h2><%: Title %></h2>
	<h5>Barcode and QR code demos using ZXing.NET</h5>
	<h4>Create a test barcode</h4>
	<div class="tile">
		<asp:UpdatePanel ID="upUploadBarcode" runat="server" UpdateMode="Conditional">
			<ContentTemplate>
				<asp:FileUpload ID="FileUploadBarcode" runat="server" CssClass="hidden" />
				<input type="button" id="btnUploadBarcode" runat="server" class="btn btn-default min-width-100" value="Open File..." />
			</ContentTemplate>
			<Triggers>
				<asp:PostBackTrigger ControlID="btnUploadBarcode" />
			</Triggers>
		</asp:UpdatePanel>
		<asp:Panel ID="pnlTempBarcode" runat="server" Visible="false">
			<table class="form-table">
				<tr>
					<td>Encoder Type:</td>
					<td>
						<asp:Label ID="lblEncodeType" runat="server"></asp:Label>
					</td>
				</tr>
				<tr>
					<td>Code Text:</td>
					<td>
						<asp:Label ID="lblCodeText" runat="server"></asp:Label>
					</td>
				</tr>
			</table>
		</asp:Panel>
		<div class="tile">
			<asp:Panel ID="pnlTempImage" runat="server" Visible="false">
				<asp:Image ID="imgTempImage" runat="server" BorderWidth="1" />
			</asp:Panel>
		</div>
		<p>
			<asp:Label ID="lblTempImageError" runat="server" ForeColor="Red"></asp:Label>
		</p>
	</div>
	<div class="clear-both"></div>
	<div style="height: 40px"></div>
	<h4>Create a new barcode</h4>
	<div class="tile">
		<table class="form-table">
			<tr>
				<td>Encoder Type:</td>
				<td>
					<asp:DropDownList ID="ddlEncodeType" runat="server" Width="300"></asp:DropDownList>
				</td>
			</tr>
			<tr>
				<td>Code Text:</td>
				<td>
					<asp:TextBox ID="txtCodeText" runat="server" Width="300"></asp:TextBox>
				</td>
			</tr>
		</table>
		<div class="padb4">
			<asp:Button ID="btnCreate" runat="server" Text="Create" CssClass="btn btn-default min-width-100" OnClick="btnCreate_Click" />
		</div>
	</div>
	<div class="tile">
		<asp:Panel ID="pnlNewImage" runat="server" Width="300" Visible="false">
			<asp:Image ID="imgNewImage" runat="server" Width="300" BorderWidth="1" />
		</asp:Panel>
	</div>
	<div class="clear-both"></div>
	<p>
		<asp:Label ID="lblNewImageError" runat="server" ForeColor="Red"></asp:Label>
	</p>
</asp:Content>
