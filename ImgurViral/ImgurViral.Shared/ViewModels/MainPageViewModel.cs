using Caliburn.Micro;
using ImgurViral.Models;
using ImgurViral.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;

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

        protected override async void OnActivate()
        {
            base.OnActivate();
            await this.dataService.getGalleryImage((gallery, err) => {
                if (gallery != null)
                {
                    this.Items = gallery;
                    ProgressRingIsActive = false;
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
