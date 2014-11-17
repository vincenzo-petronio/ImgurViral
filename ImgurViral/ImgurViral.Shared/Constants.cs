using System;
using System.Collections.Generic;
using System.Text;

namespace ImgurViral
{
    static class Constants
    {
        // API TOKEN
        public const String API_CLIENTID = "XXXXXXXXXXXXXXXX";
        public const String API_SECRET = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";

        // ENDPOINT
        private const String ENDPOINT_API_BASE = "https://api.imgur.com/3/";
        public const String ENDPOINT_API_GALLERY_VIRAL = ENDPOINT_API_BASE + "gallery/hot/viral/0.json";
    }
}
