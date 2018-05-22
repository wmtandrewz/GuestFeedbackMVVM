using System;
using System.Collections.Generic;
using CGFSMVVM.Behaviors;
using CGFSMVVM.ViewModels;
using Xamarin.Forms;

namespace CGFSMVVM.Views
{
    public class TextCommentView : ContentPage
    {

        private StackLayout _baseLayout;
        private Image _headerImage;
        private Label _questionLabel,_messageLabel;

        private TextCommentViewModel textCommentViewModel;

        private string _prevQuesIndex;
        private string _currQuesIndex;

        ComponentComment cc = new ComponentComment();

        public TextCommentView(string prevQuesIndex, string currQuesIndex)
        {
            this._prevQuesIndex = prevQuesIndex;
            this._currQuesIndex = currQuesIndex;

            InitUI();

            NavigationPage.SetHasNavigationBar(this, false);

            textCommentViewModel = new TextCommentViewModel(Navigation, currQuesIndex);

            BindingContext = textCommentViewModel;

            textCommentViewModel.LoadQuestionCommand.Execute(_questionLabel);
            textCommentViewModel.LoadMessageTextCommand.Execute(_messageLabel);

            cc._commentEditor.Behaviors.Add(new EventToCommandBehavior
            {
                EventName = "Completed",
                Command = textCommentViewModel.EntryCompletedCommand,
                CommandParameter = cc._commentEditor
            });

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

            _baseLayout.Children.Add(_headerImage);
            _baseLayout.Children.Add(_questionLabel);

            StackLayout sl = cc.GetCommentLayout();

            ComponentNavPane npv = new ComponentNavPane();
            RelativeLayout sl2 = npv.GetNavPane();
            _messageLabel = npv.GetMessageLabel();

            ComponentProgressPane cpp = new ComponentProgressPane();
            StackLayout sl3 = cpp.GetProgressPane();

            _baseLayout.Children.Add(sl);
            _baseLayout.Children.Add(sl2);
            _baseLayout.Children.Add(sl3);

            Content = _baseLayout;

        }


		protected override void OnAppearing()
		{
            base.OnAppearing();
            cc._commentEditor.Focus();
		}
	}
}
