using System;
using CGFSMVVM.DataParsers;
using Xamarin.Forms;

namespace CGFSMVVM.Views
{
    public class ComponentProgressPane
    {
        private StackLayout _baseLayer,_upperLayer,_bottomLayer;
        private Label _label;
        private ProgressBar _progressBar;

        public StackLayout GetProgressPane()
        {
            
            _baseLayer = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Orientation=StackOrientation.Vertical,
                HeightRequest = 40
            };

            _upperLayer = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            _bottomLayer = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };


            _label = new Label()
            {
                Text = QuestionJsonDeserializer.GetCurrentQuestionIndex().ToString() + " of " + QuestionJsonDeserializer.GetQuestionCount().ToString(),
                TextColor = Color.Green,
                FontSize = 14,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
            };

            _progressBar = new ProgressBar()
            {
                VerticalOptions = LayoutOptions.Center,
                Progress = (double)QuestionJsonDeserializer.GetCurrentQuestionIndex() / (double)QuestionJsonDeserializer.GetQuestionCount(),
                WidthRequest = Application.Current.MainPage.Width - 10
            };

            _upperLayer.Children.Add(_progressBar);
            _bottomLayer.Children.Add(_label);

            _baseLayer.Children.Add(_upperLayer);
            _baseLayer.Children.Add(_bottomLayer);

       
            return _baseLayer;
        }
    }
}
