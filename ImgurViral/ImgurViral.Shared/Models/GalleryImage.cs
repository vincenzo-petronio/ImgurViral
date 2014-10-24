using System;
using System.Collections.Generic;
using System.Text;

namespace ImgurViral.Models
{
    /// <summary>
    /// Model for Gallery Image API Endpoint https://api.imgur.com/3/gallery/image/{id}
    /// </summary>
    class GalleryImage
    {
        public String Id { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public Int32 DateTime { get; set; }
        public String Type { get; set; }
        public Boolean Animated { get; set; }
        public Int32 Width { get; set; }
        public Int32 Height { get; set; }
        public Int32 Size { get; set; }
        public Int32 Views { get; set; }
        public Int32 Bandwidth { get; set; }
        public String DeleteHash { get; set; }
        public String Link { get; set; }
        public String Gifv { get; set; }
        public String Mp4 { get; set; }
        public String Webm { get; set; }
        public String Looping { get; set; }
        private String Vote { get; set; }
        public Boolean Favorite { get; set; }
        public Boolean Nsfw { get; set; }
        public String Section { get; set; }
        public String AccountUrl { get; set; }
        public Int32 AccountId { get; set; }
        public Int32 Ups { get; set; }
        public Int32 Downs { get; set; }
        public Int32 Score { get; set; }
        public Boolean IsAlbum { get; set; }
    }
}
