using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CGFSMVVM.DataParsers;
using CGFSMVVM.Helpers;
using CGFSMVVM.Models;
using CGFSMVVM.Services;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace CGFSMVVM.ViewModels
{
    /// <summary>
    /// Finish page view model.
    /// </summary>
    public class FinishPageViewModel
    {
        public ICommand FinishButtonCommand { get; }
        public ICommand PageAppearingCommand { get; }
        public INavigation Navigation { get; }
        public Button finishButton { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CGFSMVVM.ViewModels.FinishPageViewModel"/> class.
        /// </summary>
        /// <param name="navigation">Navigation.</param>
        public FinishPageViewModel(INavigation navigation)
        {
            this.Navigation = navigation;

            FinishButtonCommand = new Command(FinishButtonPressed);
            PageAppearingCommand = new Command<Button>(OnAppearing);
        }

        /// <summary>
        /// Save feedback data to API in page appearing
        /// </summary>
        /// <param name="button">Button.</param>
        private async void OnAppearing(Button button)
        {
            button.IsEnabled = false;

            try
            {
				bool responce = await APIPostServices.SaveFeedbackData();
                if(responce)
                {
                    button.IsEnabled = true;
                }
                else
                {
                    button.IsEnabled = true;
                }
            }
            catch(Exception)
            {
            }
        }

        /// <summary>
        /// Clear the feedback data and reloanch the main page the button pressed.
        /// </summary>
        private async void FinishButtonPressed()
        {
            bool isCompleted = await GetDeviceInformation();

            if (isCompleted)
            {
                FeedbackCart.ClearSavedData();
                await Navigation.PopToRootAsync();
                //Thread.CurrentThread.Abort();
            }
            else
            {
                FeedbackCart.ClearSavedData();
                await Navigation.PopToRootAsync();
            }
        }

        public async Task<bool> GetDeviceInformation()
        {

            try
            {
                if (!string.IsNullOrEmpty(Settings.DeviceUUID))
                {
                    HttpClient client = new HttpClient
                    {
                        BaseAddress = new Uri(Settings.ConfigAPIUri)
                    };
                    var response = await client.GetAsync($"GFBConfig/GetDeviceInfo/{Settings.DeviceUUID}").ConfigureAwait(true);
                    var responceDevice = response.Content.ReadAsStringAsync().Result;

                    if (!string.IsNullOrEmpty(responceDevice))
                    {
                        DeviceInfoModel deviceInfoModel = JsonConvert.DeserializeObject<DeviceInfoModel>(responceDevice);

                        if (deviceInfoModel != null && deviceInfoModel.IsResgistered == "false")
                        {
                            Settings.HotelCode = string.Empty;
                            Settings.HotelIdentifier = string.Empty;
                            Settings.HotelName = string.Empty;
                            Settings.IsUUIDregistered = "Device is not registered";

                            return true;

                        }
                    }
                }

                return true;

            }

            catch (Exception)
            {
                return true;
            }

        }
    }
}
