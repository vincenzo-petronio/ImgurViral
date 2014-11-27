﻿using ImgurViral.Models;
using ImgurViral.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Il modello di elemento per la pagina vuota è documentato all'indirizzo http://go.microsoft.com/fwlink/?LinkId=234238

namespace ImgurViral.Views
{
    /// <summary>
    /// Pagina vuota che può essere utilizzata autonomamente oppure esplorata all'interno di un frame.
    /// </summary>
    public sealed partial class AuthView : Page
    {
        private const String AUTH_ACCESS_TOKEN = "access_token";
        private const String AUTH_EXPIRES = "expires_in";
        private const String AUTH_TYPE = "token_type";
        private const String AUTH_REFRESH = "refresh_token";
        private const String AUTH_ACCOUNT = "account_username";

        public AuthView()
        {
            this.InitializeComponent();
        }

        // WEBVIEW LifeCycle:
        // - NavigationStarting
        // - ContentLoading
        // - NavigationCompleted
        // - NavigationFailed

        private void WebView_Loaded(object sender, RoutedEventArgs e)
        {
            // Request: https://api.imgur.com/oauth2/authorize?client_id=CLIENT_ID&response_type=token
            this.webView.Navigate(new Uri(String.Format(Constants.ENDPOINT_API_AUTHORIZE, Constants.API_CLIENTID)));
        }

        private async void webView_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            // Response: http://example.com#access_token=ACCESS_TOKEN&token_type=Bearer&expires_in=3600
            string uriToString = args.Uri.ToString();
            Debug.WriteLine("[AUTH RESPONSE]\t" + uriToString);
            if (uriToString.Contains(AUTH_ACCESS_TOKEN))
            {
                AuthUser authUser = new AuthUser();

                int indexOfSharp = uriToString.IndexOf("#");
                string query = uriToString.Substring(indexOfSharp + 1, uriToString.Length - indexOfSharp - 1);

                authUser.AccessToken = query.Split('&')
                                .Where(s => s.Split('=')[0] == AUTH_ACCESS_TOKEN)
                                .Select(s => s.Split('=')[1])
                                .FirstOrDefault();

                authUser.RefreshToken = query.Split('&')
                                        .Where(s => s.Split('=')[0] == AUTH_REFRESH)
                                        .Select(s => s.Split('=')[1])
                                        .FirstOrDefault();

                authUser.Username = query.Split('&')
                                        .Where(s => s.Split('=')[0] == AUTH_ACCOUNT)
                                        .Select(s => s.Split('=')[1])
                                        .FirstOrDefault();

                authUser.ExpiresToken = query.Split('&')
                                        .Where(s => s.Split('=')[0] == AUTH_EXPIRES)
                                        .Select(s => s.Split('=')[1])
                                        .FirstOrDefault();

                authUser.TypeToken = query.Split('&')
                                        .Where(s => s.Split('=')[0] == AUTH_TYPE)
                                        .Select(s => s.Split('=')[1])
                                        .FirstOrDefault();

                if (await AuthHelper.SaveAuthData(authUser))
                {
                    Frame.Navigate(typeof(MainPageView));
                }
            }
            else
            {
                Debug.WriteLine("[AUTH RESPONSE]\t" + "NO ACCESS TOKEN!");
            }
        }

        private void webView_ManipulationStarting(object sender, ManipulationStartingRoutedEventArgs e)
        {

        }
    }
}