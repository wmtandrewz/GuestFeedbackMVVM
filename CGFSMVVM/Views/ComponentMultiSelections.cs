using System;
using System.Collections.Generic;
using CGFSMVVM.DataParsers;
using CGFSMVVM.Models;
using Xamarin.Forms;

namespace CGFSMVVM.Views
{
    public class ComponentMultiSelections
    {
        private StackLayout _baseLayer,_lableLayer, _checkboxlayer;
        Image _checkBoxImage;
        private Label _optionLabel;

        private QuestionsModel _Questions;

        public StackLayout GetMultiSelectionsLayout(string currQuestionIndex){

            this._Questions = QuestionJsonDeserializer.GetQuestion(currQuestionIndex);

            _baseLayer = new StackLayout();

            _lableLayer = new StackLayout()
            {
                Orientation=StackOrientation.Vertical,
                VerticalOptions=LayoutOptions.Center,
                HorizontalOptions=LayoutOptions.Center,
                Margin=new Thickness(100,10,100,10)
            };


            for (int i = 0; i < _Questions.OtherQuestions.Count; i++)
            {
                _checkboxlayer = new StackLayout()
                {
                    Orientation = StackOrientation.Horizontal,
                    HeightRequest = 70
                };

                _checkBoxImage = new Image()
                {
                    Aspect = Aspect.AspectFit,
                    Source = ImageSource.FromFile("Images/unchecked.png"),
                    HeightRequest = 50
                };

                _optionLabel = new Label()
                {
                    FontSize = 20,
                    VerticalTextAlignment=TextAlignment.Center,
                    HorizontalTextAlignment=TextAlignment.Center,
                    BackgroundColor=Color.FromRgb(60,0,70),
                    TextColor=Color.White,
                    WidthRequest=500,
                    Margin=new Thickness(0,5,0,5)
                };

                _checkboxlayer.Children.Add(_checkBoxImage);
                _checkboxlayer.Children.Add(_optionLabel);

                GlobalModel.MultiSelectionsLabelList.Add(_optionLabel);
                GlobalModel.MultiSelectionsIconlList.Add(_checkBoxImage);

                TapGestureRecognizer _tapGestureRecognizer = new TapGestureRecognizer();
                _tapGestureRecognizer.SetBinding(TapGestureRecognizer.CommandProperty, "OptionTappedCommand");

                MultiOpsLabelModel _multiOpsLabelModel = new MultiOpsLabelModel(i.ToString(),_optionLabel,_checkBoxImage);
                _tapGestureRecognizer.CommandParameter = _multiOpsLabelModel;

                _optionLabel.GestureRecognizers.Add(_tapGestureRecognizer);
                _checkBoxImage.GestureRecognizers.Add(_tapGestureRecognizer);

                _lableLayer.Children.Add(_checkboxlayer);
            }

            _baseLayer.Children.Add(_lableLayer);

            return _baseLayer;
        }
    }
}
