using System.Threading;
using CGFSMVVM.Helpers;
using CGFSMVVM.Models;
using CGFSMVVM.Views;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using Xamarin.Forms;

namespace CGFSMVVM
{
    /// <summary>
    /// Main Application Class 
    /// </summary>
    public partial class App : Application
    {
        private NavigationPage _navigationPage;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CGFSMVVM.App"/> class.
        /// </summary>
        public App()
        {
            InitializeComponent();

            CrossConnectivity.Current.ConnectivityChanged += Current_ConnectivityChanged;
            

			if (Settings.Username != "" && Settings.Password != "" && Settings.HotelCode != "")
            {
                //Restore Username and Password
                var user = new UserModel();
                user = new UserModel()
                {
                    Username = Settings.Username,
                    Password = Settings.Password
                };

                Constants._gatewayURL = Settings.SettingsSAPURL;
                Constants._cookie = Settings.SettingsSAPCookie;
                Constants._user = user;
                Constants._hotel_code = Settings.HotelCode;
                //Restore AccessToken and Expires Time

                Constants._access_token = Settings.AccessToken;
                Constants._expires_in = Settings.ExpiresTime;



				_navigationPage = new NavigationPage(new MainView())
                {
                    BackgroundColor = Color.Black,
                    BarTextColor = Color.White,
                    BarBackgroundColor = Color.Black,
                };

                MainPage = _navigationPage;

            }
            else //Not Logged Inn
            {
                Settings.SettingsSAPURL = "https://alastor.keells.lk:44300";
                Settings.SettingsSAPCookie = "sap-XSRF_GWP_100";
                
				_navigationPage = new NavigationPage(new LoginView())
                {
                    BackgroundColor = Color.Black,
                    BarTextColor = Color.White,
                    BarBackgroundColor = Color.Black,
                };

                MainPage = _navigationPage;
            }

			MessagingCenter.Subscribe<UserLogout>(this, "logout", (sender) =>
            {
				_navigationPage = new NavigationPage(new LoginView())
                {
                    BackgroundColor = Color.Black,
                    BarTextColor = Color.White,
                    BarBackgroundColor = Color.Black,
                };

                MainPage = _navigationPage;
            });

        }

        /// <summary>
        /// Checks the connectivity changed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Connectivity Change Event args</param>
		private async void Current_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (!e.IsConnected)
            {
                await Application.Current.MainPage.DisplayAlert("Error Connection", "Network Changed. App is Exiting..", "OK").ConfigureAwait(true);
                Thread.CurrentThread.Abort();
            }
        }

        /// <summary>
        /// Start App Center sevices in App start
        /// </summary>
        protected override void OnStart()
		{
            AppCenter.Start("ios=ce6ec43d-b49e-4cc8-a58e-fdb2a99065d9;", typeof(Analytics), typeof(Crashes));
            base.OnStart();
		}

	}
}
