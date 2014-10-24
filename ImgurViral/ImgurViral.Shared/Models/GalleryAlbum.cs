using System;
using System.Collections.Generic;
using System.Text;

namespace ImgurViral.Models
{
    /// <summary>
    /// Model for Gallery Album API Endpoint http://api.imgur.com/3/gallery/album/{id}
    /// </summary>
    class GalleryAlbum
    {
        public String Id { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public Int32 DateTime { get; set; }
        public String Cover { get; set; }
        public Int32 CoverWidth { get; set; }
        public Int32 CoverHeight { get; set; }
        public String AccountUrl { get; set; }
        public Int32 AccountId { get; set; }
        public String Privacy { get; set; }
        public String Layout { get; set; }
        public Int32 Views { get; set; }
        public String Link { get; set; }
        public Int32 Ups { get; set; }
        public Int32 Downs { get; set; }
        public Int32 Score { get; set; }
        public Boolean IsAlbum { get; set; }
        private String Vote { get; set; }
        public Boolean Favorite { get; set; }
        public Boolean Nsfw { get; set; }
        public Int32 ImagesCount { get; set; }
        public List<Image> Images { get; set; }
    }
}
