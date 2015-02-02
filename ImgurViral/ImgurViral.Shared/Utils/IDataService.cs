using ImgurViral.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImgurViral.Utils
{
    /// <summary>
    /// Interfaccia per definire l'accesso ai dati
    /// </summary>
    public interface IDataService
    {
        Task<List<GalleryImageData>> GetGalleryImage(Action<List<GalleryImageData>, Exception> callback);
        Task<List<AlbumImageCommentsData>> GetAlbumImageComments(Action<List<AlbumImageCommentsData>, Exception> callback, String id);
    }
}
