using Caliburn.Micro;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml.Controls;
using ImgurViral.ViewModels;
using ImgurViral.Views;
using ImgurViral.Utils;
using ImgurViral.Models;

// Il modello di applicazione vuota è documentato all'indirizzo http://go.microsoft.com/fwlink/?LinkId=234227

namespace ImgurViral
{
    /// <summary>
    /// Fornisci un comportamento specifico dell'applicazione in supplemento alla classe Application predefinita.
    /// </summary>
    public sealed partial class App
    {
        private WinRTContainer container;
        //public static AuthUser authUser = new AuthUser();

        /// <summary>
        /// Inizializza l'oggetto Application singleton. Si tratta della prima riga del codice creato
        /// eseguita e, come tale, corrisponde all'equivalente logico di main() o WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
        }

        protected override void Configure()
        {
            container = new WinRTContainer();
            container.RegisterWinRTServices();
            container.Singleton<IDataService, DataService>();
            container.PerRequest<AuthViewModel>();
            container.PerRequest<MainPageViewModel>();
            //container.Singleton<AuthUser>();
        }

        protected override void PrepareViewFirst(Frame rootFrame)
        {
            container.RegisterNavigationService(rootFrame);
        }

        /// <summary>
        /// Richiamato quando l'applicazione viene avviata normalmente dall'utente.  All'avvio dell'applicazione
        /// verranno utilizzati altri punti di ingresso per aprire un file specifico, per visualizzare
        /// risultati di ricerche e così via.
        /// </summary>
        /// <param name="e">Dettagli sulla richiesta e il processo di avvio.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            // View First approach
            // DisplayRootView<MainPageView>();
            DisplayRootView<AuthView>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            container.BuildUp(instance);
        }
    }
}