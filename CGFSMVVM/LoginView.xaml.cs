using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CGFSMVVM.Helpers;
using CGFSMVVM.Models;
using CGFSMVVM.Services;
using CGFSMVVM.Views;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace CGFSMVVM
{
    public partial class LoginView : ContentPage
    {
		//Data Source
		SAPAccessService sapService = new SAPAccessService();
        string SettingsSaved = "";
		public LoginView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            //float scale = (float)NumericUpDown.valu3;

            //MessagingCenter.Subscribe<SettingsPage, String>(this, "settingsSaved", (sender, arg) =>
            //{
            //    SettingsSaved = "1";
            //});

        }

        //Login Button Clicked Event
        async void LoginButtonClickEvt(object sender, EventArgs e)
        {
            //Page Loding 
            pageLoading();

            //Check internet connection
            if (CrossConnectivity.Current.IsConnected)
            {
                //Check Settings are configured
                bool isConfigured = await SettingsChecker();
                if (isConfigured)
                {
                    var user = new UserModel();
                    try
                    {
                        //Check username and0 password is not null
                        user = new UserModel()
                        {
                            Username = LoginEntryUsername.Text,
                            Password = LoginEntryPassword.Text
                        };
                        if (LoginEntryUsername.Text.ToString() != "" && LoginEntryPassword.Text.ToString() != "")
                        {

                            await oAuthLogin(user);

                        }
                        else
                        {
                            stopLoading();
                            await DisplayAlert(Constants._headerMessage, Constants._providelogincredentials, Constants._buttonClose);
                        }
                    }
                    catch (Exception)
                    {
                        stopLoading();
                        await DisplayAlert(Constants._headerMessage, Constants._providelogincredentials, Constants._buttonClose);
                    }
                }
                else
                {
                    stopLoading();
                }
            }

            else
            {
                stopLoading();
                await DisplayAlert(Constants._headerConnectionLost, Constants._unableToCOnnectToInternet, Constants._buttonOkay);
            }


        }


        async Task hotelInformationAndLogin()
        {

            string result = await sapService.HotelCodeAsync();
            if (result.Contains("Enter your user ID in the format"))
            {
                await DisplayAlert(Constants._headerMessage, Constants._unableToGetHotelCode, Constants._buttonOkay);
            }
            else if (result == "")
            {
                await DisplayAlert(Constants._headerMessage, "Sorry.. Hotel Code is not assigned to your account", Constants._buttonOkay);
            }
            else
            {
                var jObj = JObject.Parse(result);

                //Setting Hotel Code
                Constants._hotel_code = jObj["d"]["ExXhotelId"].ToString();
                Settings.HotelCode = jObj["d"]["ExXhotelId"].ToString();

				switch(jObj["d"]["ExXhotelId"].ToString())
				{
					case  "3000" :
						Settings.HotelIdentifier = "CNG";
						break;

					case "3005":
                        Settings.HotelIdentifier = "CNL";
                        break;

					case "3015":
                        Settings.HotelIdentifier = "RED";
                        break;

					case "3100":
                        Settings.HotelIdentifier = "BBH";
                        break;

					case "3110":
                        Settings.HotelIdentifier = "CIT";
                        break;

					case "3115":
                        Settings.HotelIdentifier = "LOD";
                        break;

					case "3120":
                        Settings.HotelIdentifier = "VIL";
                        break;

					case "3150":
                        Settings.HotelIdentifier = "WLD";
                        break;

					case "3160":
                        Settings.HotelIdentifier = "BEY";
                        break;

					case "3165":
                        Settings.HotelIdentifier = "BLU";
                        break;

					case "3170":
                        Settings.HotelIdentifier = "TRA";
                        break;

					case "3300":
                        Settings.HotelIdentifier = "ELL";
                        break;

					case "3305":
                        Settings.HotelIdentifier = "HAK";
                        break;

					case "3310":
                        Settings.HotelIdentifier = "DHO";
                        break;
					
				}
                                
				await Navigation.PushAsync(new MainView());

                

                //Reset the text fields
                LoginEntryUsername.Text = "";
                LoginEntryPassword.Text = "";

            }

        }

        async Task oAuthLogin(UserModel user)
        {

            //Authenticate against ADFS and NW Gateway
            oAuthLogin oauthlogin = new oAuthLogin();
            user.Username = user.Username + "@jkintranet.com";
            String access_token = await oauthlogin.LoginUserAsync(user);


            if ((access_token != "" && access_token != Constants._userNotExistInNWGateway) && access_token != Constants._gatewayUrlError)
            {
                //Store Username and Password in global Area
                Constants._user = user;

                //Store Username and Password in local shared preference
                Settings.Username = user.Username;
                Settings.Password = user.Password;

                //Retrieving Hotel Code
                try
                {
                    await hotelInformationAndLogin();
                }
                catch (Exception)
                {
                    await DisplayAlert(Constants._headerConnectionLost, Constants._networkerror, Constants._buttonOkay);
                }
            }
            else if (access_token == Constants._gatewayUrlError)
            {
                await DisplayAlert(Constants._headerConnectionLost, Constants._gatewayUrlError, Constants._buttonOkay);
            }
            else
            {
                if (access_token == Constants._userNotExistInNWGateway)
                    await DisplayAlert(Constants._headerMessage, Constants._userNotExistInNWGateway, Constants._buttonClose);
                else
                    await DisplayAlert(Constants._headerWrongCredentials, Constants._incorrectlogincredentials, Constants._buttonClose);
            }


            stopLoading();
        }

        //Animation on page load
        protected override async void OnAppearing()
        {
            if (SettingsSaved == "")
            {
                double scale = this.Scale;

                LoginButton.Scale = (scale * 70 / 100);
                LoginEntryUsername.Scale = (scale * 70 / 100);
                LoginEntryPassword.Scale = (scale * 70 / 100);
                //Startup setup
                await CinnamonLogo.TranslateTo(0, 170, 500, Easing.Linear);

                //await CheckinText.TranslateTo(0, 100, 500, Easing.Linear);
                await LoginEntryUsername.TranslateTo(0, 100, 500, Easing.Linear);
                await LoginEntryPassword.TranslateTo(0, 100, 500, Easing.Linear);
                await LoginButton.TranslateTo(0, 100, 500, Easing.Linear);

                //Cinnamon Logo Animation
                await Sleep(500);
                await CinnamonLogo.TranslateTo(0, 0, 600, Easing.CubicOut);

                //Entry and Button Animation

                //CheckinText.FadeTo(1, 800, Easing.CubicOut);
                //CheckinText.TranslateTo(0, 0, 800, Easing.CubicOut);

                LoginEntryUsername.FadeTo(1, 800, Easing.CubicOut);
                LoginEntryUsername.TranslateTo(0, 0, 800, Easing.CubicOut);

                LoginEntryPassword.FadeTo(1, 800, Easing.CubicOut);
                LoginEntryPassword.TranslateTo(0, 0, 800, Easing.CubicOut);

                LoginButton.FadeTo(1, 800, Easing.CubicOut);
                LoginButton.TranslateTo(0, 0, 800, Easing.CubicOut);

                ForgotPasswordButton.FadeTo(1, 800, Easing.CubicOut);
                ForgotPasswordButton.TranslateTo(0, 0, 800, Easing.CubicOut);

                
                TradeMark.IsVisible = true;
                TradeMark1.IsVisible = true;

                Sleep(800);

                base.OnAppearing();
            }
            //Update Availability Indicator
            //VersionChecker();
        }

        public static async Task Sleep(int ms)
        {
            await Task.Delay(ms);
        }

        void pageLoading()
        {
            //Show Loading Message
            Device.BeginInvokeOnMainThread(() =>
            {
                LoginIndicator.IsVisible = true;
                LoginIndicator.IsRunning = true;
                LoginButton.IsEnabled = false;
            });
        }

        void stopLoading()
        {
            //Hide Loading Message
            Device.BeginInvokeOnMainThread(() =>
            {
                LoginIndicator.IsVisible = false;
                LoginIndicator.IsRunning = false;
                LoginButton.IsEnabled = true;
            });
        }


        //Forgot Password Clickeds
        void ForgotPasswordClickEvt(object sender, EventArgs e)
        {

            Device.OpenUri(new Uri("https://mypassword.keells.lk"));
        }

        //Forgot Password Clickeds
        void ContactUsClickedEvent(object sender, EventArgs e)
        {

            Device.OpenUri(new Uri("https://cinnamonhotels.freshdesk.com/support/home"));
        }

        //Help button clicked
        void NeedHelpButtonClickEvt(object sender, EventArgs e)
        {
            DisplayAlert(Constants._headerHowTo, Constants._help, Constants._buttonClose);
        }
        //Override Back Button Function
        protected override bool OnBackButtonPressed()
        {
            ExitCurrentApp();
            return true;
        }
        public void ExitCurrentApp()
        {
			//if (Device.OS == TargetPlatform.Android)
			//{
			//    DependencyService.Get<CloseCurrentApp>().ExitApp();
			//}

			Application.Current.Quit();
        }

        //One Approvals Settings
        async void SettingsButtonTappedEvt(object sender, EventArgs e)
        {
            //Settings Page Configuration
            //var settings_page = new NavigationPage(new SettingsPage());
            //settings_page.BarBackgroundColor = Color.FromHex("#660099");
            //settings_page.BarTextColor = Color.White;
            //await Navigation.PushModalAsync(settings_page);
        }

        //Check whether settings are configured
        public async Task<bool> SettingsChecker()
        {
            if (Settings.SettingsSAPURL.Equals("") || Settings.SettingsSAPCookie.Equals(""))
            {
                //Icon Animation
                await SettingsIcon.ScaleTo(2, 100, Easing.CubicIn);
                await SettingsIcon.ScaleTo(1, 100, Easing.CubicOut);
                await SettingsIcon.ScaleTo(2, 100, Easing.CubicIn);
                await SettingsIcon.ScaleTo(1, 100, Easing.CubicOut);
                await SettingsIcon.ScaleTo(2, 100, Easing.CubicIn);
                await SettingsIcon.ScaleTo(1, 100, Easing.CubicOut);
                await DisplayAlert(Constants._headerMessage, "Settings not Configured, Please configure the settings to continue", Constants._buttonClose);
                return false;
            }
            else
            {
                Constants._gatewayURL = Settings.SettingsSAPURL;
                Constants._cookie = Settings.SettingsSAPCookie;
                return true;
            }
        }
    }
}
