using CGFSMVVM.Behaviors;
using CGFSMVVM.ViewModels;
using Xamarin.Forms;

namespace CGFSMVVM.Views
{
    
    public partial class ContactDetailsView : ContentPage
    {

        Entry _guestPhoneNumEditor, _guestMailEditor;
        Label _guestPhoneLabel,_guestMailLabel,_titleLabel;
        StackLayout _baseLayout, _formLayout;
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

            _titleLabel = new Label
            {
                Text = "Please provide your contact details",
                FontSize = 24,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                TextColor = Color.White,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                HeightRequest = 300

            };

            _formLayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.StartAndExpand
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

            _baseLayout.Children.Add(_titleImage);
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
                Orientation = ScrollOrientation.Horizontal,
                Content = _baseLayout
            };
        }

		protected override void OnAppearing()
		{
            base.OnAppearing();

            _guestPhoneNumEditor.Focus();
		}

	}

}