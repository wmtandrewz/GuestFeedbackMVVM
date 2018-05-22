
using CGFSMVVM.Behaviors;
using CGFSMVVM.Helpers;
using CGFSMVVM.ViewModels;
using Xamarin.Forms;

namespace CGFSMVVM
{
    
    public partial class GuestDetailsView : ContentPage
    {

        Entry _roomNumberEditor, _reservationNumberEditor, _guestPhoneNumEditor, _guestMailEditor;
        Label _reservationNumberLabel, _guestPhoneLabel, _roomNumberLabel,_guestMailLabel;
        StackLayout _baseLayout, _formLayout;
        Image _titleImage;
        ActivityIndicator _indicator;
        GuestDetailsViewModel guestDetailsViewModel;

        public GuestDetailsView()
        {
            //NavigationPage.SetHasNavigationBar(this, false);
            guestDetailsViewModel = new GuestDetailsViewModel(Navigation);

            BindingContext = guestDetailsViewModel;

            InitComp();

            BackgroundColor = Color.Black;

            guestDetailsViewModel.SetFormLayoutInstance.Execute(_formLayout);

          }


        void InitComp()
        {
            _baseLayout = new StackLayout
            {
                BackgroundColor = Color.Black,
                VerticalOptions = LayoutOptions.StartAndExpand
            };

            _titleImage = new Image { Aspect = Aspect.AspectFit };
            _titleImage.Source = ImageSource.FromFile($"Images/banners/{Settings.HotelNumber}.jpg");
            _titleImage.HeightRequest = 150;

            _formLayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.StartAndExpand
            };

            //_hotelLabel = new Label
            //{
            //    Text = "Hotel",
            //    FontSize = 18,
            //    TextColor = Color.White,
            //    VerticalOptions = LayoutOptions.Start,
            //    VerticalTextAlignment = TextAlignment.End,
            //    HeightRequest = 40

            //};

            //_hotelNameEntry = new Entry
            //{
            //    Text = "Cinnamon",
            //    IsEnabled = false,
            //    TextColor = Color.Black,
            //    HeightRequest = 40,
            //    Keyboard = Keyboard.Default
            //};

            _roomNumberLabel = new Label
            {
                Text = "Room No:",
                FontSize = 18,
                TextColor = Color.White,
                VerticalOptions = LayoutOptions.Start,
                VerticalTextAlignment = TextAlignment.End,
                HeightRequest = 40,

            };

            _roomNumberEditor = new Entry
            {
                Placeholder = "Room Number",
                TextColor = Color.Black,
                HeightRequest = 40,
                Keyboard = Keyboard.Numeric,
            };
            _roomNumberEditor.Behaviors.Add(new EventToCommandBehavior
            {
                EventName = "Unfocused",
                Command = guestDetailsViewModel.EntryUnfocusedCommand,
                CommandParameter = _roomNumberEditor
            });
            _roomNumberEditor.Behaviors.Add(new EventToCommandBehavior
            {
                EventName = "Completed",
                Command = guestDetailsViewModel.EntryCompletedCommand,
                CommandParameter = _roomNumberEditor
            });
            _roomNumberEditor.Behaviors.Add(new EventToCommandBehavior
            {
                EventName = "TextChanged",
                Command = guestDetailsViewModel.EntryTextChangedCommand,
                CommandParameter = _roomNumberEditor
            });


            _indicator = new ActivityIndicator()
            {
                IsVisible = false,
                Color = Color.Green,
                IsRunning = true,
                BindingContext = this,

            };

            _reservationNumberLabel = new Label
            {
                Text = "Reservation No:",
                FontSize = 18,
                TextColor = Color.White,
                VerticalOptions = LayoutOptions.Start,
                VerticalTextAlignment = TextAlignment.End,
                HeightRequest = 40,

            };

            _reservationNumberEditor = new Entry
            {
                Placeholder = "Reservation Number",
                HeightRequest = 40,
                TextColor = Color.Black,
                IsEnabled = false
            };
            _reservationNumberEditor.SetBinding(Entry.TextProperty, "ReservationNumber");

            _guestPhoneLabel = new Label
            {
                Text = "Mobile Phone Number",
                FontSize = 18,
                TextColor = Color.White,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center,
                VerticalOptions = LayoutOptions.Start,
                HeightRequest = 40,
            };

            _guestPhoneNumEditor = new Entry
            {
                Placeholder = "Your Mobile Number",
                HeightRequest = 40,
                Keyboard = Keyboard.Telephone
            };
            _guestPhoneNumEditor.SetBinding(Entry.TextProperty, "MobileNumber");

            _guestMailLabel = new Label
            {
                Text = "E Mail Address",
                FontSize = 18,
                TextColor = Color.White,
                VerticalOptions = LayoutOptions.Start,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center,
                HeightRequest = 40,
            };

            _guestMailEditor = new Entry
            {
                Placeholder = "Your e mail",
                HeightRequest = 40,
                Keyboard = Keyboard.Email
            };
            _guestMailEditor.SetBinding(Entry.TextProperty, "EmailAddress");

            _baseLayout.Children.Add(_titleImage);
            _baseLayout.Children.Add(_indicator);

            //_formLayout.Children.Add(_hotelLabel);
            //_formLayout.Children.Add(_hotelNameEntry);
            _formLayout.Children.Add(_roomNumberLabel);
            _formLayout.Children.Add(_roomNumberEditor);
            _formLayout.Children.Add(_reservationNumberLabel);
            _formLayout.Children.Add(_reservationNumberEditor);
            _formLayout.Children.Add(_guestPhoneLabel);
            _formLayout.Children.Add(_guestPhoneNumEditor);
            _formLayout.Children.Add(_guestMailLabel);
            _formLayout.Children.Add(_guestMailEditor);

            _formLayout.Padding = new Thickness(50, 10, 50, 10);

            _baseLayout.Children.Add(_formLayout);

            _baseLayout.Padding = 10;
            _baseLayout.Spacing = 10;

            Content = new ScrollView
            {
                Orientation = ScrollOrientation.Horizontal,
                Content = _baseLayout
            };
        }

		protected override void OnAppearing()
		{
            base.OnAppearing();

            _roomNumberEditor.Focus();
		}

	}

}