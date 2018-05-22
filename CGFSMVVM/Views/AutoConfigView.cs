using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CGFSMVVM.DataParsers;
using CGFSMVVM.Helpers;
using CGFSMVVM.Models;
using CGFSMVVM.Services;
using CGFSMVVM.ViewModels;
using Xamarin.Forms;

namespace CGFSMVVM.Views
{
    public class AutoConfigView : ContentPage
    {


        Entry uuidEditor;
        StackLayout layout, formLayout;
        Label deviceNameLabel, appVersionLabel, hotelNameLabel;
        Button exitButton;
        ActivityIndicator indicator;

        public AutoConfigView()
        {
            
            InitComp();
            BackgroundColor = Color.Black;
            NavigationPage.SetHasNavigationBar(this, false);

        }


        private void InitComp()
        {
            layout = new StackLayout();
            layout.BackgroundColor = Color.Black;
            layout.VerticalOptions = LayoutOptions.CenterAndExpand;

            var titleLabel = new Label
            {
                Text = "Device Registration",
                FontSize = 24,
                TextColor = Color.Blue,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                VerticalTextAlignment = TextAlignment.Center,
                HeightRequest = 100

            };

            var deviceDetailsLayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.Center
            };

             deviceNameLabel = new Label
            {
                Text = $"\tDevice Name\t\t\t\t : {Settings.DeviceName}",
                FontSize = 18,
                TextColor = Color.Green,
                VerticalOptions = LayoutOptions.Start,
                VerticalTextAlignment = TextAlignment.End,
                HeightRequest = 40

            };

             appVersionLabel = new Label
            {
                Text = $"\tApp Version\t\t\t\t : {Settings.AppVersion}",
                FontSize = 18,
                TextColor = Color.Green,
                VerticalOptions = LayoutOptions.Start,
                VerticalTextAlignment = TextAlignment.End,
                HeightRequest = 40

            };

             hotelNameLabel = new Label
            {
                Text = $"\tHotel Name\t\t\t\t : {Settings.HotelName}",
                FontSize = 18,
                TextColor = Color.Green,
                VerticalOptions = LayoutOptions.Start,
                VerticalTextAlignment = TextAlignment.End,
                HeightRequest = 40

            };

            exitButton = new Button
            {
                Text = "Exit",
                TextColor = Color.White,
                FontSize = 20,
                HorizontalOptions = LayoutOptions.Start,
                HeightRequest = 40,
                WidthRequest = 400,
                BackgroundColor = Color.Blue,
                CornerRadius = 20,
                Margin = new Thickness(0, 20, 0, 20)
            };

            exitButton.Clicked+=delegate {

                Navigation.PopToRootAsync();
            };



            deviceDetailsLayout.Children.Add(deviceNameLabel);
            deviceDetailsLayout.Children.Add(appVersionLabel);
            deviceDetailsLayout.Children.Add(hotelNameLabel);
            deviceDetailsLayout.Children.Add(exitButton);


            //Form Layout
            formLayout = new StackLayout
            {
                //formLayout.BackgroundColor = Color.DarkSlateGray;
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.Start
            };


            var uuidLabel = new Label
            {
                Text = "Device ID (UUID)",
                FontSize = 18,
                TextColor = Color.White,
                VerticalOptions = LayoutOptions.Start,
                VerticalTextAlignment = TextAlignment.End,
                HeightRequest = 40

            };

            uuidEditor = new Entry
            {
                Placeholder = "Enter device UUID",
				Text = Settings.DeviceCurrentUUID,
                HeightRequest = 40,
                WidthRequest = 350,
                TextColor = Color.Black,
            };

             indicator = new ActivityIndicator
            {
                VerticalOptions=LayoutOptions.Center,
                Color = Color.Blue,
                HeightRequest = 50,
                IsVisible = false
            };

            var submitButton = new Button
            {
                Text = "Register",
                TextColor = Color.White,
                FontSize = 24,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                HeightRequest = 40,
                WidthRequest = 400,
                BackgroundColor = Color.Green,
                CornerRadius = 20,
                Margin = new Thickness(0, 20, 0, 20)
            };

            submitButton.Clicked +=  async delegate
            {
                if (!string.IsNullOrEmpty(uuidEditor.Text))
                {
                    RegisterButtonClicked();
                 }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "UUID Field can't be Empty \nPlease enter device uuid", "OK").ConfigureAwait(true);
                    uuidEditor.Focus();
                }
            }; 


            formLayout.Children.Add(titleLabel);
            formLayout.Children.Add(uuidLabel);
            formLayout.Children.Add(uuidEditor);
            formLayout.Children.Add(indicator);
            formLayout.Children.Add(submitButton);
            formLayout.Padding = new Thickness(150, 10, 150, 10);

            layout.Children.Add(deviceDetailsLayout);


            if (Settings.IsDeviceRegistered == false)
            {
                layout.Children.Add(formLayout);
            }


            layout.Padding = 10;
            layout.Spacing = 10;

            Content = layout;
        }

		protected override void OnAppearing()
		{
            base.OnAppearing();
            uuidEditor.Focus();
		}

        private async void RegisterButtonClicked()
        {
            indicator.IsVisible = true;
            indicator.IsRunning = true;
            this.IsBusy = true;

            var responce = await ConfigurationAPIServices.RegisterDevice(uuidEditor.Text.ToLower());
            //var responce = await ConfigurationAPIServices.RegisterDevice("39e3ba5743f5741b9aaa5b930f0f5ed5040aba24");

            if (responce.Contains("Device not configured"))
            {
                deviceNameLabel.Text = $"\tDevice is not configured";
                deviceNameLabel.TextColor = Color.Red;

                indicator.IsVisible = false;
                indicator.IsRunning = false;
                this.IsBusy = false;

                await Application.Current.MainPage.DisplayAlert("Error", "Device is not available in database.. \nPlease contact system administrator", "OK").ConfigureAwait(true);
            }
            else if (responce.Contains("Device Already Registered"))
            {
                deviceNameLabel.Text = $"\tDevice Already Registered";
                deviceNameLabel.TextColor = Color.Red;

                indicator.IsVisible = false;
                indicator.IsRunning = false;
                this.IsBusy = false;

                await Application.Current.MainPage.DisplayAlert("Error", "Device already registered", "OK").ConfigureAwait(true);
            }
            else if (responce.Contains("HtlCode"))
            {

                layout.Children.Remove(formLayout);

                List<HotelModel> hotelList = ConfigDeserializer.DeserializeHotels(responce);
                HotelModel selected = new HotelModel();

                if (hotelList.Count > 1)
                {
                    var action = await Application.Current.MainPage.DisplayActionSheet(
                    "Select your hotel.",
                    "Cancel",
                    null,
                    hotelList.Select(i => i.HtlName).ToArray());

                    selected = hotelList.Where(i => i.HtlName.ToString() == action).FirstOrDefault();

                    deviceNameLabel.TextColor = Color.Green;
                    hotelNameLabel.Text = $"\tHotel Name\t\t\t\t : {action}";
                    Settings.IsDeviceRegistered = true;
                }
                else if (hotelList.Count == 1)
                {
                    selected = hotelList[0];
                    hotelNameLabel.Text = $"\tHotel Name\t\t\t\t : {hotelList[0].HtlName}";
                    Settings.IsDeviceRegistered = true;
                }

                DeviceInfoModel deviceInfo = await CheckDeviceInfo(uuidEditor.Text.ToLower());

                if (deviceInfo!=null)
                {
                    if (deviceInfo.IsResgistered == "true")
                    {
                        deviceNameLabel.Text = $"\tDevice Name\t\t\t\t : {deviceInfo.Notes} ### (Registered)";
                        deviceNameLabel.TextColor = Color.Green;
                        Settings.DeviceName = $"{deviceInfo.Notes} ### (Registered)";

                        indicator.IsVisible = false;
                        indicator.IsRunning = false;
                        this.IsBusy = false;

                        await Application.Current.MainPage.DisplayAlert("Success!", "Device Registered Successfully", "OK").ConfigureAwait(true);
                    }
                    else
                    {
                        deviceNameLabel.Text = $"\tDevice Not Registered";
                        deviceNameLabel.TextColor = Color.Red;
                    }
                }

                ConfigModel responceConfigs = await ConfigDeserializer.DeserializeConfigurations();

                SaveConfigSettings(responceConfigs, responce , selected);
            }

            else
            {
                await Application.Current.MainPage.DisplayAlert("Unknown Error", "Bad Request", "OK").ConfigureAwait(true);
            }

        }

        private void SaveConfigSettings(ConfigModel configModel, string hotelJson, HotelModel hotelModel)
        {
            Settings.HotelCode = hotelModel.HtlCode;
            Settings.HotelName = hotelModel.HtlName;
            Settings.HotelNumber = hotelModel.TmsId;
            Settings.BaseDomainURL = configModel.APIM_BaseURI;
            Settings.SubscriptionKey = configModel.APIMSubKey;
            Settings.FTPUri = configModel.FTPLocationURI;
            Settings.HotelList = hotelJson;
            Settings.DeviceCurrentUUID = uuidEditor.Text.ToLower();

            if(configModel.SendErrorByEmail=="true")
            {
                Settings.EmailServiceStat = true;
                Console.WriteLine(Settings.EmailServiceStat);
            }
            else
            {
                Settings.EmailServiceStat = false;
                Console.WriteLine(Settings.EmailServiceStat);
            }


            if (configModel.TakePhoto == "true")
            {
                Settings.AVStreamStat = true;
                Console.WriteLine(Settings.AVStreamStat);
            }
            else
            {
                Settings.AVStreamStat = false;
                Console.WriteLine(Settings.AVStreamStat);
            }

            Debug.WriteLine(
                "\nHotel Code :" + Settings.HotelCode +
                "\nHotel Name :" + Settings.HotelName +
                "\nHotel Number :" + Settings.HotelNumber +
                "\nBase Domain URL :" + Settings.BaseDomainURL +
                "\nSubscription Key :" + Settings.SubscriptionKey +
                "\nFTP Uri :" + Settings.FTPUri +
                "\nApp Version :" + Settings.AppVersion +
                "\nAV Stream Stat :" + Settings.AVStreamStat +
                "\nDevice Name :" + Settings.DeviceName +
                "\nIs Device Registered :" + Settings.IsDeviceRegistered +
                "\nEmail Service Stat :" + Settings.EmailServiceStat +
                "\nUUID :" + "39e3ba5743f5741b9aaa5b930f0f5ed5040aba24"
            );

            Console.WriteLine(hotelJson);

        }

        private async Task<DeviceInfoModel> CheckDeviceInfo(string uuid)
        {
            var responce = await ConfigurationAPIServices.GetDeviceInformation(uuid);

            if(responce != null)
            {
                DeviceInfoModel deviceInfoModel = ConfigDeserializer.DeserializeDeviceInfo(responce);

                if(deviceInfoModel != null)
                {
                    return deviceInfoModel;
                }

                return null;
            }

            return null;
        }
	}

}
