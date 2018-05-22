﻿using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using CGFSMVVM.DataParsers;
using CGFSMVVM.Helpers;
using CGFSMVVM.Models;
using CGFSMVVM.Services;
using CGFSMVVM.ViewModels;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace CGFSMVVM.Views
{
    public class MainView : ContentPage
    {

        private StackLayout _baseLayout;
        private Image _headerImage,_settingsIcon;
        private Label _welcomeLabel,_titleLabel,_copyrightLabel;
        private Button _startButton;
        private ActivityIndicator _indicator;

		private bool RegistrationCheckLock = false;

        MainViewModel _mainViewModel;

        public MainView()
        {
            InitUI();
            _mainViewModel = new MainViewModel(Navigation);
            BindingContext = _mainViewModel;
            NavigationPage.SetHasNavigationBar(this, false);

        }

        private void InitUI()
        {

            _baseLayout = new StackLayout()
            {
                BackgroundColor = Color.FromRgb(0, 0, 0),
                VerticalOptions = LayoutOptions.Fill,
                Padding = new Thickness(10,50,10,50)
            };

            _settingsIcon = new Image
            {
                Aspect = Aspect.AspectFit,
                Source = ImageSource.FromFile("Images/SettingsIcon.png"),
                HeightRequest = 20,
                WidthRequest = 50,
                HorizontalOptions = LayoutOptions.End
            };
            TapGestureRecognizer _tapGestureRecognizer = new TapGestureRecognizer();
            _tapGestureRecognizer.SetBinding(TapGestureRecognizer.CommandProperty, "SettingsTappedCommand");
            _settingsIcon.GestureRecognizers.Add(_tapGestureRecognizer);

            _headerImage = new Image
            {
                Aspect = Aspect.AspectFit,
                Source = ImageSource.FromFile("Images/cinnamon.png"),
                HeightRequest = 150,
                HorizontalOptions = LayoutOptions.Center
            };

            _welcomeLabel = new Label
            {
                Text="Welcome!",
                FontSize = 60,
                HorizontalTextAlignment=TextAlignment.Center,
                TextColor=Color.White,
                VerticalOptions=LayoutOptions.Center,
                HorizontalOptions=LayoutOptions.Center,
                HeightRequest=300,
                Margin = 40
            };

            _titleLabel = new Label
            {
                Text = "Tell us about your experience at Cinnamon",
                FontSize = 30,
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = Color.White,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                HeightRequest = 300,
                Margin = 20
            };

            _indicator = new ActivityIndicator()
            {
                IsVisible = false,
                Color = Color.Green,
                IsRunning = true,
                BindingContext = this,

            };

            _copyrightLabel = new Label
            {
                Text = "Designed and developed by Cinnamon IT | All Rights Reserved. Copyright © 2018",
                FontSize = 12,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.DarkGray,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                HeightRequest = 200

            };

            _startButton = new Button
            {
                Text = "Start",
                FontSize = 30,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                HeightRequest = 100,
                WidthRequest = 100,
            };
            _startButton.SetBinding(Button.CommandProperty, new Binding("LaunchStartButtonCommand"));

            _baseLayout.Children.Add(_settingsIcon);
            _baseLayout.Children.Add(_headerImage);
            _baseLayout.Children.Add(_welcomeLabel);
            _baseLayout.Children.Add(_titleLabel);
            _baseLayout.Children.Add(_indicator);
            _baseLayout.Children.Add(_startButton);
            _baseLayout.Children.Add(_copyrightLabel);

            Content = _baseLayout;
        }
        
        protected override async void OnAppearing()
        {

			bool isConnected = CrossConnectivity.Current.IsConnected;

			if (isConnected)
			{
				_mainViewModel.CheckAppVersionCommand.Execute(null);

				AccessQuestionAPI();

				CheckIsRegistered();
			}
			else
			{
				await Application.Current.MainPage.DisplayAlert("No Internet Connection", "Please check your internet connection", "OK").ConfigureAwait(true);
				Thread.CurrentThread.Abort();
               
			}

			base.OnAppearing();
		}
       

        private async void AccessQuestionAPI()
        {
            
            this.IsBusy = true;
            _indicator.IsVisible = true;
            _indicator.IsRunning = true;

            _startButton.IsEnabled = false;
                                       

			bool isCompletedQ = await QuestionJsonDeserializer.DeserializeQuestions().ConfigureAwait(true);


            if (isCompletedQ)
            {
                _indicator.IsVisible = false;
                _indicator.IsRunning = false;

                _startButton.IsEnabled = true;
                
			}
			else
			{
				CheckAppConfigurations();
			}

        }

        private async void CheckAppConfigurations()
		{
			RegistrationCheckLock = true;

			if(string.IsNullOrEmpty(Settings.DeviceCurrentUUID) || string.IsNullOrEmpty(Settings.HotelCode))
			{
				Debug.WriteLine("CheckAppConfigurations => Re-register");

				await Application.Current.MainPage.DisplayAlert("Device Not Registered", "Please register the device", "OK").ConfigureAwait(true);
                await Navigation.PushAsync(new Login());
			}

		}

		private async void CheckIsRegistered()
		{
			try
			{
				if (!RegistrationCheckLock)
				{
					var responce = await ConfigurationAPIServices.GetDeviceInformation(Settings.DeviceCurrentUUID);

					if (!string.IsNullOrEmpty(responce))
					{
						DeviceInfoModel deviceInfoModel = ConfigDeserializer.DeserializeDeviceInfo(responce);

						if (deviceInfoModel != null)
						{
							var isRegistered = Convert.ToBoolean(deviceInfoModel.IsResgistered);

							if (!isRegistered)
							{
								Debug.WriteLine("CheckIsRegistered => Re-register");

								Settings.IsDeviceRegistered = false;
								await Application.Current.MainPage.DisplayAlert("App has been reset", "Please register the device", "OK").ConfigureAwait(true);
								await Navigation.PushAsync(new Login());
							}
							else
							{
								Debug.WriteLine("CheckIsRegistered => Registered");
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Analytics.TrackEvent($"MainView.CheckIsRegistered {ex}");
			}

		}
                
    }
}
