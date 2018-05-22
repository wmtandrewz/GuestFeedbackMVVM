using System;
using System.Collections.Generic;
using CGFSMVVM.ViewModels;
using Xamarin.Forms;

namespace CGFSMVVM.Views
{
    public class MultiOptionView : ContentPage
    {

        private StackLayout _baseLayout;
        private Image _headerImage;
        private Label _questionLabel,_messageLabel;
        private MultiOptionsViewModel multiOptionsViewModel;

        private string _prevQuesIndex;
        private string _currQuesIndex;

        public MultiOptionView(string prevQuesIndex,string currQuesIndex)
        {
            
            this._prevQuesIndex = prevQuesIndex;
            this._currQuesIndex = currQuesIndex;

            InitUI();

            NavigationPage.SetHasNavigationBar(this,false);

            multiOptionsViewModel = new MultiOptionsViewModel(Navigation,currQuesIndex);
            BindingContext = multiOptionsViewModel;

            multiOptionsViewModel.LoadQuestionCommand.Execute(_questionLabel);
            multiOptionsViewModel.LoadMessageTextCommand.Execute(_messageLabel);
            multiOptionsViewModel.LoadOptionsDescCommand.Execute(null);

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
                FontSize=30,
                HorizontalTextAlignment=TextAlignment.Center,
                TextColor=Color.White,
                VerticalOptions=LayoutOptions.Center,
                HorizontalOptions=LayoutOptions.Center,
                HeightRequest=300,
                Margin = 20
            };

            ComponentMultiOptions cmp = new ComponentMultiOptions();
            StackLayout sl1 = cmp.GetMultiOptionsLayout(Navigation,_currQuesIndex);

            ComponentNavPane cnp = new ComponentNavPane();
            RelativeLayout sl2 = cnp.GetNavPane();
            _messageLabel = cnp.GetMessageLabel();

            ComponentProgressPane cpp = new ComponentProgressPane();
            StackLayout sl3 = cpp.GetProgressPane();

            _baseLayout.Children.Add(_headerImage);
            _baseLayout.Children.Add(_questionLabel);
            _baseLayout.Children.Add(sl1);
            _baseLayout.Children.Add(sl2);
            _baseLayout.Children.Add(sl3);

            Content = _baseLayout;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

        }
    }
}
