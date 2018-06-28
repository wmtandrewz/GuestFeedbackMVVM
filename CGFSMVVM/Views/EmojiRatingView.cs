using System;
using System.Collections.Generic;
using CGFSMVVM.ViewModels;
using Xamarin.Forms;

namespace CGFSMVVM.Views
{
    public class EmojiRatingView : ContentPage
    {
        private ScrollView _scrollView;
        private StackLayout _baseLayout,_childLayout;
        private Image _headerImage;
        private Label _questionLabel,_messageLabel;

        private EmojiRatingViewModel emojiRatingViewModel;

        private string _prevQuesIndex;
        private string _currQuesIndex;

        public EmojiRatingView(string prevQuesIndex, string currQuesIndex)
        {
            this._prevQuesIndex = prevQuesIndex;
            this._currQuesIndex = currQuesIndex;

            InitUI();

            NavigationPage.SetHasNavigationBar(this,false);

            emojiRatingViewModel = new EmojiRatingViewModel(Navigation,currQuesIndex,_childLayout,_scrollView);
            BindingContext = emojiRatingViewModel;

            emojiRatingViewModel.LoadQuestionCommand.Execute(_questionLabel);
            emojiRatingViewModel.LoadChildQuestionCommand.Execute(null);
            emojiRatingViewModel.LoadMessageTextCommand.Execute(_messageLabel);

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
                Margin = new Thickness(20, 20, 20, 60)
            };

            _baseLayout.Children.Add(_headerImage);
            _baseLayout.Children.Add(_questionLabel);


            List<string> emojiIconList = new List<string>
            {
                "Images/angry.png",
                "Images/sad.png",
                "Images/shy.png",
                "Images/surprise.png",
                "Images/love.png"
            };

            List<string> emojiDescList = new List<string>
            {
                "Highly Dissatisfied",
                "Dissatisfied",
                "Neutral",
                "Satisfied",
                "Highly Satisfied"
            };

            ComponentEmoji ecv = new ComponentEmoji();
            StackLayout sl = ecv.EmojiCompLayout(emojiIconList,emojiDescList);

            ComponentNavPane npv = new ComponentNavPane();
            RelativeLayout sl2 = npv.GetNavPane();
            _messageLabel = npv.GetMessageLabel();
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

            emojiRatingViewModel.LoadQuestionCommand.Execute(_questionLabel);
        }
    }
}
