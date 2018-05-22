using CGFSMVVM.ViewModels;
using Xamarin.Forms;

namespace CGFSMVVM.Views
{
    public class NewsLetterView : ContentPage
    {

        private StackLayout _baseLayout;
        private Image _headerImage;
        private Label _questionLabel,_messageLabel;

        private NewsLetterViewMoidel newsLetterViewModel;

        public NewsLetterView()
        {
            
            InitUI();

            NavigationPage.SetHasNavigationBar(this,false);
            newsLetterViewModel=new NewsLetterViewMoidel(Navigation);
            BindingContext = newsLetterViewModel;
        }

        private void InitUI()
        {

            _baseLayout = new StackLayout()
            {
                BackgroundColor = Color.FromRgb(0, 0, 0),
                VerticalOptions = LayoutOptions.Fill,
                Padding=20
            };

            _headerImage = new Image
            {
                Aspect = Aspect.AspectFit,
                Source = ImageSource.FromFile("Images/cinnamon.png"),
                HeightRequest = 150
            };

            _questionLabel = new Label
            {
                Text="Would you like to recieve Cinnamon promotions and news alerts?",
                FontSize=30,
                HorizontalTextAlignment=TextAlignment.Center,
                TextColor=Color.White,
                VerticalOptions=LayoutOptions.Center,
                HorizontalOptions=LayoutOptions.Center,
                HeightRequest=300,
                Margin = 20
            };

            _baseLayout.Children.Add(_headerImage);
            _baseLayout.Children.Add(_questionLabel);

            ComponentDualOptions componentDualOptions = new ComponentDualOptions();
            StackLayout sl = componentDualOptions.GetDualOptionLayout();

            ComponentNavPane npv = new ComponentNavPane();
            RelativeLayout sl2 = npv.GetNavPane();
            _messageLabel = npv.GetMessageLabel();

            _baseLayout.Children.Add(sl);
            _baseLayout.Children.Add(sl2);

            Content = _baseLayout;
        }
    }
}
