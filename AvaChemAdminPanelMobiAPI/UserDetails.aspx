<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="UserDetails.aspx.cs" Inherits="AvaChemAdminPanelMobiAPI.UserDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>AvaChem | User Details</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- BEGIN CONTENT BODY -->
    <div class="page-content">
        <!-- BEGIN PAGE HEADER-->
        <h1 class="page-title">User Details
                        <small>user details in the system</small>
        </h1>
        <div class="page-bar">
            <ul class="page-breadcrumb">
                <li>
                    <i class="icon-home"></i>
                    <a href="<%= DASHBOARD %>">Home</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <a href="<%= USERS %>">User</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <span>User Details</span>
                </li>
            </ul>
        </div>
        <!-- END PAGE HEADER-->
        <!-- START OF Update USER PORTLET -->
        <div class="row">
            <div class="col-md-12">
                <div class="portlet light">
                    <div class="portlet-title">
                        <div class="caption font-dark">
                            <i class="icon-folder-alt font-dark"></i>
                            <span class="caption-subject bold uppercase">User Details</span>
                        </div>
                    </div>
                    <div class="portlet-body form" runat="server">
                        <div class="form-horizontal">
                            <div class="form-body">
                                <div class="form-group">
                                    <label class="col-md-3 control-label">Role:</label>
                                    <div class="col-md-5">
                                        <asp:TextBox class="form-control" ReadOnly="true" ID="tbxRole" runat="server"></asp:TextBox><br />
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="col-md-3 control-label">Username:</label>
                                    <div class="col-md-5">
                                        <asp:TextBox class="form-control" ReadOnly="true" ID="tbxUsername" runat="server"></asp:TextBox><br />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-3 control-label">ID Number:</label>
                                    <div class="col-md-5">
                                        <asp:TextBox class="form-control" ReadOnly="true" ID="tbxIDNumber" runat="server"></asp:TextBox><br />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-3 control-label">Name:</label>
                                    <div class="col-md-5">
                                        <asp:TextBox class="form-control" ReadOnly="true" ID="tbxName" runat="server"></asp:TextBox><br />
                                    </div>
                                </div>
                                <%--<div class="form-group">
                                    <label class="col-md-3 control-label">Date of Birth:</label>
                                    <div class="col-md-5">
                                        <asp:TextBox class="form-control" ID="tbxDob" ReadOnly="true" runat="server"></asp:TextBox><br />
                                    </div>
                                </div>--%>
                                <div class="form-group">
                                    <label class="col-md-3 control-label">Phone Number:</label>
                                    <div class="col-md-5">
                                        <asp:TextBox class="form-control" ReadOnly="true" ID="tbxPhone" runat="server"></asp:TextBox><br />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-3 control-label">Email:</label>
                                    <div class="col-md-5">
                                        <asp:TextBox class="form-control" ReadOnly="true" ID="tbxEmail" runat="server"></asp:TextBox><br />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-3 control-label">Total Credits:</label>
                                    <div class="col-md-5">
                                        <asp:TextBox class="form-control" ReadOnly="true" ID="tbxCredits" runat="server"></asp:TextBox><br />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-3 control-label">Leave Days Left:</label>
                                    <div class="col-md-5">
                                        <asp:TextBox class="form-control" ReadOnly="true" ID="tbxLeaveDaysLeft" runat="server"></asp:TextBox><br />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-3 control-label">MC Days Left:</label>
                                    <div class="col-md-5">
                                        <asp:TextBox class="form-control" ReadOnly="true" ID="tbxMCDaysLeft" runat="server"></asp:TextBox><br />
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="col-md-3 control-label">Qualifications:</label>
                                    <div class="col-md-5">
                                        <div class="table-responsive">
                                            <table class="table table-striped table-bordered table-hover" style="font-size: 13px">
                                                <thead>
                                                    <tr>
                                                        <th>Name</th>
                                                        <th>Date Obtained</th>
                                                        <th>Expiry Date</th>
                                                    </tr>
                                                </thead>
                                                <tbody id="tbdQ">
                                                    <%foreach (Qualification q in listQ)
                                                        { %>
                                                    <tr>
                                                        <td><% Response.Write(q.Name); %></td>
                                                        <td><% Response.Write(q.DateObtained.ToString("dd-MM-yyyy")); %></td>
                                                        <td><% Response.Write(q.ExpiryDate.ToString("dd-MM-yyyy")); %></td>
                                                    </tr>
                                                    <% } %>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>

                                <%--<div class="form-group">
                                    <label class="col-md-3 control-label">Jobs:</label>
                                    <div class="col-md-5">
                                        <div class="table-responsive">
                                            <table class="table table-striped table-bordered table-hover" style="font-size: 13px">
                                                <thead>
                                                    <tr>
                                                        <th>Name</th>
                                                        <th>Location</th>
                                                    </tr>
                                                </thead>
                                                <tbody id="JobID">
                                                    <%foreach (Job j in listJ)
                                                        { %>
                                                    <tr>
                                                        <td><% Response.Write(j.Name); %></td>
                                                        <td><% Response.Write(j.Location); %></td>
                                                    </tr>
                                                    <% } %>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>--%>

                                <div class="form-group">
                                    <div class="col-md-offset-3 col-md-9">
                                        <asp:Button CausesValidation="False" CssClass="btn green uppercase" ID="btnCancel" runat="server" Text="Back" OnClick="btnCancel_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- END OF Update USER PORTLET -->
    </div>

</asp:Content>
