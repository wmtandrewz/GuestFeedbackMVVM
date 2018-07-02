using CGFSMVVM.Behaviors;
using CGFSMVVM.ViewModels;
using Xamarin.Forms;

namespace CGFSMVVM.Views
{
    
    public partial class ContactDetailsView : ContentPage
    {

        Entry _guestPhoneNumEditor, _guestMailEditor;
        Label _guestPhoneLabel,_guestMailLabel,_titleLabel, _policyTitle, _privacyText, _agreeText;
        StackLayout _baseLayout, _formLayout, _agreeLayer;
        Switch _agreeSwitch;
        Image _titleImage;
        ContactDetailsViewModel contactDetailsViewModel;

        public ContactDetailsView()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            contactDetailsViewModel = new ContactDetailsViewModel(Navigation);

            BindingContext = contactDetailsViewModel;

            InitComp();

            BackgroundColor = Color.Black;

          }


        void InitComp()
        {
            _baseLayout = new StackLayout
            {
                BackgroundColor = Color.Black,
                VerticalOptions = LayoutOptions.StartAndExpand
            };

            _titleImage = new Image { Aspect = Aspect.AspectFit };
            _titleImage.Source = ImageSource.FromFile("Images/cinnamon.png");
            _titleImage.HeightRequest = 150;

            _policyTitle = new Label
            {
                Text = "Pivacy policy | EU - GDPR",
                FontSize = 24,
                FontAttributes = FontAttributes.Bold,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.FromHex("#008FBE"),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                HeightRequest = 60,
                Margin = new Thickness(10, 10, 10, 10)

            };

            _privacyText = new Label
            {
                Text = "\tI confirm that I will be providing the Hotel with my personal information and data and " +
                    "I hereby expressly consents to the use of such information and data for the purposes of the booking made with the Hotel. " +
                    "This includes express permission to share the personal data and information with the Hotel’s service providers and agents.\n " +
                    "I also further expressly consent to the use of my personal data to promote the products and services of the Hotel and the Cinnamon Group." +
                    "I confirm that I have read through the hotel’s data policy and have understood my rights in relation to the personal data which I am providing the Hotel.",
                FontSize = 20,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.White,
                Margin = new Thickness(10,10,10,10)
            };

            _agreeLayer = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                BackgroundColor = Color.Black,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                HeightRequest = 50,
                Margin = new Thickness(10, 10, 10, 10)
            };

            _agreeSwitch = new Switch()
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };

            _agreeText = new Label
            {
                Text = "I agree",
                FontSize = 24,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.White,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                WidthRequest = 150

            };

            _agreeLayer.Children.Add(_agreeText);
            _agreeLayer.Children.Add(_agreeSwitch);

            _titleLabel = new Label
            {
                Text = "Please provide your contact details",
                FontSize = 30,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.White,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                HeightRequest = 60,
                IsVisible = false

            };

            _formLayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.StartAndExpand,
                IsVisible = false
            };

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

            _guestPhoneNumEditor.Behaviors.Add(new EventToCommandBehavior
            {
                EventName = "TextChanged",
                Command = contactDetailsViewModel.MobileTextChangedCommand,
                CommandParameter = _guestPhoneNumEditor
            });

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

            _guestMailEditor.Behaviors.Add(new EventToCommandBehavior
            {
                EventName = "TextChanged",
                Command = contactDetailsViewModel.MailTextChangedCommand,
                CommandParameter = _guestMailEditor
            });

            ComponentNavPane npv = new ComponentNavPane();
            RelativeLayout rl = npv.GetNavPane();
            rl.VerticalOptions = LayoutOptions.EndAndExpand;

            _baseLayout.Children.Add(_titleImage);
            _baseLayout.Children.Add(_policyTitle);
            _baseLayout.Children.Add(_privacyText);
            _baseLayout.Children.Add(_agreeLayer);
            _baseLayout.Children.Add(_titleLabel);
            _formLayout.Children.Add(_guestPhoneLabel);
            _formLayout.Children.Add(_guestPhoneNumEditor);
            _formLayout.Children.Add(_guestMailLabel);
            _formLayout.Children.Add(_guestMailEditor);

            _formLayout.Padding = new Thickness(150, 10, 150, 10);

            _baseLayout.Children.Add(_formLayout);
            _baseLayout.Children.Add(rl);

            _baseLayout.Padding = 10;
            _baseLayout.Spacing = 10;

            Content = new ScrollView
            {
                Orientation = ScrollOrientation.Vertical,
                Content = _baseLayout
            };

            _agreeSwitch.Toggled += delegate
            {
                if (_agreeSwitch.IsToggled)
                {
                    _titleLabel.IsVisible = true;
                    _formLayout.IsVisible = true;
                    _guestPhoneNumEditor.Focus();
                }
                else
                {
                    _titleLabel.IsVisible = false;
                    _formLayout.IsVisible = false;
                }
            };
        }

		protected override void OnAppearing()
		{
            base.OnAppearing();

		}

	}

}