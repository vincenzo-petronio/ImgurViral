using System;
using System.Collections.Generic;
using System.Text;

namespace ImgurViral.Models
{
    /// <summary>
    /// Model for Authenticated User.
    /// </summary>
    public class AuthUser
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string Username { get; set; }
        public string ExpiresToken { get; set; }
        public string TypeToken { get; set; }
    }
}
