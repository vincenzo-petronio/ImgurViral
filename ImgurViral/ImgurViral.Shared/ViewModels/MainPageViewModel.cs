using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImgurViral.ViewModels
{
    public class MainPageViewModel : PropertyChangedBase
    {
        private Boolean progressRingIsActive;
        private List<String> items;
        
        public MainPageViewModel()
        {
            progressRingIsActive = true;
            this.items = new List<string>();
            items.Add("akjsnflasnfla");
            items.Add("7856346839");
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
        
        public List<String> Items
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
    }
}
