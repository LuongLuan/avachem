<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="AvaChemAdminPanelMobiAPI.Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>AvaChem | Update Proflie</title>
    <script src="assets/global/scripts/alertJquery.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- BEGIN CONTENT BODY -->
    <div class="page-content">
        <!-- BEGIN PAGE HEADER-->
        <h1 class="page-title">Update Profile 
                        <small></small>
        </h1>
        <div class="page-bar">
            <ul class="page-breadcrumb">
                <li>
                    <i class="icon-home"></i>
                    <a href="<%= DASHBOARD %>">Home</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <a href="<%= PROFILE %>">Profile</a>
                    <i class="fa fa-angle-right"></i>
                </li>
            </ul>
        </div>
        <!-- END PAGE HEADER-->
        <!-- START OF Profile DETAILS PORTLET -->
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
                            <span class="caption-subject bold uppercase">Update Profile</span>
                        </div>
                    </div>
                    <div class="portlet-body form">
                        <div class="form-horizontal">
                            <div class="form-body">
                                <div class="form-group">
                                    <label class="col-md-3 control-label">
                                        Username<label style="color: red">*</label>:</label>
                                    <div class="col-md-5">
                                        <asp:TextBox class="form-control" placeholder="Enter username" ID="tbxUsername" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator
                                            runat="server"
                                            Display="Dynamic"
                                            CssClass="txt-error"
                                            ControlToValidate="tbxUsername"
                                            ErrorMessage="Please fill in the blank!"></asp:RequiredFieldValidator>
                                        <br />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-3 control-label">
                                        ID number<label style="color: red">*</label>:</label>
                                    <div class="col-md-5">
                                        <asp:TextBox class="form-control" placeholder="Enter ID number" ID="tbxIDNumber" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator
                                            runat="server"
                                            Display="Dynamic"
                                            CssClass="txt-error"
                                            ControlToValidate="tbxIDNumber"
                                            ErrorMessage="Please fill in the blank!"></asp:RequiredFieldValidator>
                                        <br />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-3 control-label">
                                        Name<label style="color: red">*</label>:</label>
                                    <div class="col-md-5">
                                        <asp:TextBox class="form-control" placeholder="Enter name" ID="tbxName" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator
                                            runat="server"
                                            Display="Dynamic"
                                            CssClass="txt-error"
                                            ControlToValidate="tbxName"
                                            ErrorMessage="Please fill in the blank!"></asp:RequiredFieldValidator>
                                        <br />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-3 control-label">
                                        Phone Number<label style="color: red">*</label>:</label>
                                    <div class="col-md-5">
                                        <asp:TextBox class="form-control" placeholder="Enter phone number" ID="tbxPhone" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator
                                            runat="server"
                                            Display="Dynamic"
                                            CssClass="txt-error"
                                            ControlToValidate="tbxPhone"
                                            ErrorMessage="Please fill in the blank!"></asp:RequiredFieldValidator>
                                        <br />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-3 control-label">
                                        Email:</label>
                                    <div class="col-md-5">
                                        <asp:TextBox class="form-control" placeholder="Enter email" ID="tbxEmail" runat="server"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator
                                            runat="server"
                                            Display="Dynamic"
                                            CssClass="txt-error"
                                            ControlToValidate="tbxEmail"
                                            ErrorMessage="Please fill in the blank!"></asp:RequiredFieldValidator>--%>
                                        <br />
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-md-offset-3 col-md-9">
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

    </div>
</asp:Content>
