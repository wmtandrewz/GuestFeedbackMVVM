
using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows.Input;
using CGFSMVVM.DataParsers;
using CGFSMVVM.Helpers;
using CGFSMVVM.Models;
using CGFSMVVM.Services;
using CGFSMVVM.Views;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Plugin.Connectivity;
using Plugin.DeviceInfo;
using Plugin.Geolocator;
using Xamarin.Forms;

namespace CGFSMVVM.ViewModels
{
    public class MainViewModel
    {
        public INavigation _navigation
        {
            get;
            set;
        }

        public ICommand LaunchStartButtonCommand { get; }
        public ICommand SettingsTappedCommand { get; }
        public ICommand CheckConnectionCommand { get; }
        public ICommand CheckConfigurationsCommand { get; }
        public ICommand CheckAppVersionCommand { get; }

        public MainViewModel(INavigation navigation)
        {
            this._navigation = navigation;
            LaunchStartButtonCommand = new Command(StartButtonClicked);
            SettingsTappedCommand = new Command(SetingsButtonTapped);
            CheckConnectionCommand = new Command(CheckConnection);
            CheckConfigurationsCommand = new Command(CheckConfigurations);
            CheckAppVersionCommand = new Command(CheckAppVersion);

            NativeCamera.InitCamera();

			FeedbackCart._hotelIdentifier = Settings.HotelIdentifier;

        }

        private async void CheckConfigurations()
        {
            if(string.IsNullOrEmpty(Settings.BaseDomainURL))
            {
                await Application.Current.MainPage.DisplayAlert("Gateway Unavailable", "Please set App configurations", "OK").ConfigureAwait(true);
				//new UserLogout().logout();
            }
        }

        private async void CheckConnection()
        {
            bool isConnected = CrossConnectivity.Current.IsConnected;

            if (!isConnected)
            {
                await Application.Current.MainPage.DisplayAlert("No Internet Connection", "Please check your internet connection", "OK").ConfigureAwait(true);
                Thread.CurrentThread.Abort();
            }
        }

        private void SetingsButtonTapped()
        {
            _navigation.PushAsync(new SystemConfigView());
        }

        private async void StartButtonClicked()
        {
            SetDeviceInfo();
            await _navigation.PushAsync(new GuestDetailsView());
        }

        private async void CheckAppVersion()
        {
            try
			{
				ConfigModel responceConfigs = await ConfigDeserializer.DeserializeConfigurations();
                
				if (responceConfigs != null)
				{
                    if (Convert.ToDouble(responceConfigs.AppVersion) > Convert.ToDouble(Settings.AppVersion))
					{
						DateTime expiaryDate = Convert.ToDateTime(responceConfigs.PriorVersionExpiryDate);

						if (expiaryDate <= DateTime.Today)
						{
							Console.WriteLine(responceConfigs.AppVersion + " Expired " + responceConfigs.PriorVersionExpiryDate);

							await Application.Current.MainPage.DisplayAlert("Alert!", "New version available.. \n\nPlease update App to latest release", "OK").ConfigureAwait(true);
							Thread.CurrentThread.Abort();
						}

						else
						{
							await Application.Current.MainPage.DisplayAlert("Alert!", $"New version available.\n\nCurrent version will be expired on \n{expiaryDate.ToString("yyyy MMM dd")} \n\nPlease update App to latest release", "OK").ConfigureAwait(true);

						}
					}
				}

			}
			catch (Exception ex)
			{
                Debug.WriteLine("App version failed");
				Analytics.TrackEvent($"MainViewModel.CheckApp Version {ex.Message}");
				Crashes.TrackError(ex);
			}

		}

		private async void SetDeviceInfo()
		{
            //string Lati = "";
            //string Longti = "";

            try
            {
                if (IsLocationAvailable())
                {
                    var locator = CrossGeolocator.Current;
                    locator.DesiredAccuracy = 10;

                    var position = await locator.GetPositionAsync(TimeSpan.FromMilliseconds(10000));
                    Debug.WriteLine("Position Status: {0}", position.Timestamp);
                    Debug.WriteLine("Position Latitude: {0}", position.Latitude);
                    Debug.WriteLine("Position Longtitude: {0}", position.Longitude);
                    //Lati = Math.Round(position.Latitude, 6).ToString();
                    //Longti = Math.Round(position.Longitude, 6).ToString();
                }
            }
            catch(Exception)
            {

            }

			if (!string.IsNullOrEmpty(Settings.Username))
			{
                FeedbackCart._createdBy = $"{Settings.Username}";
            }
        }

        public bool IsLocationAvailable()
        {
            if (!CrossGeolocator.IsSupported)
                return false;

            return CrossGeolocator.Current.IsGeolocationAvailable;
        }
    }
}
