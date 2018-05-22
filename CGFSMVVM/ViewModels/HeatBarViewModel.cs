using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CGFSMVVM.DataParsers;
using CGFSMVVM.Models;
using CGFSMVVM.Services;
using CGFSMVVM.Views;
using Xamarin.Forms;

namespace CGFSMVVM.ViewModels
{
    public class HeatBarViewModel
    {
        public ICommand HeatBarTappedCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand NextCommand { get; }
        public ICommand LoadQuestionCommand { get; }
        public ICommand LoadMessageTextCommand { get; }
        public INavigation _navigation { get; }

        public string _currQuestionindex { get; }
        private string _selectedValue;
        private bool _tapLocked = false;
        private bool _canLoadNext = true;
        private bool _nextHasPreviousFeedback = false;

        private List<Button> buttonList = new List<Button>();
        private List<Color> _colorList;
        private QuestionsModel _Questions;


        public HeatBarViewModel(INavigation iNavigation,string currQuestionIndex)
        {
            this._navigation = iNavigation;
            this._currQuestionindex = currQuestionIndex;

            this._Questions = QuestionJsonDeserializer.GetQuestion(currQuestionIndex);

            LoadButtons();

            _colorList = new List<Color>
            {
                Color.FromRgb(119, 229, 0),
                Color.FromRgb(126, 206, 4),
                Color.FromRgb(133, 183, 8),
                Color.FromRgb(140, 160, 12),
                Color.FromRgb(157, 147, 16),
                Color.FromRgb(175, 130, 20),
                Color.FromRgb(190, 117, 25),
                Color.FromRgb(210, 100, 30),
                Color.FromRgb(220, 85, 35),
                Color.FromRgb(240, 60, 40)
            };

            HeatBarTappedCommand = new Command<HeatButtonModel>(ButtonTapped);
            BackCommand = new Command(BackButtonTapped);
            NextCommand = new Command(NextButtonTapped);
            LoadQuestionCommand = new Command<Label>(LoadData);
            LoadMessageTextCommand = new Command<Label>(SetMessageText);

            RestoreFeedbackData();
        }

        private void SetMessageText(Label label)
        {
            CommonPropertySetter.SetMessageLabelText(label, _Questions.Optional);
        }

        private void LoadButtons()
        {
            foreach (var item in GlobalModel.HeatButonList)
            {
                this.buttonList.Add(item);
            }
        }

        private void LoadData(Label label)
        {
            CommonPropertySetter.SetQuestionLabelText(label, _Questions.QDesc);
        }

        private async void ButtonTapped(HeatButtonModel heatButtonModel)
        {
            if (!_tapLocked)
            {
                _tapLocked = true;
                _canLoadNext = false;

                _selectedValue = (Convert.ToInt32(heatButtonModel.ID) + 1).ToString();
                Console.WriteLine("tapped :" + _selectedValue);

                foreach (var item in buttonList)
                {
                    item.BackgroundColor = Color.White;
                    item.TextColor = Color.Black;
                }

                int _seq = 9;

                foreach (var item in buttonList)
                {
                    item.BackgroundColor = _colorList[_seq];
                    item.TextColor = Color.White;

                    if (item.Id == heatButtonModel.button.Id)
                    {
                        break;
                    }

                    _seq--;
                }

                await heatButtonModel.button.ScaleTo(2, 150);
                await heatButtonModel.button.ScaleTo(1, 150);

                Device.StartTimer(TimeSpan.FromSeconds(GlobalModel.TimeSpan), () =>
                {
                    LoadNextPage();
                    _tapLocked = false;
                    return false;
                });
            }

        }

        private void BackButtonTapped()
        {
            _navigation.PopAsync();
        }

        private void NextButtonTapped()
        {
            if (_canLoadNext)
            {
                CheckNextHasFeedback();

                if (_Questions.Optional || _nextHasPreviousFeedback)
                {
                    LoadNextPage();
                }
                else
                {
                    if (_selectedValue == null)
                    {
                        Application.Current.MainPage.DisplayAlert("Attention!", "Please give your feedback to continue.", "OK");
                    }
                    else
                    {
                        LoadNextPage();
                    }
                }
            }
        }

        private void LoadNextPage()
        {
            AddToFeedbackCart();
            PageLoadHandler.LoadNextPage(_navigation, _currQuestionindex, _selectedValue);
            _canLoadNext = true;
        }

        private void AddToFeedbackCart()
        {
            if (FeedbackCart.RatingNVC[_Questions.QId] == null)
            {
                FeedbackCart.RatingNVC.Add(_Questions.QId, _selectedValue);
            }
            else
            {
                FeedbackCart.RatingNVC.Remove(_Questions.QId);
                AddToFeedbackCart();
            }
        }

        private void RestoreFeedbackData()
        {
            string previousFeedback = FeedbackCart.RatingNVC[_Questions.QId];

            if (previousFeedback != null)
            {
                _selectedValue = previousFeedback;

                foreach (var item in GlobalModel.HeatButonList)
                {
                    item.BackgroundColor = Color.White;
                    item.TextColor = Color.Black;
                }

                int _seq = 9;

                foreach (var item in GlobalModel.HeatButonList)
                {
                    item.BackgroundColor = _colorList[_seq];
                    item.TextColor = Color.White;

                    if ((10 -_seq).ToString() == _selectedValue)
                    {
                        break;
                    }

                    _seq--;
                }


            }
        }

        private bool CheckNextHasFeedback()
        {
            var nextQuestion = QuestionJsonDeserializer.GetNextQuestion(_currQuestionindex);

            if (nextQuestion != null)
            {
                var nextQId = nextQuestion.QId;
                var GivenNextFeedback = FeedbackCart.RatingNVC[nextQId];

                if (GivenNextFeedback != null)
                {
                    _nextHasPreviousFeedback = true;
                    return true;
                }
                else
                {
                    _nextHasPreviousFeedback = false;
                    return false;
                }
            }
            return false;
        }

    }
}
