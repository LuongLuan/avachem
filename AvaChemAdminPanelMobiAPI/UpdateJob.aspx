<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" EnableEventValidation="false" Async="true" AutoEventWireup="true" CodeBehind="UpdateJob.aspx.cs" Inherits="AvaChemAdminPanelMobiAPI.UpdateJob" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>AvaChem | Update Job</title>
    <script src="assets/global/scripts/alertJquery.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- BEGIN CONTENT BODY -->
    <div class="page-content">
        <!-- BEGIN PAGE HEADER-->
        <h1 class="page-title">Update Job
                        <small>update job in the system</small>
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
                    <span>Update Job</span>
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
                            <span class="caption-subject bold uppercase">Update Job</span>
                        </div>
                    </div>
                    <div class="portlet-body form" runat="server">
                        <div class="form-horizontal">
                            <div class="form-body">
                                <div class="form-group">
                                    <label class="col-md-3 control-label">Client<label style="color: red">*</label>:</label>
                                    <div class="col-md-5">
                                        <asp:DropDownList class="form-control" ID="ddlClient" ClientIDMode="Static" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator
                                            runat="server"
                                            Display="Dynamic"
                                            CssClass="txt-error"
                                            ControlToValidate="ddlClient"
                                            ErrorMessage="Please fill in the blank!"></asp:RequiredFieldValidator>
                                        <br />
                                        <br />
                                    </div>
                                </div>
                                <div class="form-group grpLocation">
                                    <label class="col-md-3 control-label">
                                        Location:</label>
                                    <div class="col-md-5">
                                        <asp:TextBox ReadOnly="true" class="form-control" placeholder="" ID="tbxLocation" runat="server" ClientIDMode="Static"></asp:TextBox>
                                        <br />
                                    </div>
                                </div>
                                <%--<div class="form-group">
                                    <label class="col-md-3 control-label">
                                        Service Memo / Delivery Order Number:</label>
                                    <div class="col-md-5">
                                        <asp:TextBox class="form-control" placeholder="Enter Service Memo / Delivery Order Number" ID="tbxJobNumber" runat="server"></asp:TextBox>
                                        <br />
                                    </div>
                                </div>--%>
                                <div class="form-group">
                                    <label class="col-md-3 control-label">
                                        Invoice No.</label>
                                    <div class="col-md-5">
                                        <asp:TextBox class="form-control" placeholder="Enter invoice number" ID="tbxInvoiceNo" runat="server"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator
                                            runat="server"
                                            Display="Dynamic"
                                            CssClass="txt-error"
                                            ControlToValidate="tbxInvoiceNo"
                                            ErrorMessage="Please fill in the blank!"></asp:RequiredFieldValidator>--%>
                                        <br />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-3 control-label">
                                        Name<label style="color: red">*</label>:</label>
                                    <div class="col-md-5">
                                        <asp:TextBox class="form-control" placeholder="Enter job name" ID="tbxName" runat="server"></asp:TextBox>
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
                                    <label class="col-md-3 control-label">Date<label style="color: red">*</label>:</label>
                                    <div class="col-md-5">
                                        <input id="dtWorkingDate" clientidmode="Static" type="date" value="" runat="server" />
                                        <asp:RequiredFieldValidator
                                            runat="server"
                                            Display="Dynamic"
                                            CssClass="txt-error"
                                            ControlToValidate="dtWorkingDate"
                                            ErrorMessage="Please fill in the blank!"></asp:RequiredFieldValidator>
                                        <br />
                                        <br />
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="col-md-3 control-label">Description:</label>
                                    <input id="JobDescriptionValues" clientidmode="Static" type="hidden" runat="server" />
                                    <div class="col-md-5" style="margin-left: 20px;">
                                        <%foreach (Description d in listD)
                                            { %>
                                        <div class="checkbox">
                                            <label>
                                                <input type="checkbox" id="d_<%Response.Write(d.ID); %>" onclick="Description_Click(this, '<%Response.Write(d.ID); %>')">
                                                <%Response.Write(d.Content); %>
                                            </label>
                                        </div>
                                        <% } %>

                                        <br />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-3 control-label">Remarks:</label>
                                    <div class="col-md-5">
                                        <textarea rows="3" cols="20" class="form-control" placeholder="Remarks" id="txtAdminRemarks" runat="server"></textarea>
                                        <br />
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="col-md-3 control-label">Vehicles:</label>
                                    <input id="VehicleValues" clientidmode="Static" type="hidden" runat="server" />
                                    <div class="col-md-5" style="display: flex">
                                        <div class="col-xs-12" style="padding-left: 0;">
                                            <asp:DropDownList class="form-control" ID="ddlVehicle" ClientIDMode="Static" runat="server"></asp:DropDownList>
                                        </div>
                                        <div class="text-right" style="padding: 0;">
                                            <button type="button" class="btn btn-primary" onclick="AddVehicle_Click()"><i class="fa fa-plus" aria-hidden="true"></i></button>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-3 control-label"></label>
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
                                                        <td class="text-center">
                                                            <button type="button" class="btn btn-danger btn-sm" onclick="DeleteVehicle_Click('<%Response.Write(v.ID); %>')"><i class='fa fa-times' aria-hidden='true'></i></button>
                                                        </td>
                                                    </tr>
                                                    <% } %>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="col-md-3 control-label">Staffs:</label>
                                    <input id="WorkerValues" clientidmode="Static" type="hidden" runat="server" />
                                    <div class="col-md-5" style="display: flex">
                                        <div class="col-xs-12" style="padding-left: 0;">
                                            <asp:DropDownList class="form-control" ID="ddlWorker" ClientIDMode="Static" runat="server"></asp:DropDownList>
                                        </div>
                                        <div class="text-right" style="padding: 0;">
                                            <button type="button" class="btn btn-primary" onclick="AddWorker_Click()"><i class="fa fa-plus" aria-hidden="true"></i></button>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-3 control-label"></label>
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
                                                        <th class="text-center">
                                                            <button type="button" class="btn btn-danger btn-sm" style="pointer-events: none"><i class="fa fa-times" aria-hidden="true"></i></button>
                                                        </th>
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
                                                        <td class="text-center">
                                                            <button type="button" class="btn btn-danger btn-sm"
                                                                onclick="DeleteWorker_Click('<%Response.Write(w.ID); %>')">
                                                                <i class="fa fa-times" aria-hidden="true"></i>
                                                            </button>
                                                        </td>
                                                    </tr>
                                                    <% } %>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>


                                <div class="form-group" style="display: flex; align-items: center;">
                                    <label class="col-xs-1 col-md-3 control-label" style="padding-top: 0; margin-top: 0;">Trips:</label>
                                    <input id="TripValues" clientidmode="Static" type="hidden" runat="server" />
                                    <div style="display: flex; margin-left: 15px;">
                                        <div class="text-right" style="padding: 0;">
                                            <button type="button" class="btn btn-primary" onclick="AddTrip_Click()"><i class="fa fa-plus" aria-hidden="true"></i></button>
                                        </div>
                                    </div>
                                </div>

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

                                                <div>
                                                    <%--<a class="btn blue" href="<%= UPDATE_TRIP + "?id=" + trip.ID %>"><i class="fa fa-pencil"></i></a>--%>
                                                    <button type="button" class="btn red" onclick="DeleteTrip_Click(<%Response.Write(trip.ID); %>)"><i class="fa fa-times"></i></button>
                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <label class="col-md-3 control-label">Job's Period Hours<label style="color: red">*</label>:</label>
                                                <div class="col-md-5" style="display: flex; flex-wrap: wrap">
                                                    <div>
                                                        <input type="time" id="t_<%Response.Write(trip.ID); %>_dtStart" value="<%Response.Write(trip.StartTime.ToString("HH:mm")); %>" onchange="JobTime_Change(this, <%Response.Write(trip.ID); %>, 'StartTime')" />
                                                        <br />
                                                        <br />
                                                    </div>
                                                    <label style="padding-top: 7px;">&emsp;&emsp;to&emsp;&emsp;</label>
                                                    <div>
                                                        <input type="time" id="t_<%Response.Write(trip.ID); %>_dtEnd" value="<%Response.Write(trip.EndTime.ToString("HH:mm")); %>" onchange="JobTime_Change(this, <%Response.Write(trip.ID); %>, 'EndTime')" />
                                                        <br />
                                                        <br />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-3 control-label">Logged Period Hours:</label>
                                                <div class="col-md-6" style="display: flex;">
                                                    <div>
                                                        <input type="time" id="t_<%Response.Write(trip.ID); %>_dtWStart" value="<%Response.Write(trip.WorkerStartedTime.HasValue ? ((DateTime)trip.WorkerStartedTime).ToString("HH:mm") : ""); %>" onchange="JobTime_Change(this, <%Response.Write(trip.ID); %>, 'WorkerStartedTime')" />
                                                        <br />
                                                        <br />
                                                    </div>
                                                    <label style="padding-top: 7px;">&emsp;&emsp;to&emsp;&emsp;</label>
                                                    <div>
                                                        <input type="time" id="t_<%Response.Write(trip.ID); %>_dtWEnd" value="<%Response.Write(trip.WorkerEndedTime.HasValue ? ((DateTime)trip.WorkerEndedTime).ToString("HH:mm") : ""); %>" onchange="JobTime_Change(this, <%Response.Write(trip.ID); %>, 'WorkerEndedTime')" />
                                                        <br />
                                                        <br />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-3 control-label">Service Memo / Delivery Order Number:</label>
                                                <div class="col-md-6">
                                                    <input class="form-control" id="t_<%Response.Write(trip.ID); %>_jobNumber" value="<%Response.Write(trip.JobNumber); %>" onchange="JobNumber_Change(this, <%Response.Write(trip.ID); %>)" />
                                                    <br />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-3 control-label">Driver's Remarks:</label>
                                                <div class="col-md-6">
                                                    <textarea rows="3" cols="20" class="form-control" placeholder="Remarks" id="txtRemarks" onchange="Remarks_Change(this, <%Response.Write(trip.ID); %>)"> <%Response.Write(trip.Remarks); %></textarea>
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
                                                            <button type="button" class="btn btn-danger btn-sm" style="position: absolute; z-index: 1; top: -1rem; right: -6px;" onclick="DeleteJobImg_Click(<%Response.Write(trip.ID); %>,<%Response.Write(img.ID); %>,'before')">
                                                                <i class="fa fa-times" aria-hidden="true"></i>
                                                            </button>
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
                                                            <button type="button" class="btn btn-danger btn-sm" style="position: absolute; z-index: 1; top: -1.4rem; right: -6px;" onclick="DeleteJobImg_Click(<%Response.Write(trip.ID); %>,<%Response.Write(img.ID); %>,'after')">
                                                                <i class="fa fa-times" aria-hidden="true"></i>
                                                            </button>
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
        <!-- END OF CREATE JOB PORTLET -->
    </div>
    <!-- END CONTENT BODY -->

    <script src="assets/pages/plugins/sweetalert2/sweetalert2.all.min.js"></script>
    <script>
        var enumActionTypes = <%= dataActionTypes %>; // ["Add",]
        var roles = <%= convertRoles %>; // [{"Key":1,"Value":"Admin"},...]

        var ddlClient_id = '#ddlClient';
        var ddlWorker_id = '#ddlWorker';
        var ddlVehicle_id = '#ddlVehicle';

        var dtWorkingDate_id = '#dtWorkingDate';

        var hidTripValues_id = '#TripValues';

        // Document ready
        $(function () {
            $(ddlClient_id).select2();
            $(ddlWorker_id).select2();
            $(ddlVehicle_id).select2();

            if (Array.isArray(roles)) {
                roles = roles.reduce(function (res, cur) {
                    res[cur.Key] = cur.Value;
                    return res;
                }, {})
            }

            try {
                if (!Array.isArray(enumActionTypes)) enumActionTypes = JSON.parse(enumActionTypes);
                if (enumActionTypes.indexOf("") == -1) enumActionTypes.unshift(""); // map enum value to array index
            } catch (_e) {
                return;
            }

            try {
                var jobDescriptions = JSON.parse($('#JobDescriptionValues').val() || '[]');
                for (var i = 0; i < jobDescriptions.length; i++) {
                    var jobDescription = jobDescriptions[i];
                    $('#d_' + jobDescription.ID).attr('checked', true);
                }
            } catch (ex) {
            }


            handleJobDateTimeChange();
            $(dtWorkingDate_id).on('change', handleJobDateTimeChange);

            //if (!$('#tbxLocation').val()) $('.grpLocation').hide();
            $(ddlClient_id).on('select2:select', function (e) {
                var selectedValue = e.params.data.id;
                var location = selectedValue.split('|')[1];
                $('#tbxLocation').val(location);
                if (location) $('.grpLocation').show();
                else $('.grpLocation').hide();
            });
        })

        function Description_Click(cb, id) {
            toggleJobDescription(cb, id, enumActionTypes);
        }


        function AddWorker_Click() {
            var workingDate = $(dtWorkingDate_id).val();
            var workerDTO = $(ddlWorker_id).val();
            try {
                workerDTO = JSON.parse(workerDTO);
            } catch (ex) {
                return;
            }

            if (!workingDate) {
                addWorker(workerDTO, roles, enumActionTypes);
                return;
            }

            ajaxRequest({
                method: 'POST',
                url: 'AjaxHandler.asmx/GetLeavesByDate',
                body: { uid: workerDTO.ID, workingDates: workingDate },
                success: function (response) {
                    // let { data, status, jqXHR } = response
                    var leavesRes = response.data;
                    if (leavesRes.length > 0) {
                        var hasApproved = false;
                        var hasPending = false;
                        for (var i = 0; i < leavesRes.length; i++) {
                            var l = leavesRes[i];
                            if (l.StatusID == '<%= Statuses.Approved.GetHashCode() %>') { // Approved
                                hasApproved = true;
                            } else if (l.StatusID == '<%= Statuses.Pending.GetHashCode() %>') { // Pending
                                hasPending = true;
                            }
                            if (hasApproved || hasPending) break;
                        }
                        if (hasApproved || hasPending) {
                            Swal.fire({
                                icon: 'error',
                                // title: 'Oops...',
                                text: hasApproved ? 'This ' + (roles[workerDTO.RoleID] || '').toLowerCase() + ' "' + workerDTO.Name + '" is on leave and cannot be assigned.'
                                    : hasPending ? 'This ' + (roles[workerDTO.RoleID] || '').toLowerCase() + ' "' + workerDTO.Name + '" has pending leave on this day, please reject the leave before assigning the job.'
                                        : ''
                            })
                            return;
                        }
                    }
                    addWorker(workerDTO, roles, enumActionTypes);
                },
                error: function (err) {
                    // let { jqXHR, status, error } = err
                    // console.log('Error', response.jqXHR, response.status, response.error)
                },
            });
        }

        function DeleteWorker_Click(id) {
            deleteWorker(id, enumActionTypes);
        }


        function AddVehicle_Click() {
            var vehicleDTO = $(ddlVehicle_id).val();
            addVehicle(vehicleDTO, enumActionTypes);
        }

        function DeleteVehicle_Click(id) {
            deleteVehicle(id, enumActionTypes);
        }

        function AddTrip_Click() {
            var tripValuesText = $(hidTripValues_id).val();
            var tripValues = [];
            try {
                tripValues = JSON.parse(tripValuesText);
            } catch (ex) { return; }


            var lastIndex = 0;
            var lastNegativeId = 0;
            for (var i = 0; i < tripValues.length; i++) {
                if (tripValues[i].Index > lastIndex) lastIndex = tripValues[i].Index;
                if (tripValues[i].ID < 0) lastNegativeId = tripValues[i].ID;
            }

            var newTrip = {
                ID: lastNegativeId - 1,
                DeleteImgs: [],
                Index: +lastIndex + 1,
                ActionType: enumActionTypes.indexOf("Create"),
            }
            tripValues.push(newTrip);

            var html = '';
            html += "<div id='t_" + newTrip.ID + "' style='width: 100%; display: flex; justify-content: center; margin-bottom: 25px'>";
            html += "    <div class='col-xs-12 col-md-10' style='border: 1px solid #c2cad8; height: 100%'>";
            html += "       <div class='col-md-offset-2 caption font-dark' style='margin-top: 2.5rem; margin-bottom: 2.5rem; font-size: 1.8rem; display: flex; align-items: center; justify-content: space-between'>";
            html += "           <div>";
            html += "                <i class='fa fa-paper-plane' aria-hidden='true'></i>";
            html += "                <span class='caption-subject bold uppercase'>Trip " + (+lastIndex + 1) + "</span>";
            html += "           </div>";
            html += "           <div>";
            html += "               <button type='button' class='btn red' onclick='DeleteTrip_Click(" + newTrip.ID + ")'><i class='fa fa-times'></i></button>";
            html += "           </div>";
            html += "       </div>";
            html += "       <div class='form-group'>";
            html += "           <label class='col-md-3 control-label'>Job's Period Hours<label style='color: red'>*</label>:</label>";
            html += "           <div class='col-md-5' style='display: flex; flex-wrap: wrap'>";
            html += "               <div>";
            html += "                   <input type='time' id='t_" + newTrip.ID + "_dtStart' onchange='JobTime_Change(this, " + newTrip.ID + ")' data-field='StartTime' />";
            html += "                   <br />";
            html += "                   <br />";
            html += "               </div>";
            html += "               <label style='padding-top: 7px;'>&emsp;&emsp;to&emsp;&emsp;</label>";
            html += "               <div>";
            html += "                   <input type='time' id='t_" + newTrip.ID + "_dtEnd' onchange='JobTime_Change(this, " + newTrip.ID + ")' data-field='EndTime' />";
            html += "                   <br />";
            html += "                   <br />";
            html += "               </div>";
            html += "           </div>";
            html += "       </div>";
            //html += "       <div class='form-group'>";
            //html += "           <label class='col-md-3 control-label'>Logged Period Hours:</label>";
            //html += "           <div class='col-md-6' style='display: flex;'>";
            //html += "               <div>";
            //html += "                   <input type='time' id='t_" + newTrip.ID + "_dtWStart' onchange='JobTime_Change(this, " + newTrip.ID + ", 'WorkerStartedTime')' />";
            //html += "                   <br />";
            //html += "                   <br />";
            //html += "               </div>";
            //html += "               <label style='padding-top: 7px;'>&emsp;&emsp;to&emsp;&emsp;</label>";
            //html += "               <div>";
            //html += "                   <input type='time' id='t_" + newTrip.ID + "_dtWEnd' onchange='JobTime_Change(this, " + newTrip.ID + ", 'WorkerEndedTime')' />";
            //html += "                   <br />";
            //html += "                   <br />";
            //html += "               </div>";
            //html += "           </div>";
            //html += "       </div>";
            //html += "       <div class='form-group'>";
            //html += "           <label class='col-md-3 control-label'>Driver's Remarks:</label>";
            //html += "           <div class='col-md-6'>";
            //html += "               <textarea rows='3' cols='20' class='form-control' placeholder='Remarks' id='txtRemarks' onchange='Remarks_Change(this, " + newTrip.ID + ")'> </textarea>";
            //html += "               <br />";
            //html += "           </div>";
            //html += "       </div>";
            //html += "       <div class='form-group' style='margin-bottom: 3rem'>";
            //html += "           <label class='col-md-3 control-label'>Before Images:</label>";
            //html += "           <div class='col-md-6'>";
            //html += "               <div id='tbdBeforeImage' style='display: flex; flex-wrap: wrap;'>";
            //html += "               </div>";
            //html += "           </div>";
            //html += "       </div>";
            //html += "       <div class='form-group' style='margin-bottom: 4rem'>";
            //html += "           <label class='col-md-3 control-label'>After Images:</label>";
            //html += "           <div class='col-md-6'>";
            //html += "               <div id='tbdAfterImage' style='display: flex; flex-wrap: wrap;'>";
            //html += "               </div>";
            //html += "           </div>";
            //html += "       </div>";
            //html += "       <div class='form-group grpSignatureImage'>";
            //html += "           <label class='col-md-3 control-label'>Customer Signature:</label>";
            //html += "           <div class='col-md-6'>";
            //html += "               <div id='signatureImage' style='border: 1px dashed #808080; min-height: 34px;'>";
            //html += "                   <a href='#' target='_blank'>";
            //html += "                       <img style='width: 100%' src='' />";
            //html += "                   </a>";
            //html += "               </div>";
            //html += "               <br />";
            //html += "           </div>";
            //html += "       </div>";
            html += "   </div>";
            html += "</div>";
            $('#trips').append(html);

            var newTripValuesText = JSON.stringify(tripValues);
            $(hidTripValues_id).val(newTripValuesText);
        }

        function DeleteTrip_Click(tripId) {
            var tripValuesText = $(hidTripValues_id).val();
            var tripValues = [];
            try {
                tripValues = JSON.parse(tripValuesText);
            } catch (ex) { return; }

            for (var i = 0; i < tripValues.length; i++) {
                if (tripValues[i].ID == tripId) {
                    if (tripId > 0) {
                        tripValues[i].ActionType = enumActionTypes.indexOf("Hide");
                        tripValues.splice(i, 1, tripValues[i]);
                    } else {
                        tripValues.splice(i, 1);
                    }
                    break;
                }
            }

            var newTripValuesText = JSON.stringify(tripValues);
            $(hidTripValues_id).val(newTripValuesText);

            $('#t_' + tripId).remove();

            handleJobDateTimeChange();
        }


        function JobTime_Change(cb, tripId, field) {
            field = field ? field : $('#' + cb.id).attr('data-field');

            var tripValuesText = $(hidTripValues_id).val();
            var tripValues = [];
            try {
                tripValues = JSON.parse(tripValuesText);
            } catch (ex) { return; }

            for (var i = 0; i < tripValues.length; i++) {
                if (tripValues[i].ID == tripId) {
                    tripValues[i][field] = cb.value;
                    if (tripId > 0) tripValues[i].ActionType = enumActionTypes.indexOf("Update");
                    tripValues.splice(i, 1, tripValues[i]);
                    break;
                }
            }

            var newTripValuesText = JSON.stringify(tripValues);
            $(hidTripValues_id).val(newTripValuesText);

            handleJobDateTimeChange();
        }

        function Remarks_Change(cb, tripId) {
            var tripValuesText = $(hidTripValues_id).val();
            var tripValues = [];
            try {
                tripValues = JSON.parse(tripValuesText);
            } catch (ex) { return; }

            for (var i = 0; i < tripValues.length; i++) {
                if (tripValues[i].ID == tripId) {
                    tripValues[i].Remarks = cb.value;
                    if (tripId > 0) { tripValues[i].ActionType = enumActionTypes.indexOf("Update"); }
                    tripValues.splice(i, 1, tripValues[i]);
                    break;
                }
            }

            var newTripValuesText = JSON.stringify(tripValues);
            $(hidTripValues_id).val(newTripValuesText);
        }

        function JobNumber_Change(cb, tripId) {
            var tripValuesText = $(hidTripValues_id).val();
            var tripValues = [];
            try {
                tripValues = JSON.parse(tripValuesText);
            } catch (ex) { return; }

            for (var i = 0; i < tripValues.length; i++) {
                if (tripValues[i].ID == tripId) {
                    tripValues[i].JobNumber = cb.value;
                    if (tripId > 0) { tripValues[i].ActionType = enumActionTypes.indexOf("Update"); }
                    tripValues.splice(i, 1, tripValues[i]);
                    break;
                }
            }

            var newTripValuesText = JSON.stringify(tripValues);
            $(hidTripValues_id).val(newTripValuesText);
        }

        function DeleteJobImg_Click(tripId, imgId, imgType) {
            var tripValuesText = $(hidTripValues_id).val();
            var tripValues = [];
            try {
                tripValues = JSON.parse(tripValuesText);
            } catch (ex) { return; }

            for (var i = 0; i < tripValues.length; i++) {
                if (tripValues[i].ID == tripId) {
                    tripValues[i].DeleteImgs = Array.isArray(tripValues[i].DeleteImgs) ? tripValues[i].DeleteImgs : [];
                    if (tripValues[i].DeleteImgs.indexOf(imgId) === -1) {
                        tripValues[i].DeleteImgs.push(imgId);
                        tripValues.splice(i, 1, tripValues[i]);
                    }
                    break;
                }
            }

            var newtripValuesText = JSON.stringify(tripValues);

            $(hidTripValues_id).val(newtripValuesText);
            var imgIdPrefix = imgType === 'before' ? 'beimg_' : 'afimg_';
            $('#' + imgIdPrefix + imgId).remove();
        }


        function handleJobDateTimeChange() {
            var tripValuesText = $(hidTripValues_id).val();
            var tripValues = [];
            try {
                tripValues = JSON.parse(tripValuesText);
            } catch (ex) { return; }

            var workingDate = $(dtWorkingDate_id).val();
            if (!workingDate) return;

            var leavesRes = [];
            GetLeavesByDateAjax().done(function (leavesAjaxRes) {
                leavesRes = (leavesAjaxRes[0] || {}).d || [];
            })

            var vehiclesRes = [];
            var workersRes = [];

            if (tripValues.length === 0) {
                $.when(
                    GetAvailableVehiclesAjax('', ''),
                    GetAvailableWorkersAjax('', ''),
                ).done(function (vehiclesAjaxRes, workersAjaxRes) {
                    if (!vehiclesAjaxRes && !workersAjaxRes) return;

                    var _vRes = [];
                    if (vehiclesAjaxRes) {
                        _vRes = (vehiclesAjaxRes[0] || {}).d || [];
                        vehiclesRes = vehiclesRes.length > 0 ? getSameItems(vehiclesRes, _vRes, 'ID') : _vRes;
                    }
                    var _wRes = [];
                    if (workersAjaxRes) {
                        _wRes = (workersAjaxRes[0] || {}).d || [];
                        workersRes = workersRes.length > 0 ? getSameItems(workersRes, _wRes, 'ID') : _wRes;
                    }

                    vehiclesRes = removeDuplicates(vehiclesRes, 'ID');
                    SetVehicleDropdownData(vehiclesRes);

                    workersRes = removeDuplicates(workersRes, 'ID');
                    SetWorkerDropdownData(workersRes);

                }).fail(function (jqXHR, textStatus, errorThrown) { })

                return;
            }


            var promises = [];
            var results = [];
            $.each(tripValues, function (i, curTrip) {
                if (curTrip.ActionType == enumActionTypes.indexOf("Hide") || !curTrip.StartTime || !curTrip.EndTime) return;

                promises.push(
                    $.when(
                        GetAvailableVehiclesAjax(curTrip.StartTime, curTrip.EndTime),
                        GetAvailableWorkersAjax(curTrip.StartTime, curTrip.EndTime),
                    ).done(function (vehiclesAjaxRes, workersAjaxRes) {
                        if (!vehiclesAjaxRes && !workersAjaxRes) return;

                        var _vRes = [];
                        if (vehiclesAjaxRes) _vRes = (vehiclesAjaxRes[0] || {}).d || [];
                        var _wRes = [];
                        if (workersAjaxRes) _wRes = (workersAjaxRes[0] || {}).d || [];

                        results.push({
                            vehiclesRes: _vRes,
                            workersRes: _wRes,
                        })

                    }).fail(function (jqXHR, textStatus, errorThrown) { })
                );
            });

            $.when.apply($, promises).then(function () {
                $.each(results, function (i, curResult) {
                    if (curResult.vehiclesRes) vehiclesRes = vehiclesRes.length > 0 ? getSameItems(vehiclesRes, curResult.vehiclesRes, 'ID') : curResult.vehiclesRes;
                    if (curResult.workersRes) workersRes = workersRes.length > 0 ? getSameItems(workersRes, curResult.workersRes, 'ID') : curResult.workersRes;

                    if (i !== results.length - 1) return;

                    vehiclesRes = removeDuplicates(vehiclesRes, 'ID');
                    SetVehicleDropdownData(vehiclesRes);
                    var invalidVehicles = GetInvalidVehicles(vehiclesRes);
                    var vehicleError = GetVehicleError(invalidVehicles);

                    workersRes = removeDuplicates(workersRes, 'ID');
                    SetWorkerDropdownData(workersRes);
                    var invalidWorkers = GetInvalidWorkers(workersRes, leavesRes);
                    var workerError = GetWorkerError(invalidWorkers.invalidWorkers, invalidWorkers.leaveWorkers);

                    vehicleError = vehicleError.trim();
                    workerError = workerError.trim();
                    if (!vehicleError && !workerError) return;
                    AlertError(vehicleError && workerError ? vehicleError + '<br />' + workerError : (vehicleError || workerError))
                });
            });


        }

        function AlertError(error) {
            Swal.fire({
                icon: 'error',
                // title: 'Oops...',
                html: error
            });
        }

        function GetLeavesByDateAjax() {
            var workingDate = $(dtWorkingDate_id).val();
            if (!workingDate) return;

            return ajaxRequest({
                method: 'POST',
                url: 'AjaxHandler.asmx/GetLeavesByDate',
                body: { workingDates: workingDate }
            });
        }


        function GetAvailableVehiclesAjax(start, end) {
            var workingDate = $(dtWorkingDate_id).val();
            if (!workingDate || (!start && end) || (!end && start)
                // || !(start.replace("12:", "24:").replace("00:", "12:") < end.replace("12:", "24:").replace("00:", "12:"))
            ) return;

            return ajaxRequest({
                method: 'POST',
                url: 'AjaxHandler.asmx/GetAvailableVehicles',
                body: { jobID: <%= thisJob.ID %>, workingDates: workingDate, start, end },
                beforeSend: function () {
                    $(ddlVehicle_id).empty();
                }
            });
        }

        function SetVehicleDropdownData(vehiclesRes) {
            $(ddlVehicle_id).empty();
            var placeholderOption = "<option value=''>-- Select Vehicle --</option>";
            if (vehiclesRes.length === 0) placeholderOption = "<option value=''>-- No vehicles found --</option>";
            $(ddlVehicle_id).append(placeholderOption);
            for (var i = 0; i < vehiclesRes.length; i++) {
                if (vehiclesRes[i].ID > 0) {
                    var _vehicle = {
                        JobVehicleID: 0,
                        ID: vehiclesRes[i].ID,
                        Number: vehiclesRes[i].Number,
                        Model: vehiclesRes[i].Model
                    }
                    var newOption = "<option value='" + JSON.stringify(_vehicle) + "'>" + _vehicle.Number + " (" + _vehicle.Model + ")</option>";
                    $(ddlVehicle_id).append(newOption);
                }
            }
        }

        function GetInvalidVehicles(vehiclesRes) {
            var vehicleValues = $('#VehicleValues').val() || JSON.stringify([]);
            try {
                vehicleValues = JSON.parse(vehicleValues);
            } catch (e) {
            }
            var invalidVehicles = vehicleValues.filter(function (v) {
                var isExist = vehiclesRes.some(function (_v) {
                    return _v.ID === v.ID;
                });
                return !isExist && $('#vehicle_' + v.ID).length > 0;
            });
            if (invalidVehicles.length === 0) return [];
            invalidVehicles.forEach(function (v) {
                DeleteVehicle_Click(v.ID);
            });

            return invalidVehicles;
        }

        function GetVehicleError(invalidVehicles) {
            var vehicleError = '';
            if (Array.isArray(invalidVehicles) && invalidVehicles.length > 0) {
                vehicleError = (invalidVehicles.length > 1 ? 'Some vehicles "' : 'This vehicle "') + invalidVehicles.filter(function (v) { return !!v.Number }).map(function (v) { return v.Number + (v.Model ? ' (' + v.Model + ')' : '') }).join(', ') + (invalidVehicles.length > 1 ? '" were ' : '" was ') + 'occupied in this Date & Period Hours!';
            }
            return vehicleError;
        }


        function GetAvailableWorkersAjax(start, end) {
            var workingDate = $(dtWorkingDate_id).val();
            if (!workingDate || (!start && end) || (!end && start)
                // || !(start.replace("12:", "24:").replace("00:", "12:") < end.replace("12:", "24:").replace("00:", "12:"))
            ) return;

            return ajaxRequest({
                method: 'POST',
                url: 'AjaxHandler.asmx/GetAvailableWorkers',
                body: { jobID: <%= thisJob.ID %>, workingDates: workingDate, start, end },
                beforeSend: function () {
                    $(ddlWorker_id).empty();
                }
            });
        }

        function SetWorkerDropdownData(workersRes) {
            $(ddlWorker_id).empty();
            var placeholderOption = "<option value=''>-- Select Worker --</option>";
            if (workersRes.length === 0) placeholderOption = "<option value=''>-- No workers found --</option>";
            $(ddlWorker_id).append(placeholderOption);
            for (var i = 0; i < workersRes.length; i++) {
                if (workersRes[i].ID > 0) {
                    var _worker = {
                        UserJobID: 0,
                        ID: workersRes[i].ID,
                        Name: workersRes[i].Name,
                        IDNumber: workersRes[i].IDNumber,
                        Phone: workersRes[i].Phone,
                        Email: workersRes[i].Email,
                        RoleID: workersRes[i].RoleID
                    }
                    var newOption = "<option value='" + JSON.stringify(_worker) + "'>" + _worker.Name + " (" + _worker.IDNumber + ")</option>";
                    $(ddlWorker_id).append(newOption);
                }
            }
        }

        function GetInvalidWorkers(workersRes, leavesRes) {
            var workerValues = $('#WorkerValues').val() || JSON.stringify([]);
            try {
                workerValues = JSON.parse(workerValues);
            } catch (e) {
            }
            var invalidWorkers = workerValues.filter(function (w) {
                var isExist = workersRes.some(function (_w) {
                    return _w.ID === w.ID;
                });
                return !isExist && $('#w_' + w.ID).length > 0;
            });
            invalidWorkers.forEach(function (w) {
                DeleteWorker_Click(w.ID);
            });
            if (leavesRes.length === 0) return {
                invalidWorkers,
                leaveWorkers: [],
            }

            //
            var workerValues = $('#WorkerValues').val() || JSON.stringify([]);
            try {
                workerValues = JSON.parse(workerValues);
            } catch (e) {
            }
            var leaveWorkers = workerValues.map(function (w) {
                var hasApproved = false;
                var hasPending = false;
                for (var i = 0; i < leavesRes.length; i++) {
                    var l = leavesRes[i];
                    if (l.UserID != w.ID || $('#w_' + w.ID).length === 0) continue;
                    if (l.StatusID == '<%= Statuses.Approved.GetHashCode() %>') { // Approved
                        hasApproved = true;
                    } else if (l.StatusID == '<%= Statuses.Pending.GetHashCode() %>') { // Pending
                        hasPending = true;
                    }
                    if (hasApproved || hasPending) break;
                }
                if (hasApproved || hasPending) return w;
                return undefined;
            }).filter(Boolean);
            if (leaveWorkers.length === 0 && invalidWorkers.length === 0) return [];
            leaveWorkers.forEach(function (w) {
                DeleteWorker_Click(w.ID);
            });

            return {
                invalidWorkers,
                leaveWorkers,
            }
        }

        function GetWorkerError(invalidWorkers, leaveWorkers) {
            var workerError = '';
            if (Array.isArray(invalidWorkers) && invalidWorkers.length > 0) {
                workerError = (invalidWorkers.length > 1 ? 'Some workers "' : 'This worker "') + invalidWorkers.map(function (w) { return w.Name + (w.IDNumber ? ' (' + w.IDNumber + ')' : '') }).join(', ') + (invalidWorkers.length > 1 ? '" were ' : '" was ') + 'busy in this Date & Period Hours!';
            }

            if (leaveWorkers.length === 0) return workerError;
            leaveWorkers.forEach(function (w, wIndex) {
                var hasApproved = false;
                var hasPending = false;
                for (var i = 0; i < leaveWorkers.length; i++) {
                    var l = leaveWorkers[i];
                    if (l.UserID != w.ID || $('#w_' + w.ID).length === 0) continue;
                    if (l.StatusID == '<%= Statuses.Approved.GetHashCode() %>') { // Approved
                        hasApproved = true;
                    } else if (l.StatusID == '<%= Statuses.Pending.GetHashCode() %>') { // Pending
                        hasPending = true;
                    }
                    if (hasApproved || hasPending) break;
                }
                if (hasApproved || hasPending) {
                    if (wIndex === 0) workerError += '<br />';
                    workerError += hasApproved ? 'This ' + (roles[w.RoleID] || '').toLowerCase() + ' "' + w.Name + '" is on leave and cannot be assigned.'
                        : hasPending ? 'This ' + (roles[w.RoleID] || '').toLowerCase() + ' "' + w.Name + '" has pending leave on this day, please reject the leave before assigning the job.'
                            : ''
                    workerError += '<br />';
                }
            })
            return workerError;
        }

    </script>

    <script src="assets/pages/scripts/job/description.js"></script>
    <script src="assets/pages/scripts/job/worker.js"></script>
    <script src="assets/pages/scripts/job/vehicle.js"></script>
    <%--<script src="assets/pages/scripts/job/jobImage.js"></script>--%>
</asp:Content>
