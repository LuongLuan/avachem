<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="Calendar.aspx.cs" Inherits="AvaChemAdminPanelMobiAPI.Calendar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="assets/pages/plugins/calendar/css/main.min.css" rel='stylesheet' />
    <link href="assets/pages/css/calendar.css" rel='stylesheet' />
    <%--<style>
        .fc .fc-daygrid-day-events .fc-daygrid-event {
            flex-wrap: wrap;
        }
        .fc .fc-daygrid-day-events .fc-event-title {
            overflow-x: auto;
        }
    </style>--%>

    <title>AvaChem | Calendar</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-content">
        <div style="display: flex;">
            <div style="display: flex; margin-right: 12px">
                <span style="margin-right: 6px; width: 2rem; height: 2rem; border-radius: 50% !important; display: inline-block; background: #3598DC" class="bg-job-completed"></span>
                Job (completed)
            </div>
            <div style="display: flex; margin-right: 12px">
                <span style="margin-right: 6px; width: 2rem; height: 2rem; border-radius: 50% !important; display: inline-block; background: #F59F02" class="bg-job-pending"></span>
                Job (pending)
            </div>
            <div style="display: flex; margin-right: 12px">
                <span style="margin-right: 6px; width: 2rem; height: 2rem; border-radius: 50% !important; display: inline-block; background: #ff9f89"></span>
                Leave (completed)
            </div>
            <div style="display: flex; margin-right: 12px">
                <span style="margin-right: 6px; width: 2rem; height: 2rem; border-radius: 50% !important; display: inline-block; background: #18cdc4"></span>
                Leave (pending)
            </div>
        </div>
        <br />

        <div class="portlet box blue">
            <div class="portlet-body">
                <div id="calendar"></div>
            </div>
        </div>
    </div>

    <script src="assets/pages/plugins/sweetalert2/sweetalert2.all.min.js"></script>
    <script src="assets/pages/plugins/popper/js/popper.min.js"></script>
    <script src="assets/pages/plugins/tooltip/js/tooltip.min.js"></script>
    <script src="assets/pages/plugins/calendar/js/main.min.js"></script>
    <script>
        var events = JSON.parse('<%= list %>');

        // Document ready
        $(function () {
            generateCalendar(events);
            //$.when(
            //    ajaxRequest({
            //        method: 'POST',
            //        url: 'Calendar.aspx/GetJobs',
            //        body: { from: '', to: '' },
            //        success: function (response) {
            //            // let { data, status, jqXHR } = response
            //            var evts = response.data;
            //            events = events.concat(evts);
            //        },
            //        error: function (err) {
            //            // let { jqXHR, status, error } = err
            //            // console.log('Error', response.jqXHR, response.status, response.error)
            //        },
            //    }),
            //    ajaxRequest({
            //        method: 'POST',
            //        url: 'Calendar.aspx/GetLeaves',
            //        body: { from: '', to: '' },
            //        success: function (response) {
            //            // let { data, status, jqXHR } = response
            //            var evts = response.data;
            //            events = events.concat(evts);
            //        },
            //        error: function (err) {
            //            // let { jqXHR, status, error } = err
            //            // console.log('Error', response.jqXHR, response.status, response.error)
            //        },
            //    })
            //).then(function () {
            //    generateCalendar(events);
            //});

        });

        function generateCalendar(evts) {
            var calendarEl = document.getElementById('calendar');

            var calendar = new FullCalendar.Calendar(calendarEl, {
                schedulerLicenseKey: 'CC-Attribution-NonCommercial-NoDerivatives',
                timeZone: 'Asia/Singapore',

                headerToolbar: {
                    left: 'prev,next',
                    center: 'title',
                    right: ''
                },

                initialView: 'dayGridMonth',
                views: {
                    dayGrid: {
                        dayMaxEventRows: 5, // adjust to 6 only for timeGridWeek/timeGridDay
                    },
                    month: {
                        displayEventEnd: true
                    }
                },

                events: evts,
                eventTimeFormat: { // like '14:30'
                    hour: '2-digit',
                    minute: '2-digit',
                    //second: '2-digit',
                    hour12: false,
                    meridiem: false,
                    omitZeroMinute: false
                },
                eventClick: function (eventClickInfo) {
                    eventClickInfo.jsEvent.preventDefault();
                    var win = window.open(eventClickInfo.event.url, '_blank');
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
                eventDidMount: function (evtInfo) {
                    //string job
                    //string vehicles
                    //string workers
                    var isHR = <%= role %> == <%= UserRoles.HR.GetHashCode() %>;
                    if (!evtInfo.event.extendedProps.jobId || !evtInfo.event.extendedProps.job) return;
                    var job = '<span><b>Job: </b>' + evtInfo.event.extendedProps.job + '</span>';
                    var vehicles = '<span><b>Vehicles: </b>' + evtInfo.event.extendedProps.vehicles + '</span>';
                    var workers = '<span><b>Staffs: </b>' + evtInfo.event.extendedProps.workers + '</span>';
                    var bgColor = 'bg-job-tooltip';
                    new Tooltip(evtInfo.el, {
                        html: true,
                        title: job + '<br /><br />' + vehicles + '<br /><br />' + workers + '<br /><br />' + (isHR ? '' : '<a target="_blank" class="btn blue" href="update-job?id=' + evtInfo.event.extendedProps.jobId + '"><i class="fa fa-upload"></i></a>'),
                        placement: 'top',
                        trigger: 'hover',
                        container: 'body',

                        template: '<div class="tooltip ' + bgColor + '" role="tooltip"><div class="tooltip-arrow"></div><div class="tooltip-inner" style="color: inherit"></div></div>',
                    });
                },
                dateClick: function (info) {
                    if (<%= role %> == <%= UserRoles.HR.GetHashCode() %>) return;
                    var win = window.open('create-job?date=' + info.dateStr, '_blank');
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
            });

            calendar.render();
        };


    </script>
</asp:Content>
