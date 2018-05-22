
using System;
using CGFSMVVM.Services;
using CGFSMVVM.ViewModels;
using Xamarin.Forms;


namespace CGFSMVVM
{
   
    public partial class FinishPageView : ContentPage
    {

        private StackLayout _layout;
        private Image _titleImage;
        private Label _thankLabel,_welcomeLabel,_titleLabel,_copyrightLabel;
        private Button _finishButton;

        FinishPageViewModel finishPageViewModel;

        public FinishPageView()
        {
            BackgroundColor = Color.Black;
            InitComp(); 

            NavigationPage.SetHasNavigationBar(this, false);

            finishPageViewModel = new FinishPageViewModel(Navigation);
            BindingContext = finishPageViewModel;
        }


        private void InitComp()
        {
            _layout = new StackLayout
            {
                BackgroundColor = Color.Black,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            _titleImage = new Image { Aspect = Aspect.AspectFit };
            _titleImage.Source = ImageSource.FromFile("Images/lifestyle.png");
            _titleImage.HeightRequest = 300;

            _thankLabel = new Label
            {
                Text = "Thank You!",
                FontSize = 40,
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = Color.White,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                HeightRequest = 100

            };

             _welcomeLabel = new Label
            {
                Text = "Have a wonderful day!",
                FontSize = 24,
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = Color.White,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                HeightRequest = 200

            };

             _titleLabel = new Label
            {
                Text = "We value your feedback & thank you very much for selecting Cinnamon.",
                FontSize = 24,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.White,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                HeightRequest = 300

            };

             _copyrightLabel = new Label
            {
                Text = "Designed and developed by Cinnamon IT | All Rights Reserved. Copyright © 2018",
                FontSize = 12,
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = Color.DarkGray,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                HeightRequest = 200

            };

            _finishButton = new Button
            {
                Text = "Finish",
                FontSize = 24,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                HeightRequest = 60,
                WidthRequest = 150
            };
            _finishButton.SetBinding(Button.CommandProperty, "FinishButtonCommand");


            _layout.Children.Add(_thankLabel);
            _layout.Children.Add(_welcomeLabel);
            _layout.Children.Add(_titleImage);
            _layout.Children.Add(_titleLabel);
            _layout.Children.Add(_finishButton);
            _layout.Children.Add(_copyrightLabel);

            _layout.Padding = 10;
            _layout.Spacing = 10;

            Content = _layout;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var endTime = DateTime.Now.ToString();
            FeedbackCart._endTime = endTime;

            finishPageViewModel.PageAppearingCommand.Execute(_finishButton);

        }

    }

}