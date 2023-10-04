<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="Credits.aspx.cs" Inherits="AvaChemAdminPanelMobiAPI.Credits" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>AvaChem | Credits</title>
    <script src="assets/global/scripts/alertJquery.js"></script>

    <%--<link href="assets/global/plugins/bootstrap-datepicker/css/bootstrap-datepicker.min.css" rel="stylesheet" type="text/css" />
    <script src="assets/global/plugins/bootstrap-datepicker/js/bootstrap-datepicker.min.js" type="text/javascript"></script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- BEGIN CONTENT BODY -->
    <div class="page-content">
        <!-- BEGIN PAGE HEADER-->
        <h1 class="page-title">Credits
                    <small>update credits</small>
        </h1>
        <div class="page-bar">
            <ul class="page-breadcrumb">
                <li>
                    <i class="icon-home"></i>
                    <a href="<%= DASHBOARD %>">Home</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <span>Credits</span>
                </li>
            </ul>

            <% if (new int[] { UserRoles.SuperAdmin.GetHashCode(), UserRoles.OverallAdmin.GetHashCode(), UserRoles.CreditAdmin.GetHashCode() }.Contains(role))
                { %>
            <div class="page-toolbar">
                <div class="btn-group pull-right">
                    <button type="button" class="btn btn-fit-height grey-salt dropdown-toggle" data-toggle="dropdown" data-hover="dropdown" data-delay="1000" data-close-others="true">
                        Actions
                                <i class="fa fa-angle-down"></i>
                    </button>
                    <ul class="dropdown-menu pull-right" role="menu">
                        <li>
                            <a href="<%= CREATE_CREDIT %>"><i class="icon-plus"></i>
                                Update Credits For User</a>
                        </li>
                    </ul>
                </div>
            </div>
            <% } %>
        </div>

        <div class="row">
            <div class="col-md-12">
                <div class="alert alert-success" runat="server" id="successLabel"></div>
                <div class="alert alert-danger" runat="server" id="warningLabel"></div>
            </div>
        </div>

        <!-- START OF USERS SEARCH FORM -->

        <div class="caption font-dark">
            <div class="input-group" style="width: 100%">
                <%--<div class="col-sm-5 col-xs-12" style="padding-left: 0px; padding-right: 0px;">
                    <asp:TextBox ID="tbxSearch" runat="server" CssClass="form-control" Style="width: 100%" Type="search" Placeholder="Search Credit Record (Description, User Name, User ID Number)"></asp:TextBox>
                </div>

                <div class="col-sm-5 col-xs-12" style="padding-left: 5px; padding-right: 0px;">
                    <div class="input-group input-daterange">
                        <input readonly="readonly" autocomplete="off" placeholder="Start Date" type="text" class="form-control" value="" id="dtStartDate" runat="server" style="background: #fff; cursor: pointer">
                        <div class="input-group-addon">to</div>
                        <input readonly="readonly" autocomplete="off" placeholder="To Date" type="text" class="form-control" value="" id="dtEndDate" runat="server" style="background: #fff; cursor: pointer">
                    </div>
                </div>

                <div class="col-sm-2 col-xs-12" style="padding-left: 10px; padding-right: 0px;">
                    <asp:Button ID="lbtnSearch" CssClass="btn btn-primary" runat="server" Style="width: 100%" Text="Search" OnClick="lbtnSearch_Click" />
                </div>--%>

                <div class="col-sm-5 col-xs-12" style="padding-left: 5px; padding-right: 0px;">
                    <asp:TextBox ID="tbxSearch" runat="server" CssClass="form-control" Style="width: 100%" Type="search" Placeholder="Search (Staff Name)"></asp:TextBox>
                </div>

                <div class="col-sm-5 col-xs-12" style="padding-left: 5px; padding-right: 0px;">
                    <div class="col-sm-6" style="padding-left: 0px; padding-right: 0px;">
                        <asp:DropDownList ID="ddlMonth" ClientIDMode="Static" runat="server" class="form-control"></asp:DropDownList>
                    </div>
                    <div class="col-sm-6" style="padding-left: 0px; padding-right: 0px;">
                        <asp:DropDownList ID="ddlYear" ClientIDMode="Static" runat="server" class="form-control"></asp:DropDownList>
                    </div>
                </div>

                <div class="col-sm-2 col-xs-12" style="padding-left: 10px; padding-right: 0px;">
                    <asp:Button ID="lbtnSearch" CssClass="btn btn-primary" runat="server" Style="width: 100%" Text="Search" OnClick="lbtnSearch_Click" />
                </div>
            </div>
        </div>
        <br />
        <!-- END OF USERS SEARCH FORM -->

        <div class="portlet box blue">
            <div class="portlet-title" style="display: flex; align-items: center;">
                <div class="caption">
                    <span>All Credits</span>
                </div>
                <div style="padding-left: 10px;">
                    <asp:DropDownList ID="ddlSort" ClientIDMode="Static" runat="server" class="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlSort_SelectedIndexChanged">
                        <asp:ListItem Enabled="true" Text="-- Sort --" Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Earliest to latest" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Latest to earliest" Value="2"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="portlet-body">
                <div class="table-scrollable">
                    <table class="table table-striped table-bordered table-hover">
                        <%if (list.Count == 0)
                            { %>
                        <% Response.Write("<div class='alert alert-info'><p>There are currently no credits<p></div>"); %>
                        <% }
                            if (list.Count > 0)
                            { %>
                        <thead>
                            <tr>
                                <th class="text-center">No.</th>
                                <th>User</th>
                                <th class="text-center">Amount</th>
                                <th>Description</th>
                                <th>Date </th>
                                <%--<th class="text-center">Update</th>
                                <th class="text-center">Delete</th>--%>
                            </tr>
                        </thead>
                        <tbody>
                            <%foreach (CreditTableView c in list)
                                {
                                    i++; %>
                            <tr>
                                <td class="text-center"><% Response.Write(i); %></td>
                                <td><% Response.Write(c.UName + " (" + c.UIDNumber + ")"); %></td>
                                <td class="text-center">
                                    <div style="pointer-events: none; cursor: none; font-weight: bold; <% Response.Write(c.Amount > 0 ? "border-color: #6BAC34; color: #6BAC34": "border-color: #E83215; color: #E83215"); %>" class="<% Response.Write(c.Amount > 0 ? "btn btn-outline" : "btn btn-outline"); %>">
                                        <% Response.Write(c.Amount > 0 ? $"+{c.Amount}" : $"{c.Amount}"); %>
                                    </div>
                                </td>
                                <td><% Response.Write(c.Description); %></td>
                                <td><% Response.Write(c.CreatedDate.ToString("dd-MM-yyyy HH:mm")); %></td>
                                <%--<td class="text-center"><a class="btn blue" href="<%= UPDATE_CREDIT + "?id=" + c.ID %>"><i class="fa fa-upload"></i></a></td>
                                <td class="text-center"><a class="btn red" id='"<% Response.Write(c.ID); %>"' data-toggle='modal' onclick='DeleteId_Click("<% Response.Write(c.ID); %>")' data-target='#deleteModal'><i class="fa fa-trash"></i></a></td>--%>
                            </tr>
                            <% } %>
                        </tbody>
                        <% } %>
                    </table>
                </div>
            </div>
        </div>
        <!-- END SAMPLE TABLE PORTLET-->
        <%--<!-- START OF DELETE MODAL -->
        <div class="modal fade" id="deleteModal" tabindex="-1" role="dialog" aria-labelledby="deleteModalLabel" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="deleteModalLabel">Confirmation</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <p>Are you sure you want to delete this credit record?</p>
                    </div>
                    <div class="modal-footer">
                        <input type="hidden" name="deleteInput" id="deleteInput" value="" />
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                        <asp:Button ID="deleteConfirm" runat="server" Text="Confirm" CssClass="btn btn-danger" OnClick="deleteConfirm_Click" />
                    </div>
                </div>
            </div>
        </div>
        <!-- END OF DELETE MODAL -->--%>
    </div>

    <%--<script>
        function DeleteId_Click(val) {
            $('#deleteInput').val(val);
        }

        // Document ready
        $(function () {
            $('.input-daterange input').each(function () {
                $(this).datepicker({
                    autoclose: true,
                    clearBtn: true,
                    orientation: "bottom",
                });
            });
        });
    </script>--%>
</asp:Content>

