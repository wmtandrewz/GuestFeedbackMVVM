using System;
using System.Collections.Generic;
using CGFSMVVM.Models;
using Xamarin.Forms;

namespace CGFSMVVM.Views
{
    public class ComponentMultiSelections
    {
        private StackLayout _baseLayer,_lableLayer;
        private Label _optionLabel;

        public StackLayout GetMultiSelectionsLayout(List<string>optionList){

            _baseLayer = new StackLayout();

            _lableLayer = new StackLayout()
            {
                Orientation=StackOrientation.Vertical,
                VerticalOptions=LayoutOptions.Center,
                HorizontalOptions=LayoutOptions.Center,
                Margin=new Thickness(100,10,100,10)
            };

            for (int i = 0; i < optionList.Count; i++)
            {
                _optionLabel = new Label()
                {
                    Text = optionList[i],
                    FontSize = 20,
                    VerticalTextAlignment=TextAlignment.Center,
                    HorizontalTextAlignment=TextAlignment.Center,
                    BackgroundColor=Color.FromRgb(60,0,70),
                    TextColor=Color.White,
                    HeightRequest=70,
                    WidthRequest=500,
                    Margin=new Thickness(0,5,0,5)
                };

                GlobalModel.MultiSelectionsLabelList.Add(_optionLabel);

                TapGestureRecognizer _tapGestureRecognizer = new TapGestureRecognizer();
                _tapGestureRecognizer.SetBinding(TapGestureRecognizer.CommandProperty, "OptionTappedCommand");

                MultiOpsLabelModel _multiOpsLabelModel = new MultiOpsLabelModel(i.ToString(),_optionLabel);
                _tapGestureRecognizer.CommandParameter = _multiOpsLabelModel;
                _optionLabel.GestureRecognizers.Add(_tapGestureRecognizer);

                _lableLayer.Children.Add(_optionLabel);
            }

            _baseLayer.Children.Add(_lableLayer);

            return _baseLayer;
        }
    }
}
