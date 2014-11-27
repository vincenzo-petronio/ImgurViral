using System;
using System.Collections.Generic;
using System.Text;

namespace ImgurViral
{
    static class Constants
    {
        // API TOKEN
        public const String API_CLIENTID = "";
        public const String API_SECRET = "";
        public const String AUTH_LOCALSETTINGS_FILENAME = "AUTH_LOCAL";

        // ENDPOINT
        private const String ENDPOINT_API_BASE = "https://api.imgur.com/3/";
        public const String ENDPOINT_API_AUTHORIZE = "https://api.imgur.com/oauth2/authorize?client_id={0}&response_type=token";
        public const String ENDPOINT_API_GALLERY_VIRAL = ENDPOINT_API_BASE + "gallery/hot/viral/0.json";
    }
}
