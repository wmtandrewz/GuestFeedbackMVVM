using System;
using System.Collections.Generic;
using CGFSMVVM.ViewModels;
using Xamarin.Forms;

namespace CGFSMVVM.Views
{
    public class DualOptionView : ContentPage
    {

        private StackLayout _baseLayout;
        private Image _headerImage;
        private Label _questionLabel,_messageLabel;

        private DualOptionViewModel dualOptionViewModel;

        private string _prevQuesIndex;
        private string _currQuesIndex;

        public DualOptionView(string prevQuesIndex, string currQuesIndex)
        {

            this._prevQuesIndex = prevQuesIndex;
            this._currQuesIndex = currQuesIndex;

            InitUI();

            NavigationPage.SetHasNavigationBar(this,false);
            dualOptionViewModel=new DualOptionViewModel(Navigation, currQuesIndex);
            BindingContext = dualOptionViewModel;

            dualOptionViewModel.LoadQuestionCommand.Execute(_questionLabel);
            dualOptionViewModel.LoadMessageTextCommand.Execute(_messageLabel);

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
                Text="Did you use cinnamon gym?",
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
            sl2.VerticalOptions = LayoutOptions.EndAndExpand;

            ComponentProgressPane cpp = new ComponentProgressPane();
            StackLayout sl3 = cpp.GetProgressPane();

            _baseLayout.Children.Add(sl);
            _baseLayout.Children.Add(sl2);
            _baseLayout.Children.Add(sl3);

            Content = _baseLayout;
        }
    }
}
