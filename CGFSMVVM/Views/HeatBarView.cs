using System;
using System.Collections.Generic;
using CGFSMVVM.ViewModels;
using Xamarin.Forms;

namespace CGFSMVVM.Views
{
    public class HeatBarView : ContentPage
    {
        private ScrollView _scrollView;
        private StackLayout _baseLayout, _childLayout;
        private Image _headerImage;
        private Label _questionLabel,_messageLabel;

        private HeatBarViewModel heatBarViewModel;

        private string _prevQuesIndex;
        private string _currQuesIndex;

        public HeatBarView(string prevQuesIndex, string currQuesIndex, int heatButtonCount)
        {
            this._prevQuesIndex = prevQuesIndex;
            this._currQuesIndex = currQuesIndex;

            InitUI(heatButtonCount);

            NavigationPage.SetHasNavigationBar(this, false);

            heatBarViewModel = new HeatBarViewModel(Navigation, currQuesIndex, _childLayout,_scrollView);
            BindingContext = heatBarViewModel;

            heatBarViewModel.LoadQuestionCommand.Execute(_questionLabel);
            heatBarViewModel.LoadChildQuestionCommand.Execute(null);
            heatBarViewModel.LoadMessageTextCommand.Execute(_messageLabel);

        }

        private void InitUI(int buttonCount)
        {

            _baseLayout = new StackLayout()
            {
                BackgroundColor = Color.FromRgb(0, 0, 0),
                VerticalOptions = LayoutOptions.Fill,
                Padding = 20
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
                HorizontalOptions=LayoutOptions.Center,
                Margin = new Thickness(20,20,20,60)
            };

            _baseLayout.Children.Add(_headerImage);
            _baseLayout.Children.Add(_questionLabel);

            ComponentHeatBar chb = new ComponentHeatBar();
            StackLayout sl = chb.GetHeatBarLayout(buttonCount);
            sl.VerticalOptions = LayoutOptions.StartAndExpand;

            ComponentNavPane cnp = new ComponentNavPane();
            RelativeLayout sl2 = cnp.GetNavPane();
            _messageLabel = cnp.GetMessageLabel();
            sl2.VerticalOptions = LayoutOptions.EndAndExpand;

            ComponentProgressPane cpp = new ComponentProgressPane();
            StackLayout sl3 = cpp.GetProgressPane();

            ComponentHeatBarList chbl = new ComponentHeatBarList();
            _childLayout = chbl.GetHeatBarListLayout(_currQuesIndex);
            _childLayout.IsVisible = false;

            _baseLayout.Children.Add(sl);
            _baseLayout.Children.Add(_childLayout);
            _baseLayout.Children.Add(sl2);
            _baseLayout.Children.Add(sl3);

            _scrollView = new ScrollView()
            {
                Content = _baseLayout,
                BackgroundColor = Color.Black,
                VerticalOptions = LayoutOptions.Fill
            };


            Content = _scrollView;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            heatBarViewModel.OnAppearingCommand.Execute(null);
        }
    }

}
