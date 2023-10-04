<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="UpdateCredit.aspx.cs" Inherits="AvaChemAdminPanelMobiAPI.UpdateCredit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>AvaChem | Update Credit</title>
    <script src="assets/global/scripts/alertJquery.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- BEGIN CONTENT BODY -->
    <div class="page-content">
        <!-- BEGIN PAGE HEADER-->
        <h1 class="page-title">Update Credit
                        <small>update credit in the system</small>
        </h1>
        <div class="page-bar">
            <ul class="page-breadcrumb">
                <li>
                    <i class="icon-home"></i>
                    <a href="<%= DASHBOARD %>">Home</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <a href="<%= CREDITS %>">Credits</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <span>Update Credit</span>
                </li>
            </ul>
        </div>
        <!-- END PAGE HEADER-->
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
                            <span class="caption-subject bold uppercase">Update Credit</span>
                        </div>
                    </div>
                    <div class="portlet-body form" runat="server">
                        <div class="form-horizontal">
                            <div class="form-body">
                                <div class="form-group">
                                    <label class="col-md-3 control-label">User<label style="color: red">*</label>:</label>
                                    <div class="col-md-5">
                                        <asp:DropDownList class="form-control" ID="ddlUser" ClientIDMode="Static" runat="server" OnChange="handleUser_Change()"></asp:DropDownList>
                                        <asp:RequiredFieldValidator
                                            runat="server"
                                            Display="Dynamic"
                                            CssClass="txt-error"
                                            ControlToValidate="ddlUser"
                                            ErrorMessage="Please fill in the blank!"></asp:RequiredFieldValidator>
                                        <span class="caption bold" id="spnCurrentCredits" style="padding-left: 12px"></span>
                                        <br />
                                        <br />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-3 control-label">Type<label style="color: red">*</label>:</label>
                                    <div class="col-md-5">
                                        <input id="Type" runat="server" clientidmode="Static" style="display: none" />
                                        <button id="btnTypeAdd" type="button" class="btn btn-outline bold" style="border-color: #ddd; background: #ddd; color: #fff;" onclick="Add_Click()">
                                            <div style="font-size: 25px; line-height: 20px;"><i class="fa fa-plus" aria-hidden="true"></i></div>
                                        </button>
                                        <button id="btnTypeMinus" type="button" class="btn btn-outline bold" style="border-color: #ddd; background: #ddd; color: #fff;" onclick="Minus_Click()">
                                            <div style="font-size: 25px; line-height: 20px;"><i class="fa fa-minus" aria-hidden="true"></i></div>
                                        </button>
                                        <asp:RequiredFieldValidator
                                            ID="requiredType"
                                            ClientIDMode="Static"
                                            runat="server"
                                            Display="Dynamic"
                                            CssClass="txt-error"
                                            ControlToValidate="Type"
                                            ErrorMessage="Please fill in the blank!"></asp:RequiredFieldValidator>
                                        <br />
                                        <br />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-3 control-label">Amount<label style="color: red">*</label>:</label>
                                    <div class="col-md-5">
                                        <input type="number" class="form-control" autocomplete="Off" placeholder="Enter amount" id="tbxAmount" clientidmode="Static" runat="server">
                                        <asp:RequiredFieldValidator
                                            runat="server"
                                            Display="Dynamic"
                                            CssClass="txt-error"
                                            ControlToValidate="tbxAmount"
                                            ErrorMessage="Please fill in the blank!"></asp:RequiredFieldValidator>
                                        <br />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-3 control-label">Description<label style="color: red">*</label>:</label>
                                    <div class="col-md-5">
                                        <textarea rows="4" class="form-control" type="text" placeholder="Enter description" id="tbxDescription" runat="server"></textarea>
                                        <asp:RequiredFieldValidator
                                            runat="server"
                                            Display="Dynamic"
                                            CssClass="txt-error"
                                            ControlToValidate="tbxDescription"
                                            ErrorMessage="Please fill in the blank!"></asp:RequiredFieldValidator>
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
    <script>
        var ddlUser_id = '#ddlUser';
        var btnTypeAdd_id = '#btnTypeAdd';
        var btnTypeMinus_id = '#btnTypeMinus';
        var hidType_id = '#Type'
        var spnCurrentCredits_id = '#spnCurrentCredits';
        var tbxAmount_id = '#tbxAmount';

        // Document ready
        $(function () {
            $(ddlUser_id).select2();
            try {
                handleUser_Change();

                var initAmount = +$(tbxAmount_id).val();
                if (initAmount >= 0) Add_Click();
                else Minus_Click();

                $(tbxAmount_id).val(Math.abs(initAmount));
            } catch (e) {
            }
        });

        function handleUser_Change() {
            var selectedUser = $(ddlUser_id).val();
            try {
                selectedUser = JSON.parse(selectedUser);
                $(spnCurrentCredits_id).text("Credits: " + selectedUser.Credits);
            } catch (e) {
            }
        }

        function Add_Click() {
            $('#requiredType').hide();
            $(hidType_id).val("+");
            $(btnTypeAdd_id).css({
                background: '#6BAC34',
                color: '#fff'
            })
            $(btnTypeMinus_id).css({
                background: '#ddd',
                'border-color': '#ddd',
                color: '#fff',
            })
        }

        function Minus_Click() {
            $('#requiredType').hide();
            $(hidType_id).val("-");
            $(btnTypeMinus_id).css({
                background: '#E83215',
                color: '#fff'
            })
            $(btnTypeAdd_id).css({
                background: '#ddd',
                'border-color': '#ddd',
                color: '#fff',
            })
        }
    </script>
</asp:Content>
