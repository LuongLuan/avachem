<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="JobDetails.aspx.cs" Inherits="AvaChemAdminPanelMobiAPI.JobDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>AvaChem | Job Details</title>
    <script src="assets/global/scripts/alertJquery.js"></script>
    <style>
        .w-auto {
            height: 35px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- BEGIN CONTENT BODY -->
    <div class="page-content">
        <!-- BEGIN PAGE HEADER-->
        <h1 class="page-title">Job Details
                        <small>job details in the system</small>
        </h1>
        <div class="page-bar">
            <ul class="page-breadcrumb">
                <li>
                    <i class="icon-home"></i>
                    <a href="<%= DASHBOARD %>">Home</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <a href="<%= JOBS %>">Jobs</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <span>Job Details</span>
                </li>
            </ul>
        </div>
        <!-- END PAGE HEADER-->
        <!-- START OF CREATE JOB PORTLET -->
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
                            <span class="caption-subject bold uppercase">Job Details</span>
                        </div>
                    </div>
                    <div class="portlet-body form" runat="server">
                        <div class="form-horizontal">
                            <div class="form-body">
                                <% if (isCompleted
                                            && new int[] { UserRoles.SuperAdmin.GetHashCode(), UserRoles.OverallAdmin.GetHashCode() }.Contains(role))
                                    { %>
                                <div class="form-group" style="margin-bottom: 35px">
                                    <label class="col-md-3 control-label">Report<label style="color: red">*</label>:</label>
                                    <div class="col-md-5">
                                        <div style="display: flex;">
                                            <asp:TextBox placeholder="" ID="tbxReportName" ClientIDMode="Static" runat="server" CssClass="form-control w-auto"></asp:TextBox>
                                            <a class="btn" id="btnGeneratePDF" onclick="PrintReport_Click()" style="height: 35px">
                                                <i class="fa fa-print" aria-hidden="true"></i>
                                                Print
                                            </a>
                                        </div>
                                    </div>
                                </div>
                                <% } %>
                                <div class="form-group">
                                    <label class="col-md-3 control-label">
                                        Client<label style="color: red">*</label>:</label>
                                    <div class="col-md-5">
                                        <div id="tbxClient" clientidmode="Static" runat="server" style="border: 1px solid #c2cad8; padding: 12px; background: #eef1f5">abc</div>
                                        <br />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-3 control-label">
                                        Location<label style="color: red">*</label>:</label>
                                    <div class="col-md-5">
                                        <asp:TextBox ReadOnly="true" class="form-control" placeholder="" ID="tbxLocation" runat="server"></asp:TextBox>
                                        <br />
                                    </div>
                                </div>
                                <%--<div class="form-group">
                                    <label class="col-md-3 control-label">
                                        Service Memo / Delivery Order Number<label style="color: red">*</label>:</label>
                                    <div class="col-md-5">
                                        <asp:TextBox ReadOnly="true" class="form-control" placeholder="" ID="tbxJobNumber" runat="server"></asp:TextBox>
                                        <br />
                                    </div>
                                </div>--%>
                                <div class="form-group">
                                    <label class="col-md-3 control-label">
                                        Invoice No<label style="color: red">*</label>.</label>
                                    <div class="col-md-5">
                                        <asp:TextBox ReadOnly="true" class="form-control" placeholder="" ID="tbxInvoiceNo" runat="server"></asp:TextBox>
                                        <br />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-3 control-label">
                                        Name<label style="color: red">*</label>:</label>
                                    <div class="col-md-5">
                                        <asp:TextBox ReadOnly="true" class="form-control" placeholder="" ID="tbxName" runat="server"></asp:TextBox>
                                        <br />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-3 control-label">Date<label style="color: red">*</label>:</label>
                                    <div class="col-md-5">
                                        <input readonly="readonly" class="form-control" id="dtWorkingDate" type="date" value="" runat="server" />
                                        <br />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-3 control-label">
                                        Description:</label>
                                    <div class="col-md-5">
                                        <div readonly="readonly" class="form-control" id="tbxDescription" runat="server" style="min-height: 34px; height: auto"></div>
                                        <br />
                                    </div>
                                </div>
                                <%-- <div class="form-group">
                                    <label class="col-md-3 control-label">Description:</label>
                                    <input id="JobDescriptionValues" clientidmode="Static" type="hidden" runat="server" />
                                    <div class="col-md-5" style="margin-left: 20px;">
                                        <%foreach (Description d in listD)
                                            { %>
                                        <div class="checkbox">
                                            <label>
                                                <input disabled type="checkbox" id="d_<%Response.Write(d.ID); %>" onclick="Description_Click(this, '<%Response.Write(d.ID); %>')">
                                                <%Response.Write(d.Content); %>
                                            </label>
                                        </div>
                                        <% } %>

                                        <br />
                                    </div>
                                </div>--%>

                                <div class="form-group">
                                    <label class="col-md-3 control-label">Remarks:</label>
                                    <div class="col-md-5">
                                        <textarea readonly="readonly" rows="3" cols="20" class="form-control" placeholder="" id="txtAdminRemarks" runat="server"></textarea>
                                        <br />
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="col-md-3 control-label">Vehicles:</label>
                                    <div class="col-md-5">
                                        <div class="table-responsive">
                                            <table class="table table-striped table-bordered table-hover" style="font-size: 13px">
                                                <thead>
                                                    <tr>
                                                        <th>Vehicle Number</th>
                                                        <th>Vehicle Model</th>
                                                    </tr>
                                                </thead>
                                                <tbody id="tbdVehicle">
                                                    <%foreach (Vehicle v in listV)
                                                        { %>
                                                    <tr id="vehicle_<%Response.Write(v.ID); %>">
                                                        <td id="vehicle_<%Response.Write(v.ID); %>_Number"><%Response.Write(v.Number); %></td>
                                                        <td id="vehicle_<%Response.Write(v.ID); %>_Model"><%Response.Write(v.Model); %></td>
                                                    </tr>
                                                    <% } %>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="col-md-3 control-label">Staffs:</label>
                                    <div class="col-md-6">
                                        <div class="table-responsive">
                                            <table class="table table-striped table-bordered table-hover" style="font-size: 13px">
                                                <thead>
                                                    <tr>
                                                        <th>Name</th>
                                                        <th>ID Number</th>
                                                        <th class="text-center">Phone</th>
                                                        <th>Email</th>
                                                        <th class="text-center">Role</th>
                                                    </tr>
                                                </thead>
                                                <tbody id="tbdWorker">
                                                    <%foreach (User w in listWorkers)
                                                        { %>
                                                    <tr id="w_<%Response.Write(w.ID); %>">
                                                        <td id="w_<%Response.Write(w.ID); %>_Name"><%Response.Write(w.Name); %></td>
                                                        <td id="w_<%Response.Write(w.ID); %>_IDNumber"><%Response.Write(w.IDNumber); %></td>
                                                        <td id="w_<%Response.Write(w.ID); %>_Phone" class="text-center"><%Response.Write(w.Phone); %></td>
                                                        <td id="w_<%Response.Write(w.ID); %>_Email"><%Response.Write(w.Email); %></td>
                                                        <td id="w_<%Response.Write(w.ID); %>_RoleID" class="text-center"><% Response.Write(((UserRoles)w.RoleID).ToString()); %></td>
                                                    </tr>
                                                    <% } %>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>

                                <%--<div class="form-group" style="display: flex; align-items: center;">
                                    <label class="col-xs-1 col-md-3 control-label" style="padding-top: 0; margin-top: 0;">Trips:</label>
                                </div>--%>

                                <div id="trips">
                                    <%foreach (TripDetailsDTO trip in listT)
                                        { %>
                                    <div id="t_<%Response.Write(trip.ID); %>" style="width: 100%; display: flex; justify-content: center; margin-bottom: 25px">
                                        <div class="col-xs-12 col-md-10" style="border: 1px solid #c2cad8; height: 100%">

                                            <div class="col-md-offset-2 caption font-dark" style="margin-top: 2.5rem; margin-bottom: 2.5rem; font-size: 1.8rem; display: flex; align-items: center; justify-content: space-between">
                                                <div>
                                                    <i class="fa fa-paper-plane" aria-hidden="true"></i>
                                                    <span class="caption-subject bold uppercase">Trip <% Response.Write(trip.Index); %></span>
                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <label class="col-md-3 control-label">Job's Period Hours<label style="color: red">*</label>:</label>
                                                <div class="col-md-5" style="display: flex; flex-wrap: wrap">
                                                    <div>
                                                        <input readonly="readonly" type="time" id="t_<%Response.Write(trip.ID); %>_dtStart" value="<%Response.Write(trip.StartTime.ToString("HH:mm")); %>" onchange="JobTime_Change(this, <%Response.Write(trip.ID); %>, 'StartTime')" />
                                                        <br />
                                                        <br />
                                                    </div>
                                                    <label style="padding-top: 7px;">&emsp;&emsp;to&emsp;&emsp;</label>
                                                    <div>
                                                        <input readonly="readonly" type="time" id="t_<%Response.Write(trip.ID); %>_dtEnd" value="<%Response.Write(trip.EndTime.ToString("HH:mm")); %>" onchange="JobTime_Change(this, <%Response.Write(trip.ID); %>, 'EndTime')" />
                                                        <br />
                                                        <br />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-3 control-label">Logged Period Hours:</label>
                                                <div class="col-md-6" style="display: flex;">
                                                    <div>
                                                        <input readonly="readonly" type="time" id="t_<%Response.Write(trip.ID); %>_dtWStart" value="<%Response.Write(trip.WorkerStartedTime.HasValue ? ((DateTime)trip.WorkerStartedTime).ToString("HH:mm") : ""); %>" onchange="JobTime_Change(this, <%Response.Write(trip.ID); %>, 'WorkerStartedTime')" />
                                                        <br />
                                                        <br />
                                                    </div>
                                                    <label style="padding-top: 7px;">&emsp;&emsp;to&emsp;&emsp;</label>
                                                    <div>
                                                        <input readonly="readonly" type="time" id="t_<%Response.Write(trip.ID); %>_dtWEnd" value="<%Response.Write(trip.WorkerEndedTime.HasValue ? ((DateTime)trip.WorkerEndedTime).ToString("HH:mm") : ""); %>" onchange="JobTime_Change(this, <%Response.Write(trip.ID); %>, 'WorkerEndedTime')" />
                                                        <br />
                                                        <br />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-3 control-label">Service Memo / Delivery Order Number:</label>
                                                <div class="col-md-6">
                                                    <input class="form-control" readonly="readonly" id="t_<%Response.Write(trip.ID); %>_jobNumber" value="<%Response.Write(trip.JobNumber); %>" onchange="JobNumber_Change(this, <%Response.Write(trip.ID); %>)" />
                                                    <br />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-3 control-label">Driver's Remarks:</label>
                                                <div class="col-md-6">
                                                    <textarea readonly="readonly" rows="3" cols="20" class="form-control" placeholder="Remarks" id="txtRemarks" onchange="Remarks_Change(this, <%Response.Write(trip.ID); %>)"> <%Response.Write(trip.Remarks); %></textarea>
                                                    <br />
                                                </div>
                                            </div>
                                            <div class="form-group" style="margin-bottom: 3rem">
                                                <label class="col-md-3 control-label">Before Images:</label>
                                                <div class="col-md-6">
                                                    <div id="tbdBeforeImage" style="display: flex; flex-wrap: wrap;">
                                                        <%foreach (JobImage img in trip.BeforeImages)
                                                            { %>
                                                        <div id="beimg_<%Response.Write(img.ID); %>" style="position: relative; margin: 0 12px 1.5rem 0; border: 1px dashed #808080;">
                                                            <a href="<%Response.Write(img.ImageUrl); %>" target="_blank">
                                                                <div style="width: 100px; height: 100px; line-height: 100px;">
                                                                    <img src="<%Response.Write(img.ImageUrl); %>" style="width: 100%; max-height: 100%" />
                                                                </div>
                                                            </a>
                                                            <%--<button type="button" class="btn btn-danger btn-sm" style="position: absolute; z-index: 1; top: -1rem; right: -6px;" onclick="DeleteJobImg_Click(<%Response.Write(trip.ID); %>,<%Response.Write(img.ID); %>,'before')">
                                                                <i class="fa fa-times" aria-hidden="true"></i>
                                                            </button>--%>
                                                        </div>
                                                        <% } %>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group" style="margin-bottom: 4rem">
                                                <label class="col-md-3 control-label">After Images:</label>
                                                <div class="col-md-6">
                                                    <div id="tbdAfterImage" style="display: flex; flex-wrap: wrap;">
                                                        <%foreach (JobImage img in trip.AfterImages)
                                                            { %>
                                                        <div id="afimg_<%Response.Write(img.ID); %>" style="position: relative; margin: 0 12px 12px 0; border: 1px dashed #808080;">
                                                            <a href="<%Response.Write(img.ImageUrl); %>" target="_blank">
                                                                <div style="width: 100px; height: 100px; line-height: 100px;">
                                                                    <img src="<%Response.Write(img.ImageUrl); %>" style="width: 100%; max-height: 100%" />
                                                                </div>
                                                            </a>
                                                            <%--<button type="button" class="btn btn-danger btn-sm" style="position: absolute; z-index: 1; top: -1.4rem; right: -6px;" onclick="DeleteJobImg_Click(<%Response.Write(trip.ID); %>,<%Response.Write(img.ID); %>,'after')">
                                                                <i class="fa fa-times" aria-hidden="true"></i>
                                                            </button>--%>
                                                        </div>
                                                        <% } %>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group grpSignatureImage">
                                                <label class="col-md-3 control-label">Customer Signature:</label>
                                                <div class="col-md-6">
                                                    <div id="signatureImage" style="border: 1px dashed #808080; min-height: 34px;">
                                                        <a href="<%Response.Write(trip.CustomerSignatureImage); %>" target="_blank">
                                                            <img style="width: 100%" src="<%Response.Write(trip.CustomerSignatureImage); %>" />
                                                        </a>
                                                    </div>
                                                    <br />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <% } %>
                                </div>


                                <br />

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
        <!-- END OF CREATE JOB PORTLET -->
    </div>
    <!-- END CONTENT BODY -->

    <script src="assets/pages/plugins/sweetalert2/sweetalert2.all.min.js"></script>
    <script>
        var btnGeneratePDF_id = '#btnGeneratePDF';

        // Document ready
        $(function () {

            $(btnGeneratePDF_id).css({
                'color': '#fff',
                'background-color': '#4cb64c',
                'border-color': '#409e40',
            }).mouseleave(function () {
                $(this).css({
                    'background-color': '#4cb64c',
                    'border-color': '#409e40',
                });
            }).mouseenter(function () {
                $(this).css({
                    'background-color': '#3c933c',
                    'border-color': '#2f722f',
                });
            });

            //try {
            //    var jobDescriptions = JSON.parse($('#JobDescriptionValues').val() || '[]');
            //    for (var i = 0; i < jobDescriptions.length; i++) {
            //        var jobDescription = jobDescriptions[i];
            //        $('#d_' + jobDescription.ID).attr('checked', true);
            //    }
            //} catch (_e) {
            //    console.log(_e)
            //}
        })

        function PrintReport_Click() {
            var reportName = $('#tbxReportName').val();
            if (!reportName) return;
            ajaxRequest({
                method: 'POST',
                url: 'AjaxHandler.asmx/UpdateReportName',
                body: {
                    jobID: <%= thisJob.ID %>,
                    reportName,

                    ActionLocation: '~/JobDetails.aspx',
                    ByUserID: <%= userSession.ID %>,
                    RoleID: <%= userSession.RoleID %>,
                    Name: '<%= userSession.Name %>'
                },
                beforeSend: function () {
                    $(btnGeneratePDF_id).attr('disabled', true);
                    $(btnGeneratePDF_id).css({
                        'pointer-events': 'none',
                    });
                },
                success: function (response) {
                    // let { data, status, jqXHR } = response
                    $('#tbxReportName').attr('readonly', true);
                    var newReportName = (response.data || {}).reportName || '';

                    var win = window.open('<%= JOB_REPORT %>/' + newReportName, '_blank');
                    if (win) {
                        //Browser has allowed it to be opened
                        win.focus();
                    } else {
                        Swal.fire({
                            icon: 'error',
                            // title: 'Oops...',
                            text: 'Please allow popups for this website.'
                        })
                    }
                },
                error: function (err) {
                    // let { jqXHR, status, error } = err
                    // console.log('Error', response.jqXHR, response.status, response.error)
                },
                complete: function () {
                    $(btnGeneratePDF_id).attr('disabled', false);
                    $(btnGeneratePDF_id).css({
                        'pointer-events': 'auto',
                    });
                }
            })
        }
    </script>
</asp:Content>
