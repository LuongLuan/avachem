<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="Users.aspx.cs" Inherits="AvaChemAdminPanelMobiAPI.Users" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>AvaChem | Users</title>
    <script src="assets/global/scripts/alertJquery.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- BEGIN CONTENT BODY -->
    <div class="page-content">
        <!-- BEGIN PAGE HEADER-->
        <h1 class="page-title">Users
                    <small>create, update or delete user</small>
        </h1>
        <div class="page-bar">
            <ul class="page-breadcrumb">
                <li>
                    <i class="icon-home"></i>
                    <a href="<%= DASHBOARD %>">Home</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <span>Users</span>
                </li>
            </ul>

            <% if (new int[] { UserRoles.SuperAdmin.GetHashCode(), UserRoles.HR.GetHashCode() }.Contains(role))
                { %>
            <div class="page-toolbar">
                <div class="btn-group pull-right role">
                    <button type="button" class="btn btn-fit-height grey-salt dropdown-toggle" data-toggle="dropdown" data-hover="dropdown" data-delay="1000" data-close-others="true">
                        Actions
                                <i class="fa fa-angle-down"></i>
                    </button>
                    <ul class="dropdown-menu pull-right" role="menu">
                        <li>
                            <a href="<%= CREATE_USER %>"><i class="icon-plus"></i>
                                Create A User</a>
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
                <div class="col-sm-5 col-xs-12" style="padding-left: 0px; padding-right: 0px;">
                    <asp:TextBox ID="tbxSearch" runat="server" CssClass="form-control" Style="width: 100%" Type="search" Placeholder="Search User (ID Number, Name, Phone, Email)"></asp:TextBox>
                </div>

                <%--<div class="col-sm-5 col-xs-12" style="padding-left: 5px; padding-right: 0px;">
                    <div class="input-group input-daterange">
                        <input readonly="readonly" autocomplete="off" placeholder="Start Date" type="text" class="form-control" value="" id="dtStartDate" runat="server" style="background: #fff; cursor: pointer">
                        <div class="input-group-addon">to</div>
                        <input readonly="readonly" autocomplete="off" placeholder="To Date" type="text" class="form-control" value="" id="dtEndDate" runat="server" style="background: #fff; cursor: pointer">
                    </div>
                </div>--%>

                <div class="col-sm-2 col-xs-12" style="padding-left: 10px; padding-right: 0px;">
                    <asp:Button ID="Button1" CssClass="btn btn-primary" runat="server" Style="width: 100%" Text="Search" OnClick="lbtnSearch_Click" />
                </div>
            </div>
        </div>
        <br />
        <!-- END OF USERS SEARCH FORM -->

        <div class="portlet box blue">
            <div class="portlet-title">
                <div class="caption">
                    <span>All Users</span>
                </div>
            </div>
            <div class="portlet-body">
                <div class="table-scrollable">
                    <table class="table table-striped table-bordered table-hover">
                        <%if (list.Count == 0)
                            { %>
                        <% Response.Write("<div class='alert alert-info'><p>There are currently no users<p></div>"); %>
                        <% }
                            if (list.Count > 0)
                            { %>
                        <thead>
                            <tr>
                                <th class="text-center">No.</th>
                                <th>Name</th>
                                <th>ID Number</th>
                                <th class="text-center">Phone</th>
                                <th>Email</th>
                                <th class="text-center">Role</th>
                                <th class="text-center" id="th_action_Detail" runat="server">Details</th>

                                <% if (new int[] { UserRoles.SuperAdmin.GetHashCode(), UserRoles.HR.GetHashCode() }.Contains(role))
                                    { %>
                                <th class="text-center role" id="th_action_Update" runat="server">Update</th>
                                <th class="text-center role" id="th_action_Delete" runat="server">Delete</th>
                                <% } %>
                            </tr>
                        </thead>
                        <tbody>
                            <%foreach (User u in list)
                                {
                                    i++; %>
                            <tr>
                                <td class="text-center"><% Response.Write(i); %></td>
                                <td><% Response.Write(u.Name); %></td>
                                <td><% Response.Write(u.IDNumber); %></td>
                                <td class="text-center"><% Response.Write(u.Phone); %></td>
                                <td><% Response.Write(u.Email); %></td>
                                <td class="text-center"><% Response.Write(((UserRoles)u.RoleID).ToString()); %></td>
                                <td class="text-center"><a class="btn dark btn-outline" href="<%= USER_DETAILS + "?id=" + u.ID %>">Details</a></td>

                                <% if (new int[] { UserRoles.SuperAdmin.GetHashCode(), UserRoles.HR.GetHashCode() }.Contains(role))
                                    { %>
                                <td class="text-center role">
                                    <% if (role != UserRoles.HR.GetHashCode()
                                                    || (role == UserRoles.HR.GetHashCode() && new int[] { UserRoles.Worker.GetHashCode(), UserRoles.Driver.GetHashCode() }.Contains(u.RoleID)))
                                        { %>
                                    <a class="btn blue" href="<%= UPDATE_USER + "?id=" + u.ID %>"><i class="fa fa-upload"></i></a>
                                    <% } %>
                                </td>
                                <td class="text-center role">
                                    <% if (role != UserRoles.HR.GetHashCode()
                                                    || (role == UserRoles.HR.GetHashCode() && new int[] { UserRoles.Worker.GetHashCode(), UserRoles.Driver.GetHashCode() }.Contains(u.RoleID)))
                                        { %>
                                    <a class="btn red" id='"<% Response.Write(u.ID); %>"' data-toggle='modal' onclick='DeleteId_Click("<% Response.Write(u.ID); %>", <% Response.Write(u.RoleID); %>)' data-target='#deleteModal'><i class="fa fa-trash"></i></a>
                                    <% } %>
                                </td>
                                <% } %>
                            </tr>
                            <% } %>
                        </tbody>
                        <% } %>
                    </table>
                </div>
            </div>
        </div>
        <!-- END SAMPLE TABLE PORTLET-->

        <% if (new int[] { UserRoles.SuperAdmin.GetHashCode(), UserRoles.HR.GetHashCode() }.Contains(role))
            { %>
        <!-- START OF DELETE MODAL -->
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
                        <p>Are you sure you want to delete this user?</p>
                    </div>
                    <div class="modal-footer">
                        <input type="hidden" name="deleteInput" id="deleteInput" value="" />
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                        <asp:Button ID="deleteConfirm" runat="server" Text="Confirm" CssClass="btn btn-danger" OnClick="deleteConfirm_Click" />
                    </div>
                </div>
            </div>
        </div>
        <!-- END OF DELETE MODAL -->
        <% } %>
    </div>
    <script>
        function DeleteId_Click(val, roleId) {
            <% var serializer = new System.Web.Script.Serialization.JavaScriptSerializer(); %>
            var validDeletedRoles = <%= serializer.Serialize(new int[] { UserRoles.Worker.GetHashCode(), UserRoles.Driver.GetHashCode() }) %>;
            if (<%= role %> != <%= UserRoles.HR.GetHashCode() %>
                || (<%= role %> == <%= UserRoles.HR.GetHashCode() %> && validDeletedRoles.includes(roleId))) {
                $('#deleteInput').val(val);
            }
        }
    </script>
</asp:Content>
