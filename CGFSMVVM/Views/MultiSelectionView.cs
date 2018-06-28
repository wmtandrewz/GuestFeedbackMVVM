using System;
using System.Collections.Generic;
using CGFSMVVM.ViewModels;
using Xamarin.Forms;

namespace CGFSMVVM.Views
{
    public class MultiSelectionView : ContentPage
    {

        private StackLayout _baseLayout;
        private Image _headerImage;
        private Label _questionLabel,_messageLabel;
        private MultiSelectionsViewModel multiSelectionsViewModel;

        private string _prevQuesIndex;
        private string _currQuesIndex;

        public MultiSelectionView(string prevQuesIndex, string currQuesIndex)
        {

            this._prevQuesIndex = prevQuesIndex;
            this._currQuesIndex = currQuesIndex;

            InitUI();

            NavigationPage.SetHasNavigationBar(this,false);
            multiSelectionsViewModel=new MultiSelectionsViewModel(Navigation, currQuesIndex);
            BindingContext = multiSelectionsViewModel;

            multiSelectionsViewModel.LoadQuestionCommand.Execute(_questionLabel);
            multiSelectionsViewModel.LoadMessageTextCommand.Execute(_messageLabel);
            multiSelectionsViewModel.LoadOptionsDescCommand.Execute(null);

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
                Margin = 20
            };

            _baseLayout.Children.Add(_headerImage);
            _baseLayout.Children.Add(_questionLabel);


            List<string> _optionList = new List<string>
            {
                "Selection 1",
                "Selection 2",
                "Selection 3",
                "Selection 4",
            };

            ComponentMultiSelections cmp = new ComponentMultiSelections();
            StackLayout sl1 = cmp.GetMultiSelectionsLayout(_optionList);

            ComponentNavPane cnp = new ComponentNavPane();
            RelativeLayout sl2 = cnp.GetNavPane();
            _messageLabel = cnp.GetMessageLabel();
            sl2.VerticalOptions = LayoutOptions.EndAndExpand;

            ComponentProgressPane cpp = new ComponentProgressPane();
            StackLayout sl3 = cpp.GetProgressPane();

            _baseLayout.Children.Add(sl1);
            _baseLayout.Children.Add(sl2);
            _baseLayout.Children.Add(sl3);

            Content = _baseLayout;
        }
    }
}
