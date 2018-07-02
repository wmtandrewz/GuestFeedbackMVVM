using System;
using System.Collections.Generic;
using CGFSMVVM.Models;
using CGFSMVVM.ViewModels;
using Xamarin.Forms;

namespace CGFSMVVM.Views
{
    public class ComponentHeatBar
    {
        private StackLayout _baseLayout, _imLayout;
        private Button _button;

        public StackLayout GetHeatBarLayout(int buttonCount)
        {
            _baseLayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Start,
                HeightRequest  = 80
            };
            _imLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start,
                HeightRequest = 60
            };

            for (int i = 0; i < buttonCount; i++)
            {
                _button = new Button()
                {
                    BackgroundColor = Color.White,
                    Text = (i + 1).ToString(),
                    TextColor = Color.Black,
                    FontSize = 20,
                    HeightRequest = 60,
                    WidthRequest = 60
                };

                _imLayout.Children.Add(_button);

                GlobalModel.HeatButonList.Add(_button);

                HeatButtonModel _buttonModel = new HeatButtonModel(i.ToString(), _button);
                _button.CommandParameter = _buttonModel;
                _button.SetBinding(Button.CommandProperty, new Binding("HeatBarTappedCommand"));

            }

            _baseLayout.Children.Add(_imLayout);

            return _baseLayout;
        }

    }
}
