using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Newtonsoft.Json;
using RestSharp;

namespace AvaChemAdminPanelMobiAPI.Common_File
{
    public static class Utils
    {
        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string HexConverter(System.Drawing.Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

        public static RgbColor RGBConverter(string hexValue)
        {
            System.Drawing.Color c = System.Drawing.ColorTranslator.FromHtml(hexValue);
            return new RgbColor
            {
                R = c.R,
                G = c.G,
                B = c.B
            };
        }
        public static string GetRedirectUrl(string path, int? message = null, object parameters = null)
        {
            string redirectUrl = path;
            if (parameters != null)
            {
                string p = Utils.GetQueryString(parameters);
                redirectUrl += (p + "" != "" ? $"?{p}" : "");
            }
            if (message + "" != "")
            {
                redirectUrl += (parameters != null ? "&" : "?") + "message=" + message;
            }

            return redirectUrl;
        }
        public static string GetQueryString(object obj)
        {
            var serilaizeJson = JsonConvert.SerializeObject(obj, Formatting.None, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            });
            var deserializeObject = JsonConvert.DeserializeObject<IDictionary<string, string>>(serilaizeJson);
            var result = deserializeObject.Select(x => HttpUtility.UrlEncode(x.Key) + "=" + HttpUtility.UrlEncode(x.Value));
            return string.Join("&", result);
        }
        public static string Hash(string input = "")
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            if (input + "" == "")
            {
                input = DateTime.Now.ToString() + input;
            }
            byte[] originalBytes = ASCIIEncoding.Default.GetBytes(input);
            byte[] encodedBytes = md5.ComputeHash(originalBytes);

            if (input + "" == "")
            {
                return BitConverter.ToString(encodedBytes).Replace("-", "").Replace("y", "e").Replace("r", "c");
            }
            else
            {
                return BitConverter.ToString(encodedBytes).Replace("-", "").Replace("r", "c").Replace("g", "h");

            }
        }
        public static void Log(CreateLogDTO createLogDTO, dynamic session = null)
        {
            if (HttpContext.Current.IsDebuggingEnabled && System.Diagnostics.Debugger.IsAttached) return;

            var userSession = session + "" != "" ? (session["admin"] as User) : null;
            new LogDataConnector().CreateLog(new Log()
            {
                ActionName = userSession + "" != "" ? $@"{(UserRoles)userSession.RoleID} ""{userSession.Name}"": {createLogDTO.ActionName}" : createLogDTO.ActionName,
                ActionLocation = createLogDTO.ActionLocation,
                ActionType = createLogDTO.ActionType,
                ActionMeta = createLogDTO.ActionMeta + "" != "" ? JsonConvert.SerializeObject(createLogDTO.ActionMeta) : "",
                ByUserID = createLogDTO.ByUserID != 0 ? createLogDTO.ByUserID : userSession + "" != "" ? userSession.ID : 0,
                CreatedDate = DateTime.Now,
                SoftDelete = false
            });
        }
        public static async Task<string> UploadFile(string prefix,
                                        HttpPostedFile postedFile,
                                        SupportedFileTypes type = SupportedFileTypes.All,
                                        // upto 10 MB
                                        int? maxContentLength = 1024 * 1024 * 10)
        {
            if (postedFile == null || postedFile.ContentLength <= 0) throw new ArgumentNullException();

            string extension = System.IO.Path.GetExtension(postedFile.FileName).ToLower();
            string fileName = prefix + "-" + DateTimeOffset.Now.ToUnixTimeMilliseconds() + System.Guid.NewGuid().ToString().Replace("-", "") + extension;

            List<string> allowedFileExtensions = null;

            switch (type)
            {
                case SupportedFileTypes.Image:
                    allowedFileExtensions = new List<string> { ".jpeg", ".jpg", ".png" };
                    break;
                default:
                    break;
            }
            if (allowedFileExtensions != null && !allowedFileExtensions.Contains(extension))
            {
                switch (type)
                {
                    case SupportedFileTypes.Image:
                        throw new ArgumentException(@"Please upload image of types "".jpeg"", "".jpg"", "".png""");
                    default:
                        throw new ArgumentException(@"Please upload the valid type of file");
                }
            }
            else if (postedFile.ContentLength > maxContentLength)
            {
                throw new ArgumentException($"Please upload a file upto {maxContentLength} MB");
            }
            else
            {
                Uri serviceUri = new Uri("https://corsivacdncontent.blob.core.windows.net/");
                StorageSharedKeyCredential credential = new StorageSharedKeyCredential(
                            "corsivacdncontent",
                            "b6ANjJuW0M/DVk4/EPyarz2yhFCnnY4wmPremMMQGf5h58VcA6U5LchCazdqBiAuOwKDtInclYorwyo1dKUqog=="
                );
                BlobServiceClient blobServiceClient = new BlobServiceClient(serviceUri, credential);
                BlobContainerClient container = blobServiceClient.GetBlobContainerClient("avachem");
                //await container.CreateIfNotExistsAsync();

                BlobClient blob = container.GetBlobClient(fileName);
                await blob.UploadAsync(postedFile.InputStream,
                    new BlobHttpHeaders()
                    {
                        ContentType = postedFile.ContentType
                    }
                );

                return blob.Uri.AbsoluteUri;
            }
        }
        public static async Task<bool> PushNotification(FCMNotificationRequest notifReq)
        {
            string FCM_BASE_URL = "https://fcm.googleapis.com/fcm/send";
            string SERVER_KEY = "AAAARak-30I:APA91bF98cbuUM51fAw3rsaBYD1v5LtqGEn_p7U_34OmU4prt3IsMLIeLi4v-GbIrJWuTzpIQQ9Sb--r4yeQv7zlBDh4MZ5PxlRxyH3cz-PsnIVf0umhAIbx62-52IcKakUNb-uiMOsp";
            int TIMEOUT = 300000; // ms

            // RestClient
            var client = new RestClient(FCM_BASE_URL)
            {
                Encoding = Encoding.UTF8,
                Timeout = TIMEOUT
            };

            // Request
            var request = new RestRequest(Method.POST) { RequestFormat = DataFormat.Json };
            request.AddHeader("Authorization", $"key={SERVER_KEY}");
            request.AddHeader("Content-Type", "application/json");

            // Body
            var bodyString = JsonConvert.SerializeObject(notifReq, Formatting.None, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore
            });
            request.AddJsonBody(bodyString);

