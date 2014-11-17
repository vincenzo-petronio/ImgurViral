using System;
using System.Collections.Generic;
using System.Text;

namespace ImgurViral.Models
{
    /// <summary>
    /// Wrapper Model for Gallery Image data
    /// </summary>
    public class GalleryImage
    {
        public IEnumerable<GalleryImageData> Data { get; set; }
        public Boolean Status { get; set; }
        public Int32 Success { get; set; }
    }
}
