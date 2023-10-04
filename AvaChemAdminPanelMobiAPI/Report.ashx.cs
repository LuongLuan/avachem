using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using AvaChemAdminPanelMobiAPI.Common_File;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Newtonsoft.Json;

namespace AvaChemAdminPanelMobiAPI
{
    /// <summary>
    /// Summary description for Report
    /// </summary>
    public class Report : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            var Session = context.Session;
            var Request = context.Request;
            var Response = context.Response;

            // add this to everypage so User have to login to access to data/webpage
            // Session["admin"] the "admin" inside is a container to link to other pages
            // it's not connected to the class and dataconnector
            if (Session["admin"] == null) { Response.Redirect(Routes.LOG_IN); }

            // when update and value is null add if and else statement
            string reportName = Request.RequestContext.RouteData.Values["ReportName"] as string;
            if (reportName == null)
            {
                Response.Redirect(Routes.JOBS, false);
                return;
            }

            Job thisJob = new JobDataConnector().GetByReportName(reportName);

            byte[] pdfBuffer = this.GeneratePDF(thisJob);

            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", $"inline; filename={thisJob.ReportName}.pdf"); // for downloading: $"attachment; filename={fileName}"
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.BinaryWrite(pdfBuffer);
            Response.Flush();
            Response.End();
        }

        private byte[] GeneratePDF(Job thisJob)
        {
            Client client = new ClientDataConnector().GetClientByFirstVariable(thisJob.ClientID);
            List<UserWithUserJobDTO> listWorkers = new UserJobDataConnector().GetUsersByJobID(thisJob.ID).ToList<UserWithUserJobDTO>();
            List<Vehicle> listV = new JobVehicleDataConnector().GetVehiclesByParams(thisJob.ID).ToList<Vehicle>();

            List<TripDetailsDTO> listT = new List<TripDetailsDTO>();
            int tripIndex = 0;
            foreach (var trip in new TripDataConnector().GetByJobID(thisJob.ID).ToList())
            {
                tripIndex++;

                var rawTrip = JsonConvert.SerializeObject(trip);
                var tripDetailsDTO = JsonConvert.DeserializeObject<TripDetailsDTO>(rawTrip);

                tripDetailsDTO.Index = tripIndex;

                tripDetailsDTO.BeforeImages = new List<JobImage>();
                tripDetailsDTO.AfterImages = new List<JobImage>();
                foreach (JobImage img in new JobImageDataConnector().GetByTripID(trip.ID).ToList<JobImage>())
                {
                    switch (img.Type)
                    {
                        case "before":
                            tripDetailsDTO.BeforeImages.Add(img);
                            break;
                        case "after":
                            tripDetailsDTO.AfterImages.Add(img);
                            break;
                        default:
                            break;
                    }
                }
                listT.Add(tripDetailsDTO);
            }

            byte[] buffer = null;
            using (MemoryStream memStream = new MemoryStream())
            {
                using (PdfWriter pdfWriter = new PdfWriter(memStream))
                {
                    pdfWriter.SetCloseStream(true);
                    using (PdfDocument pdfDocument = new PdfDocument(pdfWriter))
                    {
                        pdfDocument.SetDefaultPageSize(PageSize.A4);
                        using (Document document = new Document(pdfDocument))
                        {
                            // Font and style
                            string rootPath = System.IO.Path.Combine(HttpContext.Current.Server.MapPath("~/."), "assets/pages/fonts/report/");
                            PdfFont normalFont = PdfFontFactory.CreateFont(rootPath + "OpenSans-Regular.ttf");
                            PdfFont semiBoldFont = PdfFontFactory.CreateFont(rootPath + "OpenSans-SemiBold.ttf");
                            PdfFont boldFont = PdfFontFactory.CreateFont(rootPath + "OpenSans-Bold.ttf");

                            Style headerStyle = new Style()
                                // Font
                                .SetFont(boldFont)
                                // Size
                                .SetFontSize(26)
                                //// Bold
                                //.SetTextRenderingMode(PdfCanvasConstants.TextRenderingMode.FILL_STROKE)
                                //.SetStrokeWidth(0.5f)
                                // Color
                                .SetStrokeColor(this.RgbToDeviceRgb("#666666"));
                            Style subHeaderStyle = new Style()
                                // Font
                                .SetFont(semiBoldFont)
                                // Size
                                .SetFontSize(14)
                                // Color
                                .SetStrokeColor(this.RgbToDeviceRgb("#333333"));
                            Style normalStyle = new Style()
                                // Font
                                .SetFont(normalFont)
                                // Size
                                .SetFontSize(12)
                                // Color
                                .SetStrokeColor(this.RgbToDeviceRgb("#5d5d5d"));
                            Style boldStyle = new Style()
                                // Font
                                .SetFont(semiBoldFont)
                                // Size
                                .SetFontSize(12)
                                // Color
                                .SetStrokeColor(this.RgbToDeviceRgb("#5d5d5d"));


                            //// New line
                            Paragraph newline = new Paragraph(new Text("\n"));
                            //document.Add(newline);

                            // Header - Report....
                            string reportName = thisJob.ReportName;
                            //string jobNumber = thisJob.JobNumber;

                            Paragraph header = new Paragraph($"Report {reportName}").AddStyle(headerStyle);
                            document.Add(header);


                            // Line break
                            SolidLine solidLine = new SolidLine(0.5f);
                            solidLine.SetColor(this.RgbToDeviceRgb("#eee"));
                            document.Add(new LineSeparator(solidLine));


                            // JobDetails table
                            float[] columnWidths = { 6, 4 };
                            Table tblJobDetails = new Table(columnWidths, false)
                                .SetKeepTogether(true)
                                .SetWidth(UnitValue.CreatePercentValue(100))
                                .SetFixedLayout();

                            Cell cellJD1full = new Cell(1, 2).SetPadding(8).SetBackgroundColor(this.RgbToDeviceRgb("#f3f4f6"))
                               .Add(new Paragraph("Job Details").AddStyle(subHeaderStyle));
                            tblJobDetails.AddCell(cellJD1full);

                            string jobName = thisJob.Name;
                            string workingDate = thisJob.WorkingDate.ToString("dd MMM yyyy");
                            string location = thisJob.Location;
                            string description = string.Join(", ", new JobDescriptionDataConnector().GetByJobID(thisJob.ID).Select(jd => jd.Content).ToArray());
                            string adminRemarks = thisJob.AdminRemarks;

                            Paragraph cellJD21P = new Paragraph()
                                        .Add(new Text("Name").AddStyle(boldStyle))
                                        .Add(new Text($"\n{jobName}\n\n").AddStyle(normalStyle))

                                        .Add(new Text("Date: ").AddStyle(boldStyle))
                                        .Add(new Text($"{workingDate}\n").AddStyle(normalStyle))

                                        .Add(new Text("Location: ").AddStyle(boldStyle))
                                        .Add(new Text($"{location}\n").AddStyle(normalStyle))

                                        .Add(new Text("Description: ").AddStyle(boldStyle))
                                        .Add(new Text($"{description}\n").AddStyle(normalStyle));
                            if (adminRemarks + "" != "")
                            {
                                cellJD21P.Add(new Text("Remarks: ").AddStyle(boldStyle))
                                        .Add(new Text($"{adminRemarks}\n").AddStyle(normalStyle));
                            }
                            Cell cellJD21 = new Cell(1, 1).SetPadding(8).Add(cellJD21P);


                            string dateAdded = thisJob.CreatedDate.ToString("dd MMM yyyy");
                            string invoiceNo = thisJob.InvoiceNo;

                            Cell cellJD22 = new Cell(1, 1).SetPadding(8).SetVerticalAlignment(VerticalAlignment.BOTTOM)
                               .Add(new Paragraph()
                                        .Add(new Text("Date Added: ").AddStyle(boldStyle))
                                        .Add(new Text($"{dateAdded}\n").AddStyle(normalStyle))

                                        .Add(new Text("Invoice No. ").AddStyle(boldStyle))
                                        .Add(new Text($"{invoiceNo}\n").AddStyle(normalStyle)) // Note

                                   //.Add(new Text("Service Memo / Delivery Order Number: ").AddStyle(boldStyle))
                                   //.Add(new Text($"#{jobNumber}\n").AddStyle(normalStyle))
                                   );

                            tblJobDetails.AddCell(cellJD21);
                            tblJobDetails.AddCell(cellJD22);

                            document.Add(newline);
                            document.Add(tblJobDetails);

                            // Client table
                            Table tblClient = new Table(2, false)
                                .SetKeepTogether(true)
                                .SetWidth(UnitValue.CreatePercentValue(100));

                            Cell cellCl1full = new Cell(1, 2).SetPadding(8).SetBackgroundColor(this.RgbToDeviceRgb("#f3f4f6"))
                               .Add(new Paragraph("Client").AddStyle(subHeaderStyle));
                            tblClient.AddCell(cellCl1full);

                            string company = $"{client.CompanyName}, {client.Location}";
                            string primaryContactName = client.ContactNamePrimary;
                            string primaryContactDetails = client.ContactDetailsPrimary;

                            Cell cellCl21 = new Cell(1, 1).SetPadding(8)
                               .Add(new Paragraph()
                                        .Add(new Text("Company").AddStyle(boldStyle))
                                        .Add(new Text($"\n{company}\n\n").AddStyle(normalStyle))

                                        .Add(new Text("Primary Contact Name: ").AddStyle(boldStyle))
                                        .Add(new Text($"{primaryContactName}\n").AddStyle(normalStyle))

                                        .Add(new Text("Primary Contact Details: ").AddStyle(boldStyle))
                                        .Add(new Text($"{primaryContactDetails}\n").AddStyle(normalStyle))
                                   );

                            string secondaryContactName = client.ContactNameSecondary;
                            string secondaryContactDetails = client.ContactDetailsSecondary;

                            Cell cellCl22 = new Cell(1, 1).SetPadding(8).SetVerticalAlignment(VerticalAlignment.BOTTOM)
                               .Add(new Paragraph()
                                        .Add(new Text("Secondary Contact Name: ").AddStyle(boldStyle))
                                        .Add(new Text($"{secondaryContactName}\n").AddStyle(normalStyle))

                                        .Add(new Text("Secondary Contact Details: ").AddStyle(boldStyle))
                                        .Add(new Text($"{secondaryContactDetails}\n").AddStyle(normalStyle))
                                   );

                            tblClient.AddCell(cellCl21);
                            tblClient.AddCell(cellCl22);

                            document.Add(newline);
                            document.Add(tblClient);


                            // Vehicles table
                            Table tblVehicles = new Table(2, false)
                                .SetKeepTogether(true)
                                .SetWidth(UnitValue.CreatePercentValue(100));

                            Cell cellV1full = new Cell(1, 2).SetPadding(8).SetBackgroundColor(this.RgbToDeviceRgb("#f3f4f6"))
                               .Add(new Paragraph("Vehicles").AddStyle(subHeaderStyle));
                            tblVehicles.AddCell(cellV1full);

                            Cell cellV21 = new Cell(1, 1).SetPadding(8)
                               .Add(new Paragraph("Number").AddStyle(boldStyle));

                            Cell cellV22 = new Cell(1, 1).SetPadding(8)
                               .Add(new Paragraph("Model").AddStyle(boldStyle));

                            tblVehicles.AddCell(cellV21);
                            tblVehicles.AddCell(cellV22);

                            // Map data
                            for (int i = 0; i < listV.Count; i++)
                            {
                                Vehicle v = listV[i];

                                Cell cellVi1 = new Cell(1, 1).SetPadding(8)
                                .Add(new Paragraph(v.Number).AddStyle(normalStyle));
                                Cell cellVi2 = new Cell(1, 1).SetPadding(8)
                                    .Add(new Paragraph(v.Model).AddStyle(normalStyle));

                                tblVehicles.AddCell(cellVi1);
                                tblVehicles.AddCell(cellVi2);
                            }

                            document.Add(newline);
                            document.Add(tblVehicles);


                            // Workers table
                            Table tblWorkers = new Table(5, false)
                                .SetKeepTogether(true)
                                .SetWidth(UnitValue.CreatePercentValue(100));

                            Cell cellW1full = new Cell(1, 5).SetPadding(8).SetBackgroundColor(this.RgbToDeviceRgb("#f3f4f6"))
                               .Add(new Paragraph("Staffs").AddStyle(subHeaderStyle));
                            tblWorkers.AddCell(cellW1full);

                            Cell cellW21 = new Cell(1, 1).SetPadding(8)
                               .Add(new Paragraph("Name").AddStyle(boldStyle));
                            Cell cellW22 = new Cell(1, 1).SetPadding(8)
                               .Add(new Paragraph("ID Number").AddStyle(boldStyle));
                            Cell cellW23 = new Cell(1, 1).SetPadding(8)
                               .Add(new Paragraph("Phone").AddStyle(boldStyle));
                            Cell cellW24 = new Cell(1, 1).SetPadding(8)
                               .Add(new Paragraph("Email").AddStyle(boldStyle));
                            Cell cellW25 = new Cell(1, 1).SetPadding(8).SetTextAlignment(TextAlignment.CENTER)
                               .Add(new Paragraph("Role").AddStyle(boldStyle));

                            tblWorkers.AddCell(cellW21);
                            tblWorkers.AddCell(cellW22);
                            tblWorkers.AddCell(cellW23);
                            tblWorkers.AddCell(cellW24);
                            tblWorkers.AddCell(cellW25);

                            // Map data
                            for (int i = 0; i < listWorkers.Count; i++)
                            {
                                UserWithUserJobDTO w = listWorkers[i];

                                Cell cellWi1 = new Cell(1, 1).SetPadding(8)
                                    .Add(new Paragraph(w.Name).AddStyle(normalStyle));
                                Cell cellWi2 = new Cell(1, 1).SetPadding(8)
                                    .Add(new Paragraph(w.IDNumber).AddStyle(normalStyle));
                                Cell cellWi3 = new Cell(1, 1).SetPadding(8)
                                    .Add(new Paragraph(w.Phone).AddStyle(normalStyle));
                                Cell cellWi4 = new Cell(1, 1).SetPadding(8)
                                    .Add(new Paragraph(w.Email).AddStyle(normalStyle));
                                Cell cellWi5 = new Cell(1, 1).SetPadding(8).SetTextAlignment(TextAlignment.CENTER)
                                    .Add(new Paragraph(((UserRoles)w.RoleID).ToString()).AddStyle(normalStyle));

                                tblWorkers.AddCell(cellWi1);
                                tblWorkers.AddCell(cellWi2);
                                tblWorkers.AddCell(cellWi3);
                                tblWorkers.AddCell(cellWi4);
                                tblWorkers.AddCell(cellWi5);
                            }

                            document.Add(newline);
                            document.Add(tblWorkers);


                            // Workers table


                            foreach (TripDetailsDTO trip in listT)
                            {
                                Table tblTrips = new Table(1, false)
                                    .SetKeepTogether(true)
                                    .SetWidth(UnitValue.CreatePercentValue(100));
                                Cell cellTfull = new Cell(1, 1).SetPadding(8).SetBackgroundColor(this.RgbToDeviceRgb("#f3f4f6"))
                                    .Add(new Paragraph($"Trip {trip.Index}").AddStyle(subHeaderStyle));
                                tblTrips.AddCell(cellTfull);

                                string jobPeriodHours = $"{trip.StartTime.ToString("h:mm tt")} - {trip.EndTime.ToString("h:mm tt")}";
                                string workerStartedTime = trip.WorkerStartedTime.HasValue ? ((DateTime)trip.WorkerStartedTime).ToString("h:mm tt") : "";
                                string workerEndedTime = trip.WorkerEndedTime.HasValue ? ((DateTime)trip.WorkerEndedTime).ToString("h:mm tt") : "";
                                string loggedPeriodHours = $"{workerStartedTime} - {workerEndedTime}";

                                Paragraph tripContent = new Paragraph();
                                tripContent.Add(new Text("Job's Period Hours: ").AddStyle(boldStyle))
                                           .Add(new Text($"{jobPeriodHours}\n").AddStyle(normalStyle))

                                           .Add(new Text("Logged Period Hours: ").AddStyle(boldStyle))
                                           .Add(new Text($"{loggedPeriodHours}\n").AddStyle(normalStyle));

                                string jobNumber = trip.JobNumber;
                                tripContent.Add(new Text("Service Memo / Delivery Order Number: ").AddStyle(boldStyle))
                                                .Add(new Text($"#{jobNumber}\n").AddStyle(normalStyle));

                                string driverRemarks = trip.Remarks;
                                if (driverRemarks + "" != "")
                                {
                                    tripContent.Add(new Text("Driver's Remarks: ").AddStyle(boldStyle))
                                               .Add(new Text($"{driverRemarks}\n").AddStyle(normalStyle));
                                }

                                tripContent.Add(new Text("Before Images:\n").AddStyle(boldStyle));
                                Paragraph beforeImgContainerfull = new Paragraph();
                                // Map data
                                for (int i = 0; i < trip.BeforeImages.Count; i++)
                                {
                                    string beforeUri = trip.BeforeImages[i].ImageUrl;

                                    Image beforeImg = new Image(ImageDataFactory
                                                .Create(new Uri(beforeUri)))
                                                .SetAutoScale(true)
                                                .SetTextAlignment(TextAlignment.CENTER)
                                                .SetHorizontalAlignment(HorizontalAlignment.CENTER);
                                    Div divBeforeImg = new Div()
                                                    .Add(beforeImg)
                                                    .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                                                    .SetWidth(100)
                                                    .SetHeight(100)
                                                    .SetBorder(new DashedBorder(1))
                                                    .SetMarginRight(10).SetMarginBottom(12)
                                                    .SetAction(PdfAction.CreateURI(beforeUri));

                                    beforeImgContainerfull.Add(divBeforeImg);
                                }
                                tripContent.Add(beforeImgContainerfull);
                                tripContent.Add(new Text("\n").AddStyle(boldStyle));

                                tripContent.Add(new Text("After Images:\n").AddStyle(boldStyle));
                                Paragraph afterImgContainerfull = new Paragraph();
                                // Map data
                                for (int i = 0; i < trip.AfterImages.Count; i++)
                                {
                                    string afterUri = trip.AfterImages[i].ImageUrl;

                                    Image afterImg = new Image(ImageDataFactory
                                                .Create(new Uri(afterUri)))
                                                .SetAutoScale(true)
                                                .SetTextAlignment(TextAlignment.CENTER)
                                                .SetHorizontalAlignment(HorizontalAlignment.CENTER);
                                    Div divAfterImg = new Div()
                                                    .Add(afterImg)
                                                    .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                                                    .SetWidth(100)
                                                    .SetHeight(100)
                                                    .SetBorder(new DashedBorder(1))
                                                    .SetMarginRight(10).SetMarginBottom(12)
                                                    .SetAction(PdfAction.CreateURI(afterUri));

                                    afterImgContainerfull.Add(divAfterImg);
                                }
                                tripContent.Add(afterImgContainerfull);
                                tripContent.Add(new Text("\n").AddStyle(boldStyle));

                                tripContent.Add(new Text("Customer Signature:\n").AddStyle(boldStyle));
                                PageSize pageSize = document.GetPdfDocument().GetDefaultPageSize();
                                float pageWidth = pageSize.GetWidth() - document.GetLeftMargin() - document.GetRightMargin();
                                string signatureUri = trip.CustomerSignatureImage;
                                tripContent.Add(new Paragraph()
                                            .SetWidth(UnitValue.CreatePercentValue(100))
                                            .Add(
                                                new Image(ImageDataFactory
                                                    .Create(new Uri(signatureUri)))
                                                    .ScaleToFit((float)(pageWidth / 1.5), 500)
                                                    .SetTextAlignment(TextAlignment.CENTER)
                                                    .SetHorizontalAlignment(HorizontalAlignment.CENTER)
                                                )
                                            .SetBorder(new DashedBorder(1))
                                            .SetTextAlignment(TextAlignment.CENTER)
                                            .SetHorizontalAlignment(HorizontalAlignment.CENTER)
                                            .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                                            .SetAction(PdfAction.CreateURI(signatureUri)));
                                tripContent.Add(new Text("\n").AddStyle(boldStyle));

                                Cell cellTripfull = new Cell(1, 1).SetPadding(8).Add(tripContent);
                                tblTrips.AddCell(cellTripfull);

                                document.Add(newline);
                                document.Add(tblTrips);
                            }

                            // Close document
                            document.Close();
                        }
                        // Close PDF doc
                        pdfDocument.Close();

                    }
                }
                buffer = memStream.ToArray();
            }

            return buffer;
        }

        private DeviceRgb RgbToDeviceRgb(string hexValue)
        {
            RgbColor rgb = Utils.RGBConverter(hexValue);
            return new DeviceRgb(rgb.R, rgb.G, rgb.B);
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}