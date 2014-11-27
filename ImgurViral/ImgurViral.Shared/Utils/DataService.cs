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

            // FAKE DATA
            //items.Add(new GalleryImage { Title = "iojfioqenjfodwiew 8a76f9dad snfd " });
            //items.Add(new GalleryImage { Title = "87aedfbjadc " });
            //items.Add(new GalleryImage { Title = "3333333333333 8a76f9dad snfd " });
            //items.Add(new GalleryImage { Title = "ANFLSDANFLSKDNC SDF  " });
            //return items;

            var s = await this.DownloadData(uri);
            System.Diagnostics.Debug.WriteLine("[URI]\t{0}\n[RESPONSE]{1}", uri, s);
            try
            {
                var des = JsonConvert.DeserializeObject<GalleryImage>(s);
                foreach (var d in des.Data)
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

        private async Task<String> DownloadData(String uri)
        {
            var response = String.Empty;
            var httpClient = new HttpClient();
            try
            {
                StorageFolder sFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                StorageFile sFile = await sFolder.CreateFileAsync(Constants.AUTH_LOCALSETTINGS_FILENAME, CreationCollisionOption.OpenIfExists);
                // READ
                string sFileContent = null;
                using (var sFileReader = new StreamReader(await sFile.OpenStreamForReadAsync()))
                {
                    sFileContent = await sFileReader.ReadToEndAsync();
                }

                if (sFileContent != null)
                {

                    // httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", "Client-ID " + Constants.API_CLIENTID);
                    AuthUser jsonToAuthUser = await JsonConvert.DeserializeObjectAsync<AuthUser>(sFileContent);
                    Debug.WriteLine("[DATASERVICE]\t" + jsonToAuthUser.AccessToken);
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", "Bearer " + jsonToAuthUser.AccessToken);
                    response = await httpClient.GetStringAsync(new Uri(uri));
                    //var content = new FormUrlEncodedContent(new[] 
                    //    {
                    //        new KeyValuePair<string, string>("Authorization", "Client-ID " + Constants.API_CLIENTID)
                    //    });
                    //var r = await httpClient.PostAsync(new Uri(uri), content);
                    //response = r.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[DataService.DownloadData] \n" + ex.ToString());
            }
            
            return response;
        }
    }
}
