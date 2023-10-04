<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="UpdateOT.aspx.cs" Inherits="AvaChemAdminPanelMobiAPI.UpdateOT" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>AvaChem | Update OT</title>
    <script src="assets/global/scripts/alertJquery.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- BEGIN CONTENT BODY -->
    <div class="page-content">
        <!-- BEGIN PAGE HEADER-->
        <h1 class="page-title">Update OT
                        <small>update OT in the system</small>
        </h1>
        <div class="page-bar">
            <ul class="page-breadcrumb">
                <li>
                    <i class="icon-home"></i>
                    <a href="<%= DASHBOARD %>">Home</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <a href="<%= OT %>">OT</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <span>Update OT</span>
                </li>
            </ul>
        </div>
        <!-- END PAGE HEADER-->
        <!-- START OF UPDATE OT PORTLET -->
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
                            <span class="caption-subject bold uppercase">Update OT</span>
                        </div>
                    </div>
                    <div class="portlet-body form" runat="server">
                        <div class="form-horizontal">
                            <div class="form-body">
                                <div class="form-group">
                                    <label class="col-md-3 control-label">Status<label style="color: red">*</label>:</label>
                                    <div class="col-md-5">
                                        <asp:DropDownList class="form-control" ID="ddlStatus" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator
                                            runat="server"
                                            Display="Dynamic"
                                            CssClass="txt-error"
                                            ControlToValidate="ddlStatus"
                                            ErrorMessage="Please fill in the blank!"></asp:RequiredFieldValidator>
                                        <br />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-3 control-label">Driver's Working Hours<label style="color: red">*</label>:</label>
                                    <div class="col-md-5" style="display: flex; flex-wrap: wrap">
                                        <div>
                                            <input type="time" id="dtDStart" clientidmode="Static" runat="server" />
                                            <br />
                                            <br />
                                        </div>
                                        <label style="padding-top: 7px;">&emsp;&emsp;to&emsp;&emsp;</label>
                                        <div>
                                            <input type="time" id="dtDEnd" clientidmode="Static" runat="server" />
                                            <br />
                                            <br />
                                        </div>
                                        <asp:RequiredFieldValidator
                                            runat="server"
                                            Display="Dynamic"
                                            CssClass="txt-error"
                                            Style="width: 100%"
                                            ControlToValidate="dtDEnd"
                                            ErrorMessage="Please fill in the blank!"></asp:RequiredFieldValidator>
                                        <%--<asp:CustomValidator
                                            ClientValidationFunction="DEndTimeValidate"
                                            EnableClientScript="true"
                                            ID="cvDEndTime"
                                            runat="server"
                                            Display="Dynamic"
                                            Style="width: 100%"
                                            CssClass="txt-error"
                                            ControlToValidate="dtDEnd"
                                            ErrorMessage="End time must be greater than start time"></asp:CustomValidator>--%>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-3 control-label">Client's Job Working Hours<label style="color: red">*</label>:</label>
                                    <div class="col-md-5" style="display: flex; flex-wrap: wrap">
                                        <div>
                                            <input type="time" id="dtWStart" clientidmode="Static" runat="server" />
                                            <br />
                                            <br />
                                        </div>
                                        <label style="padding-top: 7px;">&emsp;&emsp;to&emsp;&emsp;</label>
                                        <div>
                                            <input type="time" id="dtWEnd" clientidmode="Static" runat="server" />
                                            <br />
                                            <br />
                                        </div>
                                        <asp:RequiredFieldValidator
                                            runat="server"
                                            Display="Dynamic"
                                            CssClass="txt-error"
                                            Style="width: 100%"
                                            ControlToValidate="dtWEnd"
                                            ErrorMessage="Please fill in the blank!"></asp:RequiredFieldValidator>
                                        <%--<asp:CustomValidator
                                            ClientValidationFunction="WEndTimeValidate"
                                            EnableClientScript="true"
                                            ID="cvWEndTime"
                                            runat="server"
                                            Display="Dynamic"
                                            Style="width: 100%"
                                            CssClass="txt-error"
                                            ControlToValidate="dtWEnd"
                                            ErrorMessage="End time must be greater than start time"></asp:CustomValidator>--%>
                                    </div>
                                </div>
                                <%--<div class="form-group">
                                    <label class="col-md-3 control-label">Service Memo / Delivery Order Number:</label>
                                    <div class="col-md-5">
                                        <asp:DropDownList class="form-control" ID="ddlJob" ClientIDMode="Static" runat="server" OnChange="handleJob_Change()"></asp:DropDownList>
                                        <br />
                                        <br />
                                    </div>
                                </div>--%>
                                <div class="form-group">
                                    <label class="col-md-3 control-label">
                                        Service Memo / Delivery Order Number:</label>
                                    <div class="col-md-5">
                                        <asp:TextBox class="form-control" placeholder="Enter Service Memo / Delivery Order Number" ID="tbxJobNumber" ClientIDMode="Static" runat="server"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator
                                            ID="requiredJobName"
                                            ClientIDMode="Static"
                                            runat="server"
                                            Display="Dynamic"
                                            CssClass="txt-error"
                                            ControlToValidate="tbxJobNumber"
                                            ErrorMessage="Please fill in the blank!"></asp:RequiredFieldValidator>--%>
                                        <br />
                                    </div>
                                </div>

                                <div class="form-group grp-vehicles">
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

                                <%--<div class="form-group grp-crews">
                                    <label class="col-md-3 control-label">OT Crews:</label>
                                    <input id="CrewValues" clientidmode="Static" type="hidden" runat="server" />
                                    <div class="col-md-5" style="display: flex">
                                        <div class="col-xs-12" style="padding-left: 0;">
                                            <asp:DropDownList class="form-control" ID="ddlCrew" ClientIDMode="Static" runat="server"></asp:DropDownList>
                                        </div>
                                        <div class="text-right" style="padding: 0;">
                                            <button type="button" class="btn btn-primary" onclick="AddCrew_Click()"><i class="fa fa-plus" aria-hidden="true"></i></button>
                                        </div>
                                    </div>
                                </div>--%>
                                <div class="form-group">
                                    <label id="lblDriver" class="col-md-3 control-label">Driver:</label>
                                    <div class="col-md-6">
                                        <div class="table-responsive">
                                            <table class="table table-striped table-bordered table-hover" style="font-size: 13px">
                                                <thead>
                                                    <tr>
                                                        <th>Name</th>
                                                        <th>ID Number</th>
                                                        <th class="text-center">Phone</th>
                                                        <th>Email</th>
                                                        <%--<th class="text-center">Role</th>--%>
                                                        <%--<th class="text-center">
                                                            <button type="button" class="btn btn-danger btn-sm" style="pointer-events: none"><i class="fa fa-times" aria-hidden="true"></i></button>
                                                        </th>--%>
                                                    </tr>
                                                </thead>
                                                <tbody id="tbdDriver">
                                                    <%foreach (User c in listDriver)
                                                        { %>
                                                    <tr id="d_<%Response.Write(c.ID); %>">
                                                        <td id="d_<%Response.Write(c.ID); %>_Name"><%Response.Write(c.Name); %></td>
                                                        <td id="d_<%Response.Write(c.ID); %>_IDNumber"><%Response.Write(c.IDNumber); %></td>
                                                        <td id="d_<%Response.Write(c.ID); %>_Phone" class="text-center"><%Response.Write(c.Phone); %></td>
                                                        <td id="d_<%Response.Write(c.ID); %>_Email"><%Response.Write(c.Email); %></td>
                                                        <%--<td id="d_<%Response.Write(c.ID); %>_RoleID" class="text-center"><% Response.Write(((UserRoles)c.RoleID).ToString()); %></td>--%>
                                                        <%--<td class="text-center">
                                                            <button type="button" class="btn btn-danger btn-sm"
                                                                onclick="DeleteCrew_Click('<%Response.Write(c.ID); %>')">
                                                                <i class="fa fa-times" aria-hidden="true"></i>
                                                            </button>
                                                        </td>--%>
                                                    </tr>
                                                    <% } %>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label id="lblCrews" class="col-md-3 control-label">OT Crews:</label>
                                    <div class="col-md-6">
                                        <div class="table-responsive">
                                            <table class="table table-striped table-bordered table-hover" style="font-size: 13px">
                                                <thead>
                                                    <tr>
                                                        <th>Name</th>
                                                        <th>ID Number</th>
                                                        <th class="text-center">Phone</th>
                                                        <th>Email</th>
                                                        <%--<th class="text-center">Role</th>--%>
                                                        <%--<th class="text-center">
                                                            <button type="button" class="btn btn-danger btn-sm" style="pointer-events: none"><i class="fa fa-times" aria-hidden="true"></i></button>
                                                        </th>--%>
                                                    </tr>
                                                </thead>
                                                <tbody id="tbdCrew">
                                                    <%foreach (User c in listCrews)
                                                        { %>
                                                    <tr id="c_<%Response.Write(c.ID); %>">
                                                        <td id="c_<%Response.Write(c.ID); %>_Name"><%Response.Write(c.Name); %></td>
                                                        <td id="c_<%Response.Write(c.ID); %>_IDNumber"><%Response.Write(c.IDNumber); %></td>
                                                        <td id="c_<%Response.Write(c.ID); %>_Phone" class="text-center"><%Response.Write(c.Phone); %></td>
                                                        <td id="c_<%Response.Write(c.ID); %>_Email"><%Response.Write(c.Email); %></td>
                                                        <%--<td id="c_<%Response.Write(c.ID); %>_RoleID" class="text-center"><% Response.Write(((UserRoles)c.RoleID).ToString()); %></td>--%>
                                                        <%--<td class="text-center">
                                                            <button type="button" class="btn btn-danger btn-sm"
                                                                onclick="DeleteCrew_Click('<%Response.Write(c.ID); %>')">
                                                                <i class="fa fa-times" aria-hidden="true"></i>
                                                            </button>
                                                        </td>--%>
                                                    </tr>
                                                    <% } %>
                                                </tbody>
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
        <!-- END OF UPDATE OT PORTLET -->
    </div>
    <!-- END CONTENT BODY -->

    <script>
        var enumActionTypes = <%= dataActionTypes %>; // ["Add",]
        var roles = <%= convertRoles %>; // [{"Key":1,"Value":"Admin"},...]
        //var ddlJob_id = '#ddlJob';
        //var ddlCrew_id = '#ddlCrew';

        var dtDStart_id = '#dtDStart';
        var dtDEnd_id = '#dtDEnd';
        var dtWStart_id = '#dtWStart';
        var dtWEnd_id = '#dtWEnd';

        // Document ready
        $(function () {
            //$(ddlJob_id).select2();
            //$(ddlCrew_id).select2();

            if (Array.isArray(roles)) {
                roles = roles.reduce(function (res, cur) {
                    res[cur.Key] = cur.Value;
                    return res;
                }, {})
            }

            <%--$(dtDStart_id + "," + dtDEnd_id).change(function () {
                try {
                    window.ValidatorValidate(window.<%= cvDEndTime.ClientID %>);
                } catch (_e) {
                }
            });
            $(dtWStart_id + "," + dtWEnd_id).change(function () {
                try {
                    window.ValidatorValidate(window.<%= cvWEndTime.ClientID %>);
                } catch (_e) {
                }
            });--%>


            //
            //handleJob_Change();
        });

        //var tbxJobName_id = '#tbxJobName';
        //var grpCrews_class = '.grp-crews';
        //var lblCrews_id = '#lblCrews';

        //var hidCrewValues_id = '#CrewValues';

        //var grpVehicles_class = '.grp-vehicles';

        //var isFirstRender = true;
        //var lastSelectedJob;
        <%--function handleJob_Change() {
            var selectedJob = $('#<%= ddlJob.ClientID %>').find('option:selected').val();
            try {
                if (selectedJob == "0") {
                    $(grpCrews_class).hide();
                    $(lblCrews_id).text('OT Crews');
                    $(grpVehicles_class).hide();
                    $(tbxJobName_id).attr('ReadOnly', false);

                    if (isFirstRender) return;

                    $(tbxJobName_id).val('<%= thisOT.JobName %>');
                    if (!$(tbxJobName_id).val()) $('#requiredJobName').show();

                    // Remove all preparing crew values
                    var crewsText = $(hidCrewValues_id).val() || JSON.stringify([]);
                    try {
                        var crews = JSON.parse(crewsText);
                    } catch (ex) { }
                    for (var i = 0; i < crews.length; i++) {
                        $('#c_' + crews[i].ID).remove();
                    }
                    $(hidCrewValues_id).val('[]');

                    ajaxRequest({
                        method: 'POST',
                        url: 'AjaxHandler.asmx/GetCrewsByOT_ID',
                        body: { oid: <%= thisOT.ID %> },
                        success: function (response) {
                            // let { data, status, jqXHR } = response
                            var crewsRes = response.data;
                            for (var i = 0; i < crewsRes.length; i++) {
                                if (crewsRes[i].ID > 0) {
                                    addCrew(JSON.stringify(crewsRes[i]), roles, enumActionTypes);
                                }
                            }
                        },
                        error: function (err) {
                            // let { jqXHR, status, error } = err
                            // console.log('Error', response.jqXHR, response.status, response.error)
                        },
                    });
                } else {
                    $('#requiredJobName').hide();
                    $(grpCrews_class).show();
                    $(lblCrews_id).text('');
                    $(grpVehicles_class).show();
                    try {
                        selectedJob = JSON.parse(selectedJob);
                    } catch (e) {
                    }
                    $(tbxJobName_id).attr('ReadOnly', true);
                    $(tbxJobName_id).val(selectedJob.Name);

                    if (isFirstRender) return;

                    if (lastSelectedJob == "0") {
                        ajaxRequest({
                            method: 'POST',
                            url: 'AjaxHandler.asmx/GetCrewsByOT_ID',
                            body: { oid: <%= thisOT.ID %> },
                            success: function (response) {
                                // let { data, status, jqXHR } = response
                                var crewsRes = response.data;
                                for (var i = 0; i < crewsRes.length; i++) {
                                    if (crewsRes[i].ID > 0) {
                                        addCrew(JSON.stringify(crewsRes[i]), roles, enumActionTypes);
                                    }
                                }
                            },
                            error: function (err) {
                                // let { jqXHR, status, error } = err
                                // console.log('Error', response.jqXHR, response.status, response.error)
                            },
                        });
                    }

                    //
                    ajaxRequest({
                        method: 'POST',
                        url: 'AjaxHandler.asmx/GetWorkersByJobID',
                        body: { jobID: selectedJob.ID },
                        beforeSend: function () {
                            $(ddlCrew_id).empty();
                        },
                        success: function (response) {
                            // let { data, status, jqXHR } = response
                            var workersRes = response.data;
                            var placeholderOption = "<option value=''>-- Select OT Crew --</option>";
                            if (workersRes.length === 0) placeholderOption = "<option value=''>-- No workers found --</option>";
                            $(ddlCrew_id).append(placeholderOption);
                            for (var i = 0; i < workersRes.length; i++) {
                                if (workersRes[i].ID > 0) {
                                    var _worker = {
                                        UserOT_ID: 0,
                                        ID: workersRes[i].ID,
                                        Name: workersRes[i].Name,
                                        IDNumber: workersRes[i].IDNumber,
                                        Phone: workersRes[i].Phone,
                                        Email: workersRes[i].Email,
                                        RoleID: workersRes[i].RoleID
                                    }
                                    var newOption = "<option value='" + JSON.stringify(_worker) + "'>" + _worker.Name + " (" + _worker.IDNumber + ")</option>";
                                    $(ddlCrew_id).append(newOption);
                                }
                            }
                        },
                        error: function (err) {
                            // let { jqXHR, status, error } = err
                            // console.log('Error', response.jqXHR, response.status, response.error)
                        },
                    });

                    //
                    ajaxRequest({
                        method: 'POST',
                        url: 'AjaxHandler.asmx/GetVehicles',
                        body: { jobID: selectedJob.ID },
                        beforeSend: function () {
                            $('#tbdVehicle').empty();
                        },
                        success: function (response) {
                            // let { data, status, jqXHR } = response
                            var vehiclesRes = response.data;
                            for (var i = 0; i < vehiclesRes.length; i++) {
                                var newVehicle = vehiclesRes[i];

                                var html = $('#tbdVehicle').html();
                                html += "<tr id='vehicle_" + newVehicle.ID + "'>";
                                html += "     <td id='vehicle_" + newVehicle.ID + "_Number'>" + newVehicle.Number + "</td>";
                                html += "     <td id='vehicle_" + newVehicle.ID + "_Model'>" + newVehicle.Model + "</td>";
                                html += "</tr>";
                                $('#tbdVehicle').html(html);
                            }
                        },
                        error: function (err) {
                            // let { jqXHR, status, error } = err
                            // console.log('Error', response.jqXHR, response.status, response.error)
                        },
                    });
                }
            } catch (e) {
            } finally {
                lastSelectedJob = selectedJob;
                isFirstRender = false;
            }
        }--%>

        //function AddCrew_Click() {
        //    var crewDTO = $(ddlCrew_id).val();
        //    addCrew(crewDTO, roles, enumActionTypes);
        //}

        //function DeleteCrew_Click(id) {
        //    deleteCrew(id, enumActionTypes);
        //}


        //function DEndTimeValidate(source, arguments) {
        //    arguments.IsValid = false;

        //    var start = $(dtDStart_id).val().replace("12:", "24:").replace("00:", "12:");
        //    var end = $(dtDEnd_id).val().replace("12:", "24:").replace("00:", "12:");
        //    if (!start || !end) {
        //        arguments.IsValid = false;
        //        return;
        //    }
        //    arguments.IsValid = start < end;
        //}
        //function WEndTimeValidate(source, arguments) {
        //    arguments.IsValid = false;

        //    var start = $(dtWStart_id).val().replace("12:", "24:").replace("00:", "12:");
        //    var end = $(dtWEnd_id).val().replace("12:", "24:").replace("00:", "12:");
        //    if (!start || !end) {
        //        arguments.IsValid = false;
        //        return;
        //    }
        //    arguments.IsValid = start < end;
        //}
    </script>

    <%--<script src="assets/pages/scripts/ot/crew.js"></script>--%>
</asp:Content>

