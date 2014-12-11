using Caliburn.Micro;
using ImgurViral.Models;
using ImgurViral.Utils;
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
        
        public MainPageViewModel(IDataService dataService)
        {
            this.dataService = dataService;
            this.progressRingIsActive = true;
            this.items = new List<GalleryImageData>();
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
                    var resourceLoader = new ResourceLoader();
                    var dialog = new MessageDialog(resourceLoader.GetString("msg_connection_error"));
                    dialog.Commands.Add(new UICommand("OK", 
                        new UICommandInvokedHandler((s) => { CaliburnApplication.Current.Exit(); })));
                    await dialog.ShowAsync();
                    CaliburnApplication.Current.Exit();
                }
            });
        }

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
    }
}
