<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="CreateClient.aspx.cs" Inherits="AvaChemAdminPanelMobiAPI.CreateClient" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>AvaChem | Create Client</title>
    <script src="assets/global/scripts/alertJquery.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- BEGIN CONTENT BODY -->
    <div class="page-content">
        <!-- BEGIN PAGE HEADER-->
        <h1 class="page-title">Create Client
                        <small>create client in the system</small>
        </h1>
        <div class="page-bar">
            <ul class="page-breadcrumb">
                <li>
                    <i class="icon-home"></i>
                    <a href="<%= DASHBOARD %>">Home</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <a href="<%= CLIENTS %>">Clients</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <span>Create Client</span>
                </li>
            </ul>
        </div>
        <!-- END PAGE HEADER-->
        <!-- START OF Client PORTLET -->
        <div class="row">
            <div class="col-md-12">
                <div class="alert alert-success" runat="server" id="successLabel"></div>
                <div class="alert alert-danger" runat="server" id="warningLabel"></div>
            </div>
            <div class="col-md-12">
                <div class="portlet light">
                    <div class="portlet-title">
                        <div class="caption font-dark">
                            <i class="icon-folder-alt font-dark"></i>
                            <span class="caption-subject bold uppercase">Create Client</span>
                        </div>
                    </div>
                    <div class="portlet-body form" runat="server">
                        <div class="form-horizontal">
                            <div class="form-body">
                                <div class="form-group">
                                    <label class="col-md-3 control-label">
                                        Company Name
                                        <label style="color: red">*</label>:</label>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" placeholder="Enter company name" ID="tbxCompanyName" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator
                                            runat="server"
                                            Display="Dynamic"
                                            CssClass="txt-error"
                                            ControlToValidate="tbxCompanyName"
                                            ErrorMessage="Please fill in the blank!"></asp:RequiredFieldValidator>
                                        <br />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-3 control-label">
                                        Location<label style="color: red">*</label>:</label>
                                    <div class="col-md-5">
                                        <asp:TextBox class="form-control" placeholder="Enter location" ID="tbxLocation" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator
                                            runat="server"
                                            Display="Dynamic"
                                            CssClass="txt-error"
                                            ControlToValidate="tbxLocation"
                                            ErrorMessage="Please fill in the blank!"></asp:RequiredFieldValidator>
                                        <br />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-3 control-label">
                                        Primary Contact Name 
                                        <label style="color: red">*</label>:</label>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" placeholder="Enter primary contact name" ID="tbxPrimaryContactName" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator
                                            runat="server"
                                            Display="Dynamic"
                                            CssClass="txt-error"
                                            ControlToValidate="tbxPrimaryContactName"
                                            ErrorMessage="Please fill in the blank!"></asp:RequiredFieldValidator>
                                        <br />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-3 control-label">
                                        Primary Contact Details<label style="color: red">*</label>:</label>
                                    <div class="col-md-4">
                                        <textarea rows="4" cols="20" id="tbxPrimaryContactDetails" class="form-control" placeholder="Enter primary contact details" runat="server"></textarea>
                                        <asp:RequiredFieldValidator
                                            runat="server"
                                            Display="Dynamic"
                                            CssClass="txt-error"
                                            ControlToValidate="tbxPrimaryContactDetails"
                                            ErrorMessage="Please fill in the blank!"></asp:RequiredFieldValidator>
                                        <br />
                                        <%--<asp:TextBox class="form-control" placeholder="Enter primary contact details" ID="tbxPrimaryContactDetails" runat="server"></asp:TextBox><br />--%>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-3 control-label">Secondary Contact Name:</label>
                                    <div class="col-md-4">
                                        <asp:TextBox class="form-control" placeholder="Enter secondary contact name" ID="tbxSecondaryContactName" runat="server"></asp:TextBox><br />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-3 control-label">Secondary Contact Details:</label>
                                    <div class="col-md-4">
                                        <textarea rows="4" cols="20" id="tbxSecondaryContactDetails" class="form-control" placeholder="Enter secondary contact details" runat="server"></textarea><br />
                                        <%--<asp:TextBox class="form-control" placeholder="Enter secondary contact details" ID="tbxSecondaryContactDetails" runat="server"></asp:TextBox><br />--%>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-md-offset-3 col-md-10">
                                        <asp:Button CausesValidation="False" CssClass="btn red btn-outline" ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                                        <asp:Button CssClass="btn green uppercase" ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- END OF Client PORTLET -->
    </div>
    <!-- END CONTENT BODY -->
</asp:Content>
