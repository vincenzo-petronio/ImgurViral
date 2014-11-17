using System;
using System.Collections.Generic;
using System.Text;

namespace ImgurViral.Models
{
    /// <summary>
    /// Wrapper Model for Gallery Album data
    /// </summary>
    class GalleryAlbum
    {
        public IEnumerable<GalleryAlbumData> Data { get; set; }
        public Boolean Status { get; set; }
        public Int32 Success { get; set; }
    }
}
