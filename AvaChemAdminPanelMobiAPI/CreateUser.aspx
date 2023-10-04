<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="CreateUser.aspx.cs" Inherits="AvaChemAdminPanelMobiAPI.CreateUser" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>AvaChem | Create User</title>
    <script src="assets/global/scripts/alertJquery.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- BEGIN CONTENT BODY -->
    <div class="page-content">
        <!-- BEGIN PAGE HEADER-->
        <h1 class="page-title">Create User
                        <small>create user in the system</small>
        </h1>
        <div class="page-bar">
            <ul class="page-breadcrumb">
                <li>
                    <i class="icon-home"></i>
                    <a href="<%= DASHBOARD %>">Home</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <a href="<%= USERS %>">Users</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <span>Create User</span>
                </li>
            </ul>
        </div>
        <!-- END PAGE HEADER-->
        <!-- START OF CREATE USER PORTLET -->
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
                            <span class="caption-subject bold uppercase">Create User</span>
                        </div>
                    </div>
                    <div class="portlet-body form" runat="server">
                        <div class="form-horizontal">
                            <div class="form-body">
                                <div class="form-group">
                                    <label class="col-md-3 control-label">
                                        Role<label style="color: red">*</label>:</label>
                                    <div class="col-md-5">
                                        <asp:DropDownList class="form-control" ID="ddlRole" ClientIDMode="Static" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator
                                            runat="server"
                                            Display="Dynamic"
                                            CssClass="txt-error"
                                            ControlToValidate="ddlRole"
                                            ErrorMessage="Please fill in the blank!"></asp:RequiredFieldValidator>
                                        <br />
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="col-md-3 control-label">
                                        Username<label style="color: red">*</label>:</label>
                                    <div class="col-md-5">
                                        <asp:TextBox autocomplete="off" class="form-control" placeholder="Enter username" ID="tbxUsername" runat="server"></asp:TextBox>
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
                                        Pasword<label style="color: red">*</label>:</label>
                                    <div class="col-md-5">
                                        <asp:TextBox autocomplete="off" class="form-control" Type="Password" placeholder="Enter password" ID="tbxPassword" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator
                                            runat="server"
                                            Display="Dynamic"
                                            CssClass="txt-error"
                                            ControlToValidate="tbxPassword"
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
                                <%--<div class="form-group">
                                    <label class="col-md-3 control-label">
                                        Date of Birth<label style="color: red">*</label>:</label>
                                    <div class="col-md-5">
                                        <input id="dtDob" type="date" name="name" value="" runat="server" />
                                        <asp:RequiredFieldValidator
                                            runat="server"
                                            Display="Dynamic"
                                            CssClass="txt-error"
                                            ControlToValidate="dtDob"
                                            ErrorMessage="Please fill in the blank!"></asp:RequiredFieldValidator>
                                        <br />
                                        <br />
                                    </div>
                                </div>--%>
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
                                    <label class="col-md-3 control-label">
                                        Total Credits:</label>
                                    <div class="col-md-5">
                                        <asp:TextBox class="form-control" placeholder="Enter total credits" ID="tbxCredits" runat="server"></asp:TextBox>
                                        <br />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-3 control-label">
                                        Leave Days Left<label style="color: red">*</label>:</label>
                                    <div class="col-md-5">
                                        <asp:TextBox class="form-control" placeholder="Enter leave day left" ID="tbxLeaveDaysLeft" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator
                                            runat="server"
                                            Display="Dynamic"
                                            CssClass="txt-error"
                                            ControlToValidate="tbxLeaveDaysLeft"
                                            ErrorMessage="Please fill in the blank!"></asp:RequiredFieldValidator>
                                        <br />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-3 control-label">
                                        MC Days Left<label style="color: red">*</label>:</label>
                                    <div class="col-md-5">
                                        <asp:TextBox class="form-control" placeholder="Enter MC days left" ID="tbxMCDaysLeft" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator
                                            runat="server"
                                            Display="Dynamic"
                                            CssClass="txt-error"
                                            ControlToValidate="tbxMCDaysLeft"
                                            ErrorMessage="Please fill in the blank!"></asp:RequiredFieldValidator>
                                        <br />
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="col-md-3 control-label">Qualifications:</label>
                                    <input id="QValues" clientidmode="Static" type="hidden" runat="server" />
                                    <div class="col-md-5">
                                        <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#updateQualificationModal" onclick="AddQ_Click()"><i class="fa fa-plus" aria-hidden="true"></i></button>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-3 control-label"></label>
                                    <div class="col-md-5">
                                        <div class="table-responsive">
                                            <table class="table table-striped table-bordered table-hover" style="font-size: 13px">
                                                <thead>
                                                    <tr>
                                                        <th>Name</th>
                                                        <th>Date Obtained</th>
                                                        <th>Expiry Date</th>
                                                        <th class="text-center">
                                                            <button type="button" class="btn btn-primary btn-sm" style="pointer-events: none"><i class="fa fa-pencil" aria-hidden="true"></i></button>
                                                        </th>
                                                        <th class="text-center">
                                                            <button type="button" class="btn btn-danger btn-sm" style="pointer-events: none"><i class="fa fa-times" aria-hidden="true"></i></button>
                                                        </th>
                                                    </tr>
                                                </thead>
                                                <tbody id="tbdQ"></tbody>
                                            </table>
                                        </div>
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
        <!-- END OF Update USER PORTLET -->

        <!-- START OF ADD QUALIFICATION MODAL -->
        <div class="modal fade" id="updateQualificationModal" clientidmode="Static" tabindex="-1" role="dialog" aria-labelledby="update-q-modalLabel" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <span class="caption-subject bold uppercase">Create Qualification</span>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="form-body">
                            <input id="SubmitQID" clientidmode="Static" type="hidden" runat="server" />
                            <div class="form-group row">
                                <label class="col-md-3 control-label" style="padding-top: 7px">Name<label style="color: red">*</label>:</label>
                                <div class="col-md-8">
                                    <asp:TextBox autocomplete="off" class="form-control" placeholder="Enter Name" ID="tbxQName" ClientIDMode="Static" runat="server"></asp:TextBox><br />
                                </div>
                            </div>
                            <div class="form-group row">
                                <label class="col-md-3 control-label" style="padding-top: 7px">Date Obtained<label style="color: red">*</label>:</label>
                                <div class="col-md-8">
                                    <input autocomplete="off" id="tbxQDateObtained" clientidmode="Static" type="date" value="" runat="server" /><br />
                                    <br />
                                </div>
                            </div>
                            <div class="form-group row">
                                <label class="col-md-3 control-label" style="padding-top: 7px">Expiry Date<label style="color: red">*</label>:</label>
                                <div class="col-md-8">
                                    <input autocomplete="off" id="tbxQExpiryDate" clientidmode="Static" type="date" value="" runat="server" /><br />
                                    <br />
                                </div>
                            </div>

                            <div id="grpQModalError" clientidmode="Static" class="form-group row">
                                <label class="col-md-12 control-label" style="padding-top: 7px">
                                    <span id="spnQModalError" clientidmode="Static" class="text-danger"></span>
                                </label>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button>
                        <button type="button" class="btn btn-primary" onclick="SubmitQ_Click()">SUBMIT</button>
                    </div>
                </div>
            </div>
        </div>
        <!-- END OF ADD QUALIFICATION MODAL -->
    </div>
    <!-- END CONTENT BODY -->

    <script>
        var enumActionTypes = <%= dataActionTypes %>;

        //#region Qualification handlers
        function SubmitQ_Click() {
            submitQ_Click(enumActionTypes);
        }
        function AddQ_Click() {
            addQ_Click();
        }
        function EditQ_Click(id) {
            editQ_Click(id);
        }
        function DeleteQ_Click(id) {
            deleteQ_Click(id, enumActionTypes);
        }
        //#endregion Qualification handlers

    </script>
    <script src="assets/pages/scripts/user/qualification.js"></script>
</asp:Content>

