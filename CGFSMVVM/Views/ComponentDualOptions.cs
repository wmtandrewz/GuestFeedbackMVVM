using System;
using System.Collections.Generic;
using CGFSMVVM.Models;
using CGFSMVVM.ViewModels;
using Xamarin.Forms;

namespace CGFSMVVM.Views
{
    public class ComponentDualOptions
    {
        private StackLayout _baseLayout,_contentLayout;
        private Image _yesIcon, _noIcon;

        public StackLayout GetDualOptionLayout()
        {
            _baseLayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.Center,
                Margin=new Thickness(20,10,20,10)
            };

            _contentLayout = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Center,
                HeightRequest = 150,
                Margin = new Thickness(100, 10, 100, 10)
            };

            _yesIcon = new Image
            {
                Aspect = Aspect.AspectFit,
                Source = ImageSource.FromFile("Images/yes.png"),
                HeightRequest = 100,
                Margin=new Thickness(10,10,10,10)
            };

            _noIcon = new Image
            {
                Aspect = Aspect.AspectFit,
                Source = ImageSource.FromFile("Images/no.png"),
                HeightRequest = 100,
                Margin = new Thickness(10, 10, 10, 10)
            };

            List<Image> _iconList = new List<Image>()
            {
                _yesIcon,
                _noIcon
            };

            GlobalModel.DualOptionList = _iconList;

            TapGestureRecognizer _yesGuestureTapped = new TapGestureRecognizer();
            _yesGuestureTapped.SetBinding(TapGestureRecognizer.CommandProperty, "TappedCommand");
            DualOptionModel dualOptionModel = new DualOptionModel("1", _yesIcon);
            _yesGuestureTapped.CommandParameter = dualOptionModel;
            _yesIcon.GestureRecognizers.Add(_yesGuestureTapped);

            TapGestureRecognizer _noGuestureTapped = new TapGestureRecognizer();
            _noGuestureTapped.SetBinding(TapGestureRecognizer.CommandProperty, "TappedCommand");
            DualOptionModel dualOptionModel2 = new DualOptionModel("0", _noIcon);
            _noGuestureTapped.CommandParameter = dualOptionModel2;
            _noIcon.GestureRecognizers.Add(_noGuestureTapped);

            _contentLayout.Children.Add(_yesIcon);
            _contentLayout.Children.Add(_noIcon);

            _baseLayout.Children.Add(_contentLayout);

            return _baseLayout;
        }

    }
}
