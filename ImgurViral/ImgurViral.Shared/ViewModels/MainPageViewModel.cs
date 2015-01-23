using Caliburn.Micro;
using ImgurViral.Exceptions;
using ImgurViral.Models;
using ImgurViral.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.Resources;
using Windows.UI.Popups;

namespace ImgurViral.ViewModels
{
    public class MainPageViewModel : Screen
    {
        private IDataService dataService;
        private INavigationService navigationService;
        private Boolean progressRingIsActive;
        private List<GalleryImageData> items;
        private GalleryImageData selectedItem;
        private String itemsCounter;
        private ResourceLoader resourceLoader;
        private Boolean isLogoutVisible;
        private DataTransferManager dtm;

        public MainPageViewModel(IDataService dataService, INavigationService navigationService)
        {
            this.dataService = dataService;
            this.navigationService = navigationService;
            this.progressRingIsActive = true;
            this.items = new List<GalleryImageData>();
            this.resourceLoader = new ResourceLoader();
            this.isLogoutVisible = true;

#if WINDOWS_PHONE_APP
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
#endif

        }

        protected override void OnActivate()
        {
            base.OnActivate();
            this.dataService.GetGalleryImage(async (gallery, err) => {
                ProgressRingIsActive = false;
                if (err == null)
                {
                    this.Items = gallery;
                    this.dtm = DataTransferManager.GetForCurrentView();
                    this.dtm.DataRequested += ShareHandler;
                }
                else
                {
                    if (err.GetType() == typeof(ApiException))
                    {
                        Debug.WriteLine("[MainPageViewModel.OnActivate]\t" + "ApiException");
                        ApiError apiError = JsonConvert.DeserializeObject<ApiError>(err.Message);

                        var dialog = new MessageDialog(apiError.Status + " - " + apiError.Data.Error + "\n" + resourceLoader.GetString("msg_login_again"));
                        dialog.Commands.Add(new UICommand("OK",
                            new UICommandInvokedHandler(async (s) =>
                            {
                                await AuthHelper.DeleteAuthData();
                                navigationService.GoBack();
                            })));
                        await dialog.ShowAsync();
                    }
                    else if (err.GetType() == typeof(NetworkException))
                    {
                        Debug.WriteLine("[MainPageViewModel.OnActivate]\t" + "NetworkException");
                        var dialog = new MessageDialog(resourceLoader.GetString("msg_connection_error"));
                        dialog.Commands.Add(new UICommand("OK",
                            new UICommandInvokedHandler((s) => { CaliburnApplication.Current.Exit(); })));
                        await dialog.ShowAsync();
                    }
                    else if (err.GetType() == typeof(ArgumentNullException))
                    {
                        Debug.WriteLine("[MainPageViewModel.OnActivate]\t" + "ArgumentNullException");
                    }
                }
            });
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
            if (this.dtm != null)
            {
                this.dtm.DataRequested -= ShareHandler;
            }
        }

#if WINDOWS_PHONE_APP
        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            CaliburnApplication.Current.Exit();
        }
#endif

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

        public void ImageIsOpened()
        {
            Debug.WriteLine("IS_IMAGE_OPENED");
            // ProgressRingIsActive = false;
        }

        public void ImageIsFailed()
        {
            Debug.WriteLine("IS_IMAGE_FAILED");
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
                if (selectedItem != value && value != null)
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
            this.navigationService.GoBack();
        }

        public void Share()
        {
            Windows.ApplicationModel.DataTransfer.DataTransferManager.ShowShareUI();
        }

        private void ShareHandler(DataTransferManager sender, DataRequestedEventArgs e)
        {
            e.Request.Data.Properties.Title = resourceLoader.GetString("AppTitle/Text");
            e.Request.Data.SetText(SelectedItem.Title + "\n\n" + SelectedItem.Link);
        }
    }
}
