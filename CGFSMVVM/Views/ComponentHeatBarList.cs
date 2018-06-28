using System;
using System.Collections.Generic;
using CGFSMVVM.DataParsers;
using CGFSMVVM.Models;
using Xamarin.Forms;

namespace CGFSMVVM.Views
{
    public class ComponentHeatBarList
    {
        StackLayout _baseLayout, _heatListLayout;
        Label _titleLabel, _childTitle;
        Button _button;

        Dictionary<string, QuestionsModel> children = new Dictionary<string, QuestionsModel>();


        public StackLayout GetHeatBarListLayout(string currQuestion)
        {
            children = QuestionJsonDeserializer.GetChildQuestionSet(currQuestion);
            var childCount = 0;

            if(children!=null)
            {
                childCount = children.Count;

                foreach (var item in children)
                {
                    if(item.Value.QType == "L")
                    {
                        childCount -= 1;
                    }
                }
            }

            _baseLayout = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.EndAndExpand,
                Padding = new Thickness(20, 50, 10, 10)

            };

            _titleLabel = new Label()
            {
                HorizontalTextAlignment = TextAlignment.Start,
                FontSize = 24,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.White
            };


            List<StackLayout> hearBarList = new List<StackLayout>();

            for (int i = 0; i < childCount; i++)
            {
                _heatListLayout = new StackLayout()
                {
                    Orientation = StackOrientation.Horizontal,
                    HeightRequest = 50,
                    Padding = new Thickness(10, 10, 10, 10),
                    HorizontalOptions = LayoutOptions.EndAndExpand

                };

                _childTitle = new Label()
                {
                    HorizontalTextAlignment = TextAlignment.Start,
                    VerticalTextAlignment = TextAlignment.Center,
                    FontSize = 20,
                    TextColor = Color.White,
                    WidthRequest = 500
                };

               
                _heatListLayout.Children.Add(_childTitle);

                List<Button> buttonList = new List<Button>();

                for (int j = 0; j < 5; j++)
                {
                    _button = new Button()
                    {
                        BackgroundColor = Color.White,
                        Text = (j + 1).ToString(),
                        TextColor = Color.Black,
                        FontSize = 14,
                        HeightRequest = 50,
                        WidthRequest = 50
                    };

                    HeatButtonModel _buttonModel = new HeatButtonModel(j.ToString(), _button);
                    Heat_ChildModel heat_ChildModel = new Heat_ChildModel(i.ToString(), j.ToString(), _buttonModel);

                    _button.CommandParameter = heat_ChildModel;
                    _button.SetBinding(Button.CommandProperty, new Binding("ChildHeatBarTappedCommand"));

                    buttonList.Add(_button);

                    _heatListLayout.Children.Add(_button);
                }

                hearBarList.Add(_heatListLayout);

                ChildHeatListModel _heatListModel = new ChildHeatListModel(i.ToString(), _titleLabel, _childTitle, buttonList);

                GlobalModel.ChildHeatListCollection.Add(_heatListModel);

               

            }

            if (childCount > 0)
            {
                _baseLayout.Children.Add(_titleLabel);
            }

            foreach (var item in hearBarList)
            {
                _baseLayout.Children.Add(item);
                           
            }


            return _baseLayout;
        }
    }
}
