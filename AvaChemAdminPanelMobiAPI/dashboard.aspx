<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="dashboard.aspx.cs" Inherits="AvaChemAdminPanelMobiAPI.Dashboard" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>AvaChem | Home</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- BEGIN CONTENT BODY -->
    <div class="page-content">
        <!-- BEGIN PAGE HEADER-->
        <h1 class="page-title">Home
                    <small>Admin Dashboard</small>
        </h1>
        <div class="page-bar">
            <ul class="page-breadcrumb">
                <li>
                    <i class="icon-home"></i>
                    <a href="<%= DASHBOARD %>">Home</a>
                </li>
            </ul>
        </div>

        <div class="row">
            <div class="col-md-12">
                <div class="alert alert-success" runat="server" id="successLabel"></div>
                <div class="alert alert-danger" runat="server" id="warningLabel"></div>
            </div>
        </div>

        <!-- END PAGE HEADER-->
        <div class="row">
            <% if (new int[] { UserRoles.SuperAdmin.GetHashCode(), UserRoles.OverallAdmin.GetHashCode(), UserRoles.HR.GetHashCode() }.Contains(role))
                { %>
            <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                <a class="dashboard-stat dashboard-stat-v2 green-dark" href="<%= USERS %>">
                    <div class="visual">
                        <i class="fa fa-credit-card"></i>
                    </div>
                    <div class="details">
                        <div class="number">
                            <span data-counter="counterup" data-value="<%Response.Write(userCount); %>">0</span>
                        </div>
                        <div class="desc" style="font-size: 25px;">Users</div>
                    </div>
                </a>
            </div>
            <% } %>

            <% if (new int[] { UserRoles.SuperAdmin.GetHashCode(), UserRoles.OverallAdmin.GetHashCode(), UserRoles.HR.GetHashCode() }.Contains(role))
                { %>
            <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                <a class="dashboard-stat dashboard-stat-v2 blue-hoki" href="<%= JOBS %>">
                    <div class="visual">
                        <i class="fa fa-home"></i>
                    </div>
                    <div class="details">
                        <div class="number">
                            <span data-counter="counterup" data-value="<%Response.Write(jobCount); %>">0</span>
                        </div>
                        <div class="desc" style="font-size: 25px;">Jobs</div>
                    </div>
                </a>
            </div>
            <% } %>

            <% if (new int[] { UserRoles.SuperAdmin.GetHashCode(), UserRoles.OverallAdmin.GetHashCode(), UserRoles.HR.GetHashCode() }.Contains(role))
                { %>
            <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                <a class="dashboard-stat dashboard-stat-v2 purple" href="<%= OT %>">
                    <div class="visual">
                        <i class="fa fa-sign-out"></i>
                    </div>
                    <div class="details">
                        <div class="number">
                            <span data-counter="counterup" data-value="<%Response.Write(OTCount); %>">0</span>
                        </div>
                        <div class="desc" style="font-size: 25px;">OT</div>
                    </div>
                </a>
            </div>
            <% } %>

            <% if (new int[] { UserRoles.SuperAdmin.GetHashCode(), UserRoles.OverallAdmin.GetHashCode(), UserRoles.HR.GetHashCode() }.Contains(role))
                { %>
            <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                <a class="dashboard-stat dashboard-stat-v2 red-soft" href="<%= LEAVES %>">
                    <div class="visual">
                        <i class="fa fa-map-o"></i>
                    </div>
                    <div class="details">
                        <div class="number">
                            <span data-counter="counterup" data-value="<%Response.Write(leaveCount); %>">0</span>
                        </div>
                        <div class="desc" style="font-size: 25px;">Leaves</div>
                    </div>
                </a>
            </div>
            <% } %>
        </div>


        <div class="row">
            <% if (new int[] { UserRoles.SuperAdmin.GetHashCode(), UserRoles.OverallAdmin.GetHashCode(), UserRoles.HR.GetHashCode(), UserRoles.CreditAdmin.GetHashCode() }.Contains(role))
                { %>
            <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                <a class="dashboard-stat dashboard-stat-v2 purple" href="<%= CREDITS %>">
                    <div class="visual">
                        <i class="fa fa-dropbox"></i>
                    </div>
                    <div class="details">
                        <div class="number">
                            <span data-counter="counterup" data-value="<%Response.Write(creditCount); %>">0</span>
                        </div>
                        <div class="desc" style="font-size: 25px;">Credits</div>
                    </div>
                </a>
            </div>
            <% } %>

            <% if (new int[] { UserRoles.SuperAdmin.GetHashCode(), UserRoles.OverallAdmin.GetHashCode(), UserRoles.HR.GetHashCode() }.Contains(role))
                { %>
            <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                <a class="dashboard-stat dashboard-stat-v2 yellow-casablanca" href="<%= CLIENTS %>">
                    <div class="visual">
                        <i class="fa fa-sticky-note-o"></i>
                    </div>
                    <div class="details">
                        <div class="number">
                            <span data-counter="counterup" data-value="<%Response.Write(clientCount); %>">0</span>
                        </div>
                        <div class="desc" style="font-size: 23px;">Clients</div>
                    </div>
                </a>
            </div>
            <% } %>

            <% if (new int[] { UserRoles.SuperAdmin.GetHashCode(), UserRoles.OverallAdmin.GetHashCode(), UserRoles.HR.GetHashCode() }.Contains(role))
                { %>
            <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                <a class="dashboard-stat dashboard-stat-v2 red" href="<%= VEHICLES %>">
                    <div class="visual">
                        <i class="fa fa-sticky-note-o"></i>
                    </div>
                    <div class="details">
                        <div class="number">
                            <span data-counter="counterup" data-value="<%Response.Write(vehicleCount); %>">0</span>
                        </div>
                        <div class="desc" style="font-size: 23px;">Vehicles</div>
                    </div>
                </a>
            </div>
            <% } %>

            <% if (new int[] { UserRoles.SuperAdmin.GetHashCode(), UserRoles.OverallAdmin.GetHashCode(), UserRoles.HR.GetHashCode() }.Contains(role))
                { %>
            <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                <a class="dashboard-stat dashboard-stat-v2 blue-hoki" href="<%= CALENDAR %>">
                    <div class="visual">
                        <i class="fa fa-sticky-note-o"></i>
                    </div>
                    <div class="details">
                        <div class="number">
                            <span>
                                <br />
                            </span>
                        </div>
                        <div class="desc" style="font-size: 23px;">Calendar</div>
                    </div>
                </a>
            </div>
            <% } %>
        </div>
    </div>
    <!-- END CONTENT BODY -->
</asp:Content>

