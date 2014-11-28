using ImgurViral.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace ImgurViral.Utils
{
    class AuthHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user">AuthUser</param>
        /// <returns>Boolean True/False se il salvataggio è andato a buon fine o meno.</returns>
        public async static Task<bool> SaveAuthData(AuthUser user)
        {
            string authUserToJson = await JsonConvert.SerializeObjectAsync(user, Formatting.Indented);

            StorageFolder sFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile sFile = await sFolder.CreateFileAsync(Constants.AUTH_LOCALSETTINGS_FILENAME, CreationCollisionOption.OpenIfExists);
            
            // WRITE
            using(var sWriter = new StreamWriter(await sFile.OpenStreamForWriteAsync())) 
            {
                await sWriter.WriteAsync(authUserToJson);
                await sWriter.FlushAsync();
            }

            // READ
            string sFileContent = null;
            using (var sFileReader = new StreamReader(await sFile.OpenStreamForReadAsync()))
            {
                sFileContent = await sFileReader.ReadToEndAsync();
            }

            if (sFileContent != null)
            {
                AuthUser jsonToAuthUser = await JsonConvert.DeserializeObjectAsync<AuthUser>(sFileContent);
                Debug.WriteLine("[AUTHHELPER]\t" + 
                    "JSON=[" + "Username=" + jsonToAuthUser.Username + ", " + 
                    "AccessToken=" + jsonToAuthUser.AccessToken + ", " + 
                    "ExpiresToken=" + jsonToAuthUser.ExpiresToken + ", " + 
                    "RefreshToken=" + jsonToAuthUser.RefreshToken + ", " + 
                    "TypeToken=" + jsonToAuthUser.TypeToken + "]");
            }
            else
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>AuthUser</returns>
        public async static Task<AuthUser> ReadAuthData()
        {
            StorageFolder sFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile sFile = await sFolder.GetFileAsync(Constants.AUTH_LOCALSETTINGS_FILENAME);
            string sFileContent = null;
            AuthUser jsonToAuthUser = null;

            using (var sFileReader = new StreamReader(await sFile.OpenStreamForReadAsync()))
            {
                sFileContent = await sFileReader.ReadToEndAsync();
            }

            if (sFileContent != null)
            {
                jsonToAuthUser = await JsonConvert.DeserializeObjectAsync<AuthUser>(sFileContent);
                Debug.WriteLine("[AUTHHELPER]\t" +
                    "JSON=[" + "Username=" + jsonToAuthUser.Username + ", " +
                    "AccessToken=" + jsonToAuthUser.AccessToken + ", " +
                    "ExpiresToken=" + jsonToAuthUser.ExpiresToken + ", " +
                    "RefreshToken=" + jsonToAuthUser.RefreshToken + ", " +
                    "TypeToken=" + jsonToAuthUser.TypeToken + "]");
            }

            return jsonToAuthUser;
        }
    }
}
