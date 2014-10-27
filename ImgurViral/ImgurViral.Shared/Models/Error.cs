using System;
using System.Collections.Generic;
using System.Text;

namespace ImgurViral.Models
{
    /// <summary>
    /// Model for API error 
    /// </summary>
    class Error
    {
        public String Error { get; set; }
        public String Request { get; set; }
        public String Method { get; set; }
        public Boolean Success { get; set; }
        public Int32 Status { get; set; }
    }
}
