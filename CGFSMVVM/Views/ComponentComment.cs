using System;
using System.Collections.Generic;
using CGFSMVVM.Behaviors;
using CGFSMVVM.Models;
using CGFSMVVM.ViewModels;
using Xamarin.Forms;

namespace CGFSMVVM.Views
{
    public class ComponentComment
    {
        private StackLayout _baseLayout;
        public Editor _commentEditor;

        public StackLayout GetCommentLayout()
        {

            _baseLayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.Fill,
                Margin=new Thickness(20,10,20,10)
            };

            _commentEditor = new Editor()
            {
                BackgroundColor=Color.Purple,
                TextColor=Color.Yellow,
                HorizontalOptions=LayoutOptions.Fill,
                HeightRequest=150
            };

            GlobalModel.CommentEditor = _commentEditor;

            _baseLayout.Children.Add(_commentEditor);

            return _baseLayout;
        }

    }
}
