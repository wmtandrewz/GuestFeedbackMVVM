using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using CGFSMVVM.Helpers;
using CGFSMVVM.Models;
using CGFSMVVM.Views;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace CGFSMVVM
{
    public partial class SystemConfigView : ContentPage
    {
        List<HotelModel> hotelList = new List<HotelModel>();

        public SystemConfigView()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
        }

        void RegisterButton_Clicked(object sender, EventArgs e)
        {
            Resister();
        }

        void ExitBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainView());
            Navigation.RemovePage(this);
        }

        public static async Task<string> RegisterDevice(string uuid)
        {
            
            try
            {
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(Settings.ConfigAPIUri)
                };
                var response = await client.GetAsync($"GFBConfig/RegisterDevice/{uuid}");
                var resultVersion = response.Content.ReadAsStringAsync().Result;

                if (!string.IsNullOrEmpty(resultVersion))
                {
                    return resultVersion;
                }
                else
                {
                    return "error";
                }
            }

            catch (Exception)
            {
                return "error";
            }

        }

        private async void Resister()
        {
            try
            {
                if (!string.IsNullOrEmpty(uuidEntry.Text))
                {
                    var res = await RegisterDevice(uuidEntry.Text).ConfigureAwait(true);
                    Console.WriteLine(res);

                    if (!string.IsNullOrEmpty(res))
                    {
                        if (res.Contains("Device Already Registered"))
                        {
                            deviceLabel.Text = "Device is already registered";
                            await DisplayAlert("Failed!", "Device already registered. \nPlease contact your IT department.", "OK");
                        }
                        else if(res.Contains("Device not configured"))
                        {
                            deviceLabel.Text = "Device is not in the server";
                            await DisplayAlert("Failed!", "Device is not configured in server. \nPlease contact your IT department.", "OK");
                        }
                        else
                        {
                            hotelList = JsonConvert.DeserializeObject<List<HotelModel>>(res);

                            if (hotelList != null)
                            {
                                HotelModel hotelModel = hotelList[0];

                                Settings.HotelCode = hotelModel.TmsId;
                                Settings.HotelIdentifier = hotelModel.HtlCode;
                                Settings.HotelName = hotelModel.HtlName;
                                Settings.IsUUIDregistered = "Device is registered";
                                Settings.DeviceUUID = uuidEntry.Text;

                                hotelNameLabel.Text = Settings.HotelName;
                                versionLabel.Text = Settings.AppVersion;
                                regStatLabel.Text = Settings.IsUUIDregistered;
                                deviceLabel.Text = "Device is registered";

                                hotelNameLabel.TextColor = Color.Blue;
                                versionLabel.TextColor = Color.Blue;
                                regStatLabel.TextColor = Color.Blue;
                                deviceLabel.TextColor = Color.Blue;

                                registerButton.IsVisible = false;
                                uuidEntry.IsEnabled = false;

                                await DisplayAlert("Success!", "Device registration success.", "OK");
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                
            }
        }

        public async void GetDeviceInformation(string uuid)
        {

            try
            {
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(Settings.ConfigAPIUri)
                };
                var response = await client.GetAsync($"GFBConfig/GetDeviceInfo/{uuid}").ConfigureAwait(true);
                var responceDevice = response.Content.ReadAsStringAsync().Result;

                if (!string.IsNullOrEmpty(responceDevice))
                {
                    DeviceInfoModel deviceInfoModel = JsonConvert.DeserializeObject<DeviceInfoModel>(responceDevice);

                    if (deviceInfoModel != null && deviceInfoModel.IsResgistered == "true")
                    {
                        deviceLabel.Text = deviceInfoModel.Notes;
                    }
                    else if(deviceInfoModel.IsResgistered == "false")
                    {
                        Settings.IsUUIDregistered = "Device is not registered";

                        deviceLabel.Text = "Device is not registered";
                        hotelNameLabel.TextColor = Color.Red;
                        versionLabel.TextColor = Color.Red;
                        regStatLabel.TextColor = Color.Red;
                        deviceLabel.TextColor = Color.Red;

                        hotelNameLabel.Text = "N/A";
                        versionLabel.Text = Settings.AppVersion;
                        regStatLabel.Text = Settings.IsUUIDregistered;

                        registerButton.IsVisible = true;
                        uuidEntry.IsEnabled = true;

                        layoutConfig.IsVisible = true;

                        uuidEntry.Focus();

                        await DisplayAlert("Alert!", "Device is not registered. Please register your device", "OK");
                    }
                }

            }

            catch (Exception)
            {
               
            }

        }

        protected override void OnAppearing()
        {
            if (!string.IsNullOrEmpty(Settings.DeviceUUID))
            {
                deviceLabel.TextColor = Color.Blue;

                GetDeviceInformation(Settings.DeviceUUID);
            }

            if (Settings.IsUUIDregistered == "Device is registered")
            {

                hotelNameLabel.Text = Settings.HotelName;
                versionLabel.Text = Settings.AppVersion;
                regStatLabel.Text = Settings.IsUUIDregistered;

                hotelNameLabel.TextColor = Color.Blue;
                versionLabel.TextColor = Color.Blue;
                regStatLabel.TextColor = Color.Blue;

                uuidEntry.Text = Settings.DeviceUUID;

                uuidEntry.IsEnabled = false;
                registerButton.IsVisible = false;

                layoutConfig.IsVisible = true;
            }
            else
            {

                hotelNameLabel.TextColor = Color.Red;
                versionLabel.TextColor = Color.Red;
                regStatLabel.TextColor = Color.Red;
                deviceLabel.TextColor = Color.Red;

                hotelNameLabel.Text = "N/A";
                versionLabel.Text = Settings.AppVersion;
                regStatLabel.Text = Settings.IsUUIDregistered;

                registerButton.IsVisible = true;
                uuidEntry.IsEnabled = true;

                layoutConfig.IsVisible = true;

                uuidEntry.Focus();
            }



            base.OnAppearing();
        }
    }
}
