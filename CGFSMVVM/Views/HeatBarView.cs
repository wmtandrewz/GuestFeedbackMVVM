using System;
using System.Collections.Generic;
using CGFSMVVM.ViewModels;
using Xamarin.Forms;

namespace CGFSMVVM.Views
{
    public class HeatBarView : ContentPage
    {

        private StackLayout _baseLayout;
        private Image _headerImage;
        private Label _questionLabel,_messageLabel;

        private HeatBarViewModel heatBarViewModel;

        private string _prevQuesIndex;
        private string _currQuesIndex;

        public HeatBarView(string prevQuesIndex, string currQuesIndex)
        {
            this._prevQuesIndex = prevQuesIndex;
            this._currQuesIndex = currQuesIndex;

            InitUI();

            NavigationPage.SetHasNavigationBar(this, false);

            heatBarViewModel = new HeatBarViewModel(Navigation, currQuesIndex);
            BindingContext = heatBarViewModel;

            heatBarViewModel.LoadQuestionCommand.Execute(_questionLabel);
            heatBarViewModel.LoadChildQuestionCommand.Execute(_headerImage);
            heatBarViewModel.LoadMessageTextCommand.Execute(_messageLabel);

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
                Text="How satisfied are you",
                FontSize=30,
                HorizontalTextAlignment=TextAlignment.Center,
                TextColor=Color.White,
                VerticalOptions=LayoutOptions.Center,
                HorizontalOptions=LayoutOptions.Center,
                HeightRequest=300,
                Margin = new Thickness(20,50,20,10)
            };

            _baseLayout.Children.Add(_headerImage);
            _baseLayout.Children.Add(_questionLabel);

            ComponentHeatBar chb = new ComponentHeatBar();
            StackLayout sl = chb.GetHeatBarLayout();

            ComponentNavPane cnp = new ComponentNavPane();
            RelativeLayout sl2 = cnp.GetNavPane();
            _messageLabel = cnp.GetMessageLabel();

            ComponentProgressPane cpp = new ComponentProgressPane();
            StackLayout sl3 = cpp.GetProgressPane();

            ComponentHeatBarList chbl = new ComponentHeatBarList();
            StackLayout sl2_1 = chbl.GetHeatBarListLayout(_currQuesIndex);

            _baseLayout.Children.Add(sl);
            _baseLayout.Children.Add(sl2_1);
            _baseLayout.Children.Add(sl2);
            _baseLayout.Children.Add(sl3);

            Content = _baseLayout;
        }
    }
}
