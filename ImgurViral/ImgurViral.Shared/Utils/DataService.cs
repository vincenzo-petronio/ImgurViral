using ImgurViral.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Linq;
using Windows.Storage;
using System.IO;
using System.Diagnostics;

namespace ImgurViral.Utils
{
    public class DataService : IDataService
    {
        public async Task<List<GalleryImageData>> getGalleryImage(Action<List<GalleryImageData>, Exception> callback)
        {
            Exception exception = null;
            List<GalleryImageData> results = new List<GalleryImageData>();
            String uri = Constants.ENDPOINT_API_GALLERY_VIRAL;

            var response = await this.DownloadData(uri);
            System.Diagnostics.Debug.WriteLine("[URI]\t{0}\n[RESPONSE]{1}", uri, response);
            try
            {
                GalleryImage responseDeserialized = JsonConvert.DeserializeObject<GalleryImage>(response);
                // Filtro gli item che non sono visualizzabili, esempio video o album.
                var responseDeserializedRestricted = from item in responseDeserialized.Data where item.IsAlbum == false select item;
                foreach (var d in responseDeserializedRestricted)
                {
                    results.Add(d);
                }
            } 
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[DataService.getGalleryImage] \n" + e.ToString());
            }

            callback(results, exception);

            return null;
        }

        /// <summary>
        /// Get data from url
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        private async Task<String> DownloadData(String uri)
        {
            var response = String.Empty;
            var httpClient = new HttpClient();
            try
            {
                String accessToken = await GetAccessToken();

                if (accessToken != String.Empty)
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", "Bearer " + accessToken);
                    response = await httpClient.GetStringAsync(new Uri(uri));
                }
                else
                {
                    Debug.WriteLine("[DataService.DownloadData]\t" + "NO ACCESS TOKEN FROM LOCAL!");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[DataService.DownloadData]\n" + ex.ToString());
            }
            
            return response;
        }

        private async Task<String> GetAccessToken()
        {
            String accessToken = String.Empty;

            // Read local for Access Token
            StorageFolder sFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile sFile = await sFolder.CreateFileAsync(Constants.AUTH_LOCALSETTINGS_FILENAME, CreationCollisionOption.OpenIfExists);
            string sFileContent = null;
            using (var sFileReader = new StreamReader(await sFile.OpenStreamForReadAsync()))
            {
                sFileContent = await sFileReader.ReadToEndAsync();
            }

            if (sFileContent != null)
            {
                AuthUser jsonToAuthUser = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<AuthUser>(sFileContent));
                Debug.WriteLine("[DataService.GetAccessToken]\tAccessToken: " + jsonToAuthUser.AccessToken);
                accessToken = jsonToAuthUser.AccessToken;
            }

            return accessToken;
        }
    }
}
