using Caliburn.Micro;
using ImgurViral.Exceptions;
using ImgurViral.Models;
using ImgurViral.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.ApplicationModel.Resources;
using Windows.UI.Popups;

namespace ImgurViral.ViewModels
{
    public class MainPageViewModel : Screen
    {
        private IDataService dataService;
        private Boolean progressRingIsActive;
        private List<GalleryImageData> items;
        private GalleryImageData selectedItem;
        private String itemsCounter;
        private ResourceLoader resourceLoader;
        private Boolean isLogoutVisible;
        
        public MainPageViewModel(IDataService dataService)
        {
            this.dataService = dataService;
            this.progressRingIsActive = true;
            this.items = new List<GalleryImageData>();
            this.resourceLoader = new ResourceLoader();
            this.isLogoutVisible = true;
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            this.dataService.getGalleryImage(async (gallery, err) => {
                ProgressRingIsActive = false;
                if (err == null)
                {
                    this.Items = gallery;
                }
                else
                {
                    if(err.GetType() == typeof(ApiException)) 
                    {
                        Debug.WriteLine("ApiException");
                        ApiError apiError = JsonConvert.DeserializeObject<ApiError>(err.Message);
                        var dialog = new MessageDialog(apiError.Status + " - " + apiError.Data.Error);
                        dialog.Commands.Add(new UICommand("OK",
                            new UICommandInvokedHandler((s) => { /*CaliburnApplication.Current.Exit();*/ })));
                        await dialog.ShowAsync();
                    }
                    else if (err.GetType() == typeof(NetworkException))
                    {
                        Debug.WriteLine("NetworkException");
                        var dialog = new MessageDialog(resourceLoader.GetString("msg_connection_error"));
                        dialog.Commands.Add(new UICommand("OK",
                            new UICommandInvokedHandler((s) => { CaliburnApplication.Current.Exit(); })));
                        await dialog.ShowAsync();
                    }
                    else if (err.GetType() == typeof(ArgumentNullException))
                    {
                        Debug.WriteLine("ArgumentNullException");
                    }
                }
            });
        }

        /// <summary>
        /// In binding con la proprietà Visible della Progress.
        /// </summary>
        public Boolean ProgressRingIsActive
        {
            get
            {
                return progressRingIsActive;
            }

            set
            {
                if (progressRingIsActive != value)
                {
                    progressRingIsActive = value;
                    NotifyOfPropertyChange(() => ProgressRingIsActive);
                }
            }
        }

        /// <summary>
        /// In binding con la proprietà Visible del Button Logout.
        /// </summary>
        public Boolean IsLogoutVisible
        {
            get
            {
                return isLogoutVisible;
            }

            set
            {
                if (isLogoutVisible != value)
                {
                    IsLogoutVisible = value;
                    NotifyOfPropertyChange(() => IsLogoutVisible);
                }
            }
        }

        /// <summary>
        /// Collection of items for FlipView
        /// </summary>
        public List<GalleryImageData> Items
        {
            get
            {
                return items;
            }

            set
            {
                if(items != value)
                {
                    items = value;
                    NotifyOfPropertyChange(() => Items);
                }
            }
        }

        /// <summary>
        /// Selected item from FlipView
        /// </summary>
        public GalleryImageData SelectedItem
        {
            get
            {
                return selectedItem;
            }

            set
            {
                if (selectedItem != value)
                {
                    selectedItem = value;
                    NotifyOfPropertyChange(() => SelectedItem);
                }
            }
        }

        /// <summary>
        /// Counter for the images of FlipView. 
        /// </summary>
        public String ItemsCounter
        {
            get
            {
                return itemsCounter;
            }

            set
            {
                if (itemsCounter != value)
                {
                    itemsCounter = value;
                    NotifyOfPropertyChange(() => ItemsCounter);
                }
            }
        }

        public async void Logout()
        {
            await AuthHelper.DeleteAuthData();
            CaliburnApplication.Current.Exit();
        }
    }
}
