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
using ImgurViral.Exceptions;

namespace ImgurViral.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public class DataService : IDataService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public async Task<List<GalleryImageData>> getGalleryImage(Action<List<GalleryImageData>, Exception> callback)
        {
            Exception exception = null;
            List<GalleryImageData> results = new List<GalleryImageData>();
            String uri = Constants.ENDPOINT_API_GALLERY_VIRAL;
            var response = String.Empty;

            try
            {
                response = await this.DownloadData(uri);
                
                if (String.IsNullOrEmpty(response))
                {
                    throw new ArgumentNullException();
                }
                    
                GalleryImage responseDeserialized = JsonConvert.DeserializeObject<GalleryImage>(response);
                // Filtro gli item che non sono visualizzabili, esempio video o album o GIF.
                var responseDeserializedRestricted = from item in responseDeserialized.Data
                                                        where item.IsAlbum == false
                                                        && !item.Type.Contains("gif")
                                                        select item;
                foreach (var d in responseDeserializedRestricted)
                {
                    results.Add(d);
                }
            }
            catch (NetworkException ne)
            {
                exception = ne;
            }
            catch (ArgumentNullException ane)
            {
                exception = ane;
            }

            System.Diagnostics.Debug.WriteLine("[URI]\t{0}\n[RESPONSE]{1}\n\n", uri, response);

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
            HttpResponseMessage httpResponseMessage;
            HttpClient httpClient = new HttpClient();
            AuthUser authUser;

            if (!NetworkHelper.CheckConnectivity())
            {
                throw new NetworkException("NO_NET");
            }

            try
            {
                authUser = await AuthHelper.ReadAuthData();
                if (null != authUser && !String.IsNullOrEmpty(authUser.AccessToken) && !String.IsNullOrEmpty(authUser.RefreshToken))
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", "Bearer " + authUser.AccessToken);
                    httpResponseMessage = await httpClient.GetAsync(new Uri(uri));
                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        response = await httpResponseMessage.Content.ReadAsStringAsync();
                    }
                    else if(httpResponseMessage.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    {
                        Debug.WriteLine("[DataService.DownloadData]\t" + "HttpStatusCode 403");

                        bool isNewToken = await RefreshAccessToken(authUser.RefreshToken);
                        if (isNewToken)
                        {
                            authUser = await AuthHelper.ReadAuthData();
                            if (null != authUser && !String.IsNullOrEmpty(authUser.AccessToken) && !String.IsNullOrEmpty(authUser.RefreshToken))
                            {
                                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", "Bearer " + authUser.AccessToken);
                                httpResponseMessage = await httpClient.GetAsync(new Uri(uri));
                                if (httpResponseMessage.IsSuccessStatusCode)
                                {
                                    response = await httpResponseMessage.Content.ReadAsStringAsync();
                                }
                            }
                        }
                    }
                }
                else
                {
                    Debug.WriteLine("[DataService.DownloadData]\t" + "NO ACCESS TOKEN FROM LOCAL!");
                }
            }
            catch (HttpRequestException hre)
            {
                Debug.WriteLine("[DataService.DownloadData]\t" + "HttpRequestException\n" + hre.ToString());
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[DataService.DownloadData]\t" + "Exception\n" + ex.ToString());
            }
            
            return response;
        }

        /// <summary>
        /// Ottiene un nuovo Access Token attraverso il Refresh Token, e memorizza i nuovi dati in locale.
        /// </summary>
        /// <param name="refreshToken">String</param>
        /// <returns>bool</returns>
        public async static Task<bool> RefreshAccessToken(String refreshToken)
        {
            var response = String.Empty;
            HttpResponseMessage httpResponseMessage;
            HttpClient httpClient = new HttpClient();

            // QUERYSTRING
            var values = new Dictionary<string, string>();
            values.Add(Constants.AUTH_REFRESH, refreshToken);
            values.Add(Constants.AUTH_CLIENT_ID, Constants.API_CLIENTID);
            values.Add(Constants.AUTH_CLIENT_SECRET, Constants.API_SECRET);
            values.Add(Constants.AUTH_GRANT_TYPE, "refresh_token");
            var content = new FormUrlEncodedContent(values);

            httpResponseMessage = await httpClient.PostAsync(new Uri(Constants.ENDPOINT_API_REFRESH_BASE), content);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                response = await httpResponseMessage.Content.ReadAsStringAsync();
                Debug.WriteLine("[DataService.RefreshAccessToken]\t" + httpResponseMessage.StatusCode + " New Access Token: " + response);
                AuthUser authUser = await AuthHelper.CreateAuthUser(response, false);
                bool isSaved = await AuthHelper.SaveAuthData(authUser);
                if (isSaved)
                {
                    Debug.WriteLine("[DataService.RefreshAccessToken]\t" + "New Access Token stored!");
                    return true;
                }
            }
            else
            {

                Debug.WriteLine("[DataService.RefreshAccessToken]\t" + httpResponseMessage.StatusCode + " Error New Access Token: " + response);
            }

            return false;
        }
    }
}
