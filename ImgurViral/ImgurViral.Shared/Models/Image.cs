using System;
using System.Collections.Generic;
using System.Text;

namespace ImgurViral.Models
{
    /// <summary>
    /// Wrapper Model for Image data
    /// </summary>
    class Image
    {
        public IEnumerable<ImageData> Data { get; set; }
        public Boolean Status { get; set; }
        public Int32 Success { get; set; }
    }
}
