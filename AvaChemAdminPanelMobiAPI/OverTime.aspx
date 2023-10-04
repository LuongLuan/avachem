<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="Overtime.aspx.cs" Inherits="AvaChemAdminPanelMobiAPI.Overtime" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>AvaChem | OT</title>
    <script src="assets/global/scripts/alertJquery.js"></script>
    <style>
        .loader {
            border: 4px solid #f3f3f3; /* Light grey */
            border-top: 4px solid #3498db; /* Blue */
            border-radius: 50% !important;
            width: 20px;
            height: 20px;
            animation: spin 2s linear infinite;
        }

        @keyframes spin {
            0% {
                transform: rotate(0deg);
            }

            100% {
                transform: rotate(360deg);
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- BEGIN CONTENT BODY -->
    <div class="page-content">
        <!-- BEGIN PAGE HEADER-->
        <h1 class="page-title">OT
                    <small>create, update or delete OT</small>
        </h1>
        <div class="page-bar">
            <ul class="page-breadcrumb">
                <li>
                    <i class="icon-home"></i>
                    <a href="<%= DASHBOARD %>">Home</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <span>OT</span>
                </li>
            </ul>


            <% if (new int[] { UserRoles.SuperAdmin.GetHashCode(), UserRoles.OverallAdmin.GetHashCode() }.Contains(role))
                { %>
            <div class="page-toolbar">

                <div class="btn-group pull-right">

                    <button id="btnActions" type="button" class="btn btn-fit-height grey-salt dropdown-toggle" data-toggle="dropdown" data-hover="dropdown" data-delay="1000" data-close-others="true">
                        Actions <i class="fa fa-angle-down"></i>
                        <div class="loader"></div>
                    </button>
                    <ul class="dropdown-menu pull-right" role="menu">
                        <%--<li>
                            <a href="<%= CREATE_OT %>"><i class="icon-plus"></i>
                                Apply OT</a>
                        </li>--%>
                        <li>
                            <a href="#" id="btnExportCSV" onclick="ExportAsCSV_Click()"><i class="fa fa-download"></i>&nbsp;Export as CSV</a>
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
                    <span>All OT</span>
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
                <ul class="nav nav-tabs">
                    <li id="a-j-type-all" role="presentation"><a href="<%= OVERTIME %>">ALL</a></li>
                    <li id="a-j-type-<%=OTProgressTypes.Pending.GetHashCode()%>" role="presentation"><a href="<%= OVERTIME + "?type=" + OTProgressTypes.Pending.GetHashCode() %>">PENDING</a></li>
                    <li id="a-j-type-<%=OTProgressTypes.Approved.GetHashCode()%>" role="presentation"><a href="<%= OVERTIME + "?type=" + OTProgressTypes.Approved.GetHashCode() %>">APPROVED</a></li>
                    <li id="a-j-type-<%=OTProgressTypes.Rejected.GetHashCode()%>" role="presentation"><a href="<%= OVERTIME + "?type=" + OTProgressTypes.Rejected.GetHashCode() %>">REJECTED</a></li>
                </ul>
                <div class="table-scrollable">
                    <table class="table table-striped table-bordered table-hover">
                        <%if (list.Count == 0)
                            { %>
                        <% Response.Write("<div class='alert alert-info'><p>There are currently no OT<p></div>"); %>
                        <% }
                            if (list.Count > 0)
                            { %>
                        <thead>
                            <tr>
                                <th class="text-center">No.</th>
                                <th>Service Memo / Delivery Order Number</th>
                                <th>Status</th>
                                <th>Driver's Start Time</th>
                                <th>Driver's End Time</th>
                                <th>Client's Job Start Time</th>
                                <th>Client's Job End Time</th>

                                <% if (new int[] { UserRoles.SuperAdmin.GetHashCode(), UserRoles.OverallAdmin.GetHashCode() }.Contains(role))
                                    { %>
                                <th class="text-center" id="th_action_Update" runat="server">Update</th>
                                <th class="text-center" id="th_action_Delete" runat="server">Delete</th>
                                <% } %>
                            </tr>
                        </thead>
                        <tbody>
                            <%foreach (OT o in list)
                                {
                                    i++; %>
                            <tr>
                                <td class="text-center"><% Response.Write(i); %></td>
                                <%--<td><% Response.Write(o.JobName + (o.JobNumber + "" != "" ? $" (#{o.JobNumber})" : "")); %></td>--%>
                                <td>#<% Response.Write(o.JobNumber); %></td>
                                <td><% Response.Write(((Statuses)o.StatusID).ToString()); %></td>
                                <td><% Response.Write(o.DriverStartedTime.ToShortTimeString()); %></td>
                                <td><% Response.Write(o.DriverEndedTime.ToShortTimeString()); %></td>
                                <td><% Response.Write(o.WorkerStartedTime.ToShortTimeString()); %></td>
                                <td><% Response.Write(o.WorkerEndedTime.ToShortTimeString()); %></td>

                                <% if (new int[] { UserRoles.SuperAdmin.GetHashCode(), UserRoles.OverallAdmin.GetHashCode() }.Contains(role))
                                    { %>
                                <td class="text-center role"><a class="btn blue" href="<%= UPDATE_OT + "?id=" + o.ID %>"><i class="fa fa-upload"></i></a></td>
                                <td class="text-center role"><a class="btn red" id='"<% Response.Write(o.ID); %>"' data-toggle='modal' onclick='DeleteId_Click("<% Response.Write(o.ID); %>")' data-target='#deleteModal'><i class="fa fa-trash"></i></a></td>
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

        <% if (new int[] { UserRoles.SuperAdmin.GetHashCode(), UserRoles.OverallAdmin.GetHashCode() }.Contains(role))
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
                        <p>Are you sure you want to delete this OT?</p>
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
        function DeleteId_Click(val) {
            $('#deleteInput').val(val);
        }

        // Document ready
        $(function () {
            var type = getUrlVars()['type'] || 'all';
            $('#a-j-type-' + type).addClass('active');

            $("#btnActions").html('Actions <i class="fa fa-angle-down"></i>');
        });


        function ExportAsCSV_Click() {
            var sort = getUrlVars()['sort'] || 1;
            try {
                if (!isNaN(sort)) sort = parseInt(sort, 10)
            } catch (ex) {
            }

            console.log('sort', sort)

            ajaxRequest({
                method: 'POST',
                url: 'AjaxHandler.asmx/ExportOTasCSV',
                body: {
                    sort: sort, // revert CSV to match the table
                    //page: 0000,
                    //per_page: 0000,
                    //search: 0000,
                    //type: 0000,
                    //month: 0000,
                    //year: 0000,
                    //sort: 0000,
                },
                beforeSend: function () {
                    $("#btnActions").html('<div class="loader"></div>');
                },
                success: function (response) {
                    // let { data, status, jqXHR } = response
                    var csvContent = 'data:text/csv;charset=utf-8,' + (response.data || '').replace(/#/g, '').replace(/"/g, '""');
                    console.log('csv', csvContent);

                    var encodedUri = encodeURI(csvContent);
                    var link = document.createElement("a");
                    link.setAttribute("href", encodedUri);
                    link.setAttribute("download", "OT_" + Date.now() + ".csv");
                    document.body.appendChild(link); // Required for FF

                    link.click(); // This will download the data file named "OT_....csv".
                },
                error: function (err) {
                    // let { jqXHR, status, error } = err
                    // console.log('Error', response.jqXHR, response.status, response.error)
                },
                complete: function () {
                    $("#btnActions").html('Actions <i class="fa fa-angle-down"></i>');
                }
            })
        }
    </script>
</asp:Content>