            // Response
            var response = await client.ExecuteAsync(request);

            if (response.StatusCode.ToString() != "OK") return false;
            var result = JsonConvert.DeserializeObject<FCMNotificationResponse>(response.Content);
            return result.success > 0 && result.failure == 0;

        }
        public static async Task PushNotif(int uid, FCMNotificationBody notif = null, FCMDataBody data = null)
        {
            DeviceDataConnector deviceDataConnector = new DeviceDataConnector();
            List<Device> devices = deviceDataConnector.GetAll(uid);
            if (devices.Count == 0) return;

            int taskBatchSize = 3; // 3 devices in once
            int taskBatchCount = (int)Math.Ceiling((double)devices.Count() / taskBatchSize);
            for (int i = 0; i < taskBatchCount; i++)
            {
                var devicesToPush = devices.Skip(i * taskBatchSize).Take(taskBatchSize);
                var pushTasks = devicesToPush.Select(async dev =>
                {
                    FCMNotificationRequest notifReq = new FCMNotificationRequest
                    {
                        to = dev.FCMToken,
                    };
                    if (notif != null) // keep it null if pushing in silence
                    {
                        notifReq.notification = notif;
                    }
                    if (data != null)
                    {
                        notifReq.content_available = true;
                        notifReq.data = data;
                    }

                    bool isPushed = await Utils.PushNotification(notifReq);

                    if (isPushed == false)
                    {
                        deviceDataConnector.UpdateNumFailedNotif(dev.ID, 1, UpdateNumFailedNotifType.Increase);
                    }
                    else if (dev.NumFailedNotif > 0)
                    {
                        deviceDataConnector.UpdateNumFailedNotif(dev.ID, 0, UpdateNumFailedNotifType.Overwrite);
                    }
                });

                try
                {
                    await Task.WhenAll(pushTasks);
                }
                catch (Exception)
                {
                    continue;
                }

            }

        }
        public static async Task<bool> PushNotifToAll(FCMNotificationBody notif)
        {
            string TOPIC = "avachem-main-topic";

            FCMNotificationRequest notifReq = new FCMNotificationRequest
            {
                to = $"/topics/{TOPIC}",
                notification = notif
            };
            bool isPushed = await Utils.PushNotification(notifReq);
            return isPushed;
        }


        private static string SmtpHost = "ava-chem.com";
        private static string SmtpUserName = "ot@ava-chem.com";
        private static string SmtpPassword = "Avachem123";
        //public static string SendResetPasswordMail(string email)
        //{
        //    try
        //    {
        //        string newPassword = Utils.RandomString(12);
        //        MailMessage mm = new MailMessage();
        //        mm.From = new MailAddress("ot@ava-chem.com", "Ava-Chem");
        //        mm.To.Add(email);

        //        mm.Subject = "Ava-Chem | New Password";
        //        mm.Body = @"<p>Your password has been reset</p>"
        //                + $@"<p>Your new password is:<br/><b>{newPassword}</b></p>"
        //                + @"<p>Thank you!<br/>Warm regards,</p>";
        //        mm.IsBodyHtml = true;


        //        SmtpClient smtp = new SmtpClient();
        //        smtp.Host = Utils.SmtpHost;
        //        smtp.EnableSsl = true;
        //        NetworkCredential NetworkCred = new NetworkCredential();
        //        NetworkCred.UserName = Utils.SmtpUserName;
        //        NetworkCred.Password = Utils.SmtpPassword;
        //        smtp.UseDefaultCredentials = false;
        //        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        //        smtp.Credentials = NetworkCred;
        //        smtp.Port = 587;
        //        smtp.Send(mm);

        //        return newPassword;
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}
        public static void SendApplyOTMail(string staffName)
        {
            try
            {
                string destination = "joanne@corsivalab.com";
                MailMessage mm = new MailMessage();
                mm.From = new MailAddress("ot@ava-chem.com", "Ava-Chem");
                mm.To.Add(destination);
                mm.CC.Add("triet@corsivalab.space");

                mm.Subject = "Ava-Chem | Apply OT";
                mm.Body = $@"<p>""{staffName}"" applies for OT</p>"
                        + @"<p>Thank you!<br/>Warm regards,</p>";
                mm.IsBodyHtml = true;


                SmtpClient smtp = new SmtpClient();
                smtp.Host = Utils.SmtpHost;
                smtp.EnableSsl = true;
                NetworkCredential NetworkCred = new NetworkCredential();
                NetworkCred.UserName = Utils.SmtpUserName;
                NetworkCred.Password = Utils.SmtpPassword;
                smtp.UseDefaultCredentials = false;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send(mm);
            }
            catch (Exception)
            {
            }
        }
    }


    public class RgbColor
    {
        public int R { get; set; }
        public int G { get; set; }
        public int B { get; set; }
    }

    public class FCMNotificationRequest
    {
        public string to { get; set; }
        public FCMNotificationBody notification { get; set; }
        public bool content_available { get; set; } = true;
        public FCMDataBody data { get; set; }
    }
    public class FCMNotificationBody
    {
        public int badge { get; set; } = 0;
        public string sound { get; set; } = "default";
        public string android_channel_id { get; set; } = "avachem-default-channel";
        public string title { get; set; }
        public string body { get; set; }
    }
    public class FCMDataBody
    {
        public string code; // FCMDataCodes
        public int jobId { get; set; }
    }
    public class FCMDataCodes
    {
        public const string REFRESH_CREDITS = "REFRESH_CREDITS";
        public const string UPDATE_JOB = "UPDATE_JOB";
    }

    public class FCMNotificationResponse
    {
        public long multicast_id { get; set; }
        public int success { get; set; }
        public int failure { get; set; }
        public long canonical_ids { get; set; }
        public List<FCMNotificationResponseResult> results { get; set; }
    }
    public class FCMNotificationResponseResult
    {
        public string error { get; set; }
    }

    public class ImageUploadDTO
    {
        public string FileKey { get; set; }
        public string FileName { get; set; }
        public string Url { get; set; }
    }

    public enum DataActionTypes
    {
        Create = 1,
        Update,
        Hide,
        Show,
    }
    public enum SupportedFileTypes
    {
        All = 1,
        Image,
    }
}
