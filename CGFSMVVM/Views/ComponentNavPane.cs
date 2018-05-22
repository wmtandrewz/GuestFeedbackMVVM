using System;
using Xamarin.Forms;

namespace CGFSMVVM.Views
{
    public class ComponentNavPane
    {
        private RelativeLayout _baseLayer;
        private StackLayout _leftLayer, _midLayer, _rightLayer;
        private Image _backIcon, _nextIcon;
        private Label _messegeLabel;

        public RelativeLayout GetNavPane()
        {
            _baseLayer = new RelativeLayout()
            {
                HeightRequest = 200
            };

            _leftLayer = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.StartAndExpand
            };
            _midLayer = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.Center
            };
            _rightLayer = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.EndAndExpand
            };

            _backIcon = new Image
            {
                Aspect = Aspect.AspectFit,
                Source = ImageSource.FromFile("Images/back.png"),
                HeightRequest = 60,
                VerticalOptions = LayoutOptions.Center
            };
            _nextIcon = new Image
            {
                Aspect = Aspect.AspectFit,
                Source = ImageSource.FromFile("Images/next.png"),
                HeightRequest = 60,
                VerticalOptions = LayoutOptions.Center
            };

            _messegeLabel = new Label()
            {
                TextColor = Color.Gray,
                FontSize=14,
                WidthRequest = 300,
                VerticalOptions=LayoutOptions.Center,
                HorizontalOptions=LayoutOptions.Center,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment=TextAlignment.Center,
            };

            TapGestureRecognizer _backTapRecognizer = new TapGestureRecognizer();
            _backTapRecognizer.SetBinding(TapGestureRecognizer.CommandProperty, "BackCommand");

            TapGestureRecognizer _nextTapRecognizer = new TapGestureRecognizer();
            _nextTapRecognizer.SetBinding(TapGestureRecognizer.CommandProperty, "NextCommand");

            _backIcon.GestureRecognizers.Add(_backTapRecognizer);
            _nextIcon.GestureRecognizers.Add(_nextTapRecognizer);

            _leftLayer.Children.Add(_backIcon);
            _midLayer.Children.Add(_messegeLabel);
            _rightLayer.Children.Add(_nextIcon);

            _baseLayer.Children.Add(_leftLayer,
                                    Constraint.RelativeToParent((parent) =>
                                    {
                                        return parent.Width - (parent.Width - 60);
                                    }),
                                    Constraint.RelativeToParent((parent) =>
                                    {
                                        return parent.Height / 2;
                                    }));

            _baseLayer.Children.Add(_midLayer,
                                    Constraint.RelativeToParent((parent) =>
                                    {
                                        return (parent.Width) / 2 - 150;
                                    }),
                                    Constraint.RelativeToParent((parent) =>
                                    {
                                        return parent.Height / 2;
                                    }));

            _baseLayer.Children.Add(_rightLayer,
                                    Constraint.RelativeToParent((parent) =>
                                    {
                                        return parent.Width - 120;
                                    }),
                                    Constraint.RelativeToParent((parent) =>
                                    {
                                        return parent.Height / 2;
                                    }));

            return _baseLayer;
        }

        public Label GetMessageLabel()
        {
            return _messegeLabel;
        }
    }
}
