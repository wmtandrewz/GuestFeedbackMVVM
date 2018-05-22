using System;
using System.Collections.Generic;
using CGFSMVVM.Models;
using CGFSMVVM.ViewModels;
using Xamarin.Forms;

namespace CGFSMVVM.Views
{
    public class ComponentEmoji
    {
        public Image _emojiIcon;
        private Label _emojiDescLabel;
        private StackLayout _baseLayout, _imLayout, _emojiLayout;

        public StackLayout EmojiCompLayout(List<string> emojiList, List<string> emojiDescList)
        {
            _baseLayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.Center
            };
            _imLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
                HeightRequest = 100
            };
            for (int i = 0; i < emojiList.Count; i++)
            {
                _emojiLayout = new StackLayout
                {
                    Orientation = StackOrientation.Vertical,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    WidthRequest = 150,
                    Spacing = 20
                };
                _emojiDescLabel = new Label
                {
                    TextColor = Color.White,
                    FontSize = 18,
                    Text = emojiDescList[i],
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    HeightRequest = 200,
                };

                _emojiIcon = new Image { Aspect = Aspect.AspectFit };
                _emojiIcon.Source = ImageSource.FromFile(emojiList[i]);
                _emojiIcon.WidthRequest = 100;

                GlobalModel.EmojiDescLabelList.Add(_emojiDescLabel);
                GlobalModel.EmojiIconList.Add(_emojiIcon);

                TapGestureRecognizer _tapGestureRecognizer = new TapGestureRecognizer();
                _tapGestureRecognizer.SetBinding(TapGestureRecognizer.CommandProperty, "TappedCommand");

                EmojiIconModel _emojiIconModel = new EmojiIconModel(i.ToString(), _emojiIcon, _emojiDescLabel);
                _tapGestureRecognizer.CommandParameter = _emojiIconModel;
                _emojiIcon.GestureRecognizers.Add(_tapGestureRecognizer);

                _emojiLayout.Children.Add(_emojiIcon);
                _emojiLayout.Children.Add(_emojiDescLabel);

                _imLayout.Children.Add(_emojiLayout);

            }

            _baseLayout.Children.Add(_imLayout);

            return _baseLayout;
        }

    }
}
