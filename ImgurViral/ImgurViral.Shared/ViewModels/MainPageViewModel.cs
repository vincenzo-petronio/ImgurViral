using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImgurViral.ViewModels
{
    public class MainPageViewModel : PropertyChangedBase
    {
        private List<String> items;
        
        public MainPageViewModel()
        {
            this.items = new List<string>();
            items.Add("akjsnflasnfla");
            items.Add("7856346839");
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
