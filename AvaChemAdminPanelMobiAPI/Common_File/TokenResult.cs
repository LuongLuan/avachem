﻿namespace AvaChemAdminPanelMobiAPI.Common_File
{
    public class TokenResult
    {
        public string scope { get; set; }
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string app_id { get; set; }
        public string expires_in { get; set; }
    }
}