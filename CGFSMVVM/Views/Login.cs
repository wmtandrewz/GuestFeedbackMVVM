using Xamarin.Forms;

namespace CGFSMVVM.Views
{
    
    public partial class Login : ContentPage
    {
        Entry userNameEntry;

        public Login()
        {
            InitComp(); //  Execute the method 'InitComp' for initialize User Interface elements
            BackgroundColor = Color.Black;

        }



        private void InitComp()
        {
            var layout = new StackLayout();
            layout.BackgroundColor = Color.Black;
            layout.VerticalOptions = LayoutOptions.CenterAndExpand;

            var titleImage = new Image { Aspect = Aspect.AspectFit };
            titleImage.Source = ImageSource.FromFile("images/cinnamon.png");
            titleImage.HeightRequest = 100;

            var titleLabel = new Label
            {
                Text = "Login to Continue",
                FontSize = 36,
                TextColor = Color.White,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                VerticalTextAlignment = TextAlignment.Center,
                HeightRequest = 100

            };

            var formLayout = new StackLayout();
            formLayout.BackgroundColor = Color.DarkSlateGray;
            formLayout.Orientation = StackOrientation.Vertical;
            formLayout.VerticalOptions = LayoutOptions.Center;

            var userNameLabel = new Label
            {
                Text = "User Name",
                FontSize = 18,
                TextColor = Color.White,
                VerticalOptions = LayoutOptions.Start,
                VerticalTextAlignment = TextAlignment.End,
                HeightRequest = 40

            };


            userNameEntry = new Entry
            {
                Placeholder = "User Name",
                HeightRequest = 40,
                TextColor = Color.Black,
            };

            var passwordLabel = new Label
            {
                Text = "Password",
                FontSize = 18,
                TextColor = Color.White,
                VerticalOptions = LayoutOptions.Start,
                VerticalTextAlignment = TextAlignment.End,
                HeightRequest = 40,

            };

            var passwordEditor = new Entry
            {
                IsPassword = true,
                Placeholder = "Password",
                HeightRequest = 40,
                TextColor = Color.Black,
                Keyboard = Keyboard.Numeric
            };

            var submitButton = new Button
            {
                Text = "Login",
                FontSize = 36,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                HeightRequest = 60,
                WidthRequest = 150
            };


            submitButton.Clicked += async delegate {

                if (userNameEntry.Text == "Admin" && passwordEditor.Text == "it@cinnamon")
                {
                    await Navigation.PushAsync(new AutoConfigView());
                }
                else
                {
                    await DisplayAlert("Authorization Failed!", "Please check your username or password", "OK");
                    userNameEntry.Text = "";
                    passwordEditor.Text = "";
                    userNameEntry.Focus();
                }


            };

            formLayout.Children.Add(titleLabel);

            formLayout.Children.Add(userNameLabel);
            formLayout.Children.Add(userNameEntry);
            formLayout.Children.Add(passwordLabel);
            formLayout.Children.Add(passwordEditor);
            formLayout.Children.Add(submitButton);

            formLayout.Padding = new Thickness(150, 10, 150, 10);

            layout.Children.Add(formLayout);


            layout.Padding = 100;
            layout.Spacing = 10;

            Content = layout; // Sets dynamic layout to page 'Content' Property
        }
        //---------------------------------------------- End Of InitComp --------------------------------------
        protected override void OnAppearing()
        {
            base.OnAppearing();
            userNameEntry.Focus();

        }
    }
}