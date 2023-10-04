<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" EnableEventValidation="false" Async="true" AutoEventWireup="true" CodeBehind="CreateLeave.aspx.cs" Inherits="AvaChemAdminPanelMobiAPI.CreateLeave" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>AvaChem | Apply Leave</title>
    <script src="assets/global/scripts/alertJquery.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- BEGIN CONTENT BODY -->
    <div class="page-content">
        <!-- BEGIN PAGE HEADER-->
        <h1 class="page-title">Apply Leave
                        <small>create leave in the system</small>
        </h1>
        <div class="page-bar">
            <ul class="page-breadcrumb">
                <li>
                    <i class="icon-home"></i>
                    <a href="<%= DASHBOARD %>">Home</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <a href="<%= LEAVES %>">Leaves</a>
                    <i class="fa fa-angle-right"></i>
                </li>
                <li>
                    <span>Apply Leave</span>
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
                            <span class="caption-subject bold uppercase">Apply Leave</span>
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
                                        <span class="caption bold" id="spnDaysLeft" style="padding-left: 12px"></span>
                                        <br />
                                        <br />
                                    </div>
                                </div>
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
                                    <label class="col-md-3 control-label">Reason<label style="color: red">*</label>:</label>
                                    <div class="col-md-5">
                                        <asp:DropDownList class="form-control" ID="ddlReason" ClientIDMode="Static" runat="server"></asp:DropDownList>
                                        <asp:RequiredFieldValidator
                                            runat="server"
                                            Display="Dynamic"
                                            CssClass="txt-error"
                                            ControlToValidate="ddlReason"
                                            ErrorMessage="Please fill in the blank!"></asp:RequiredFieldValidator>
                                        <br />
                                        <br />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-3 control-label">Start Date<label style="color: red">*</label>:</label>
                                    <div class="col-md-5">
                                        <input id="dtStart" clientidmode="Static" type="date" value="" runat="server" />
                                        <asp:RequiredFieldValidator
                                            runat="server"
                                            Display="Dynamic"
                                            CssClass="txt-error"
                                            ControlToValidate="dtStart"
                                            ErrorMessage="Please fill in the blank!"></asp:RequiredFieldValidator>
                                        <br />
                                        <br />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-3 control-label">End Date<label style="color: red">*</label>:</label>
                                    <div class="col-md-5">
                                        <input id="dtEnd" clientidmode="Static" type="date" value="" runat="server" />
                                        <asp:RequiredFieldValidator
                                            runat="server"
                                            Display="Dynamic"
                                            CssClass="txt-error"
                                            ControlToValidate="dtEnd"
                                            ErrorMessage="Please fill in the blank!"></asp:RequiredFieldValidator>
                                        <asp:CompareValidator
                                            runat="server"
                                            Display="Dynamic"
                                            CssClass="txt-error"
                                            EnableClientScript="true"
                                            CultureInvariantValues="true"
                                            ControlToCompare="dtStart"
                                            ControlToValidate="dtEnd"
                                            ErrorMessage="End date must be greater than start date"
                                            Type="Date"
                                            SetFocusOnError="true"
                                            Operator="GreaterThanEqual"></asp:CompareValidator>
                                        <br />
                                        <br />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-3 control-label">No. of Days<label style="color: red">*</label>:</label>
                                    <div class="col-md-5">
                                        <input id="tbxNumDays" type="number" class="form-control" min="0" step="0.5" placeholder="Enter No. of days" runat="server" />
                                        <asp:RequiredFieldValidator
                                            runat="server"
                                            Display="Dynamic"
                                            CssClass="txt-error"
                                            ControlToValidate="tbxNumDays"
                                            ErrorMessage="Please fill in the blank!"></asp:RequiredFieldValidator>
                                        <br />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-3 control-label">Remarks:</label>
                                    <div class="col-md-5">
                                        <textarea id="tbxRemarks" class="form-control" placeholder="Remarks" runat="server"></textarea><br />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-3 control-label">Image (Optional):</label>
                                    <div class="col-md-5">
                                        <asp:FileUpload Style="display: none;" accept=".png,.jpg,.jpeg" class="form-control" ID="fuUpload" ClientIDMode="Static" runat="server"></asp:FileUpload>
                                        <button class="btn" type="button" onclick="(function(e){ $('#fuUpload').trigger('click'); })(arguments[0]);return false;">Choose Image</button>
                                        <br />
                                        <%--<asp:RegularExpressionValidator ID="regexValidator" runat="server" ControlToValidate="fuUpload" ErrorMessage="Only csv files are allowed" ValidationExpression="(.*\.([cC][sS][vV])$)"></asp:RegularExpressionValidator>--%>
                                    </div>
                                </div>
                                <div class="form-group grpImage">
                                    <label class="col-md-3 control-label">
                                    </label>
                                    <div class="col-md-5">
                                        <asp:HyperLink NavigateUrl="#" ID="lnkImage" ClientIDMode="Static" Target="_blank" runat="server">
                                            <asp:Image Style="width: 100%" ID="imgProof" ClientIDMode="Static" runat="server"></asp:Image>
                                        </asp:HyperLink>
                                        <br />
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-md-offset-3 col-md-9">
                                        <asp:Button CausesValidation="False" CssClass="btn red btn-outline" ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                                        <asp:Button CssClass="btn green uppercase" ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" ClientIDMode="Static" />

                                        <%--UseSubmitBehavior="False" 
                                            OnClientClick="ValidateLeave(this)"--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- END CONTENT BODY -->

    <script src="assets/pages/plugins/sweetalert2/sweetalert2.all.min.js"></script>
    <script>
        var ddlUser_id = '#ddlUser';
        var ddlReason_id = '#ddlReason';

        var grpImage_class = '.grpImage';
        var lnkImage_id = '#lnkImage';
        var imgProof_id = '#imgProof';

        var spnDaysLeft_id = '#spnDaysLeft';

        // Document ready
        $(function () {
            $(ddlUser_id).select2();
            $(ddlReason_id).select2();

            handleUser_Change();

            if (!$(imgProof_id).attr('src')) $(grpImage_class).hide();
            $("#fuUpload").change(function (event) {
                try {
                    if (!event.target.files || !event.target.files[0]) {
                        if (!$(imgProof_id).attr('src')) $(grpImage_class).hide();
                        return;
                    }
                    var uploadedFile = event.target.files[0];
                    var uploadedFileUrl = URL.createObjectURL(uploadedFile);
                    if (!uploadedFileUrl) {
                        if (!$(imgProof_id).attr('src')) $(grpImage_class).hide();
                        return;
                    }
                    $(lnkImage).attr("href", uploadedFileUrl);
                    $(imgProof_id).attr('src', uploadedFileUrl);
                    $(grpImage_class).show();
                } catch (e) {
                }
            });

            function validated(isValid) {
                if (!isValid) return;
                __doPostBack('<%= btnSubmit.UniqueID %>', '');
            }
            var form_id = $('#btnSubmit').closest("form").attr('id');
            $('#' + form_id).submit(function (event) {
                event.preventDefault();
                ValidateLeave(validated);

            });
        });

        function ValidateLeave(cb) {
            var selectedUser = $(ddlUser_id).val();
            if (!selectedUser) return cb(false);

            selectedUser = JSON.parse(selectedUser);
            var from = $('#dtStart').val();
            var to = $('#dtEnd').val();
            ajaxRequest({
                method: 'POST',
                url: 'AjaxHandler.asmx/GetJobsByDate',
                body: { uid: selectedUser.ID, from, to },
                success: function (response) {
                    // let { data, status, jqXHR } = response
                    var jobsRes = response.data;
                    if (jobsRes.length > 0) {
                        /*var error = 'This user have job assigned during the selected period (Service Memo / Delivery Order Number: ' + jobsRes.map(function (j) { return '#' + j.JobNumber }).join(', ') + ')';*/
                        var error = 'This user have job assigned during the selected period (Job: ' + jobsRes.map(function (j) { return j.Name }).join(', ') + ')';
                        Swal.fire({
                            icon: 'error',
                            text: error
                        })
                        return cb(false);
                    }
                    return cb(true);
                },
                error: function (err) {
                    return cb(true);
                    // let { jqXHR, status, error } = err
                    // console.log('Error', response.jqXHR, response.status, response.error)
                },
            });
        }

        function handleUser_Change() {
            var selectedUser = $(ddlUser_id).val();
            try {
                if (!selectedUser) return;
                selectedUser = JSON.parse(selectedUser);
                $(spnDaysLeft_id).text("Leave Days Left: " + selectedUser.LeaveDaysLeft + ", MC Days Left: " + selectedUser.MCDaysLeft);
            } catch (e) {
            }
        }
    </script>
</asp:Content>
