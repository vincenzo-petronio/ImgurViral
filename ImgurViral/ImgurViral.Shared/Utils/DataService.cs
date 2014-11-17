using ImgurViral.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Linq;

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
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", "Client-ID " + Constants.API_CLIENTID);
                response = await httpClient.GetStringAsync(new Uri(uri));
                //var content = new FormUrlEncodedContent(new[] 
                //    {
                //        new KeyValuePair<string, string>("Authorization", "Client-ID " + Constants.API_CLIENTID)
                //    });
                //var r = await httpClient.PostAsync(new Uri(uri), content);
                //response = r.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[DataService.DownloadData] \n" + ex.ToString());
            }
            
            return response;
        }
    }
}
