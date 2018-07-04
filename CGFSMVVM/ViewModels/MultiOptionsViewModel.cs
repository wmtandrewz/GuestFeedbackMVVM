using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using CGFSMVVM.DataParsers;
using CGFSMVVM.Models;
using CGFSMVVM.Services;
using Xamarin.Forms;

namespace CGFSMVVM.ViewModels
{
    public class MultiOptionsViewModel
    {
        public ICommand OptionTappedCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand NextCommand { get; }
        public ICommand LoadQuestionCommand { get; }
        public ICommand LoadMessageTextCommand { get; }
        public ICommand LoadOptionsDescCommand { get; }
        public ICommand OnAppearingCommand { get; }
        public INavigation _navigation { get; }

        public string _currQuestionindex { get; }
        private string _selectedValue;
        private bool _nextHasPreviousFeedback = false;
        private List<Label> MultiOptionsLabelList = new List<Label>();
        private bool _tapLocked = false;
        private bool _canLoadNext = true;
        private bool _canGoBack = true;

        private QuestionsModel _Questions;

        public MultiOptionsViewModel(INavigation iNavigation,string currQuestionIndex)
        {
            this._navigation = iNavigation;
            this._currQuestionindex = currQuestionIndex;

            this._Questions = QuestionJsonDeserializer.GetQuestion(currQuestionIndex);

            LoadLabels();

            OnAppearingCommand = new Command(OnAppearing);

            OptionTappedCommand = new Command<MultiOpsLabelModel>(async (multiOpsModel) => await OptionTapped(multiOpsModel).ConfigureAwait(true));
            LoadQuestionCommand = new Command<Label>(LoadData);
            LoadMessageTextCommand = new Command<Label>(SetMessageText);
            LoadOptionsDescCommand = new Command(LoadOptionsDesc);
            BackCommand = new Command(BackButtonTapped);
            NextCommand = new Command(NextButtonTapped);

            RestoreFeedbackData();

        }

        private void OnAppearing()
        {
            _canLoadNext = true;
            _canGoBack = true;
            _tapLocked = false;
        }

        private void LoadLabels()
        {
            foreach (var item in GlobalModel.MultiOptionsLabelList)
            {
                this.MultiOptionsLabelList.Add(item);
            }
        }

        private void SetMessageText(Label label)
        {
            CommonPropertySetter.SetMessageLabelText(label, _Questions.Optional);
        }

        private void LoadOptionsDesc()
        {
            List<OtherQuestionsModel> QDesc = _Questions.OtherQuestions;

            int _index = 0;

            foreach (var item in GlobalModel.MultiOptionsLabelList)
            {
                item.Text = QDesc[_index].QODesc;
                _index++;
            }
        }

        private void LoadData(Label label)
        {
            CommonPropertySetter.SetQuestionLabelText(label, _Questions.QDesc);
        }

        async Task OptionTapped(MultiOpsLabelModel multiOpsModel)
        {
            if (!_tapLocked)
            {
                _tapLocked = true;
                _canLoadNext = false;
                _canGoBack = false;

                _selectedValue = multiOpsModel.ID;
                Console.WriteLine("tapped :" + _selectedValue);

                foreach (var item in MultiOptionsLabelList)
                {
                    if (item.Id == multiOpsModel.OptionLabel.Id)
                    {
                        item.Opacity = 1;
                    }
                    else
                    {
                        item.Opacity = 0.5;
                    }
                }


                await multiOpsModel.OptionLabel.ScaleTo(2, 150);
                await multiOpsModel.OptionLabel.ScaleTo(1, 150);


                Device.StartTimer(TimeSpan.FromSeconds(GlobalModel.TimeSpan), () =>
                {
                    LoadNextPage();
                    //_tapLocked = false;
                    return false;

                });

            }
        }

        private void BackButtonTapped()
        {
            if (_canGoBack)
            {
                _navigation.PopAsync();
            }
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

            if (FeedbackCart._guestImage == null)
            {
                NativeCamera.UploadImage();
            }

            PageLoadHandler.LoadNextPage(_navigation, _currQuestionindex, _selectedValue);
            //_canLoadNext = true;
        }

        private void AddToFeedbackCart()
        {
            if (FeedbackCart.OtherNVC[_Questions.QId] == null)
            {
                if (string.IsNullOrEmpty(_selectedValue))
                {
                    _selectedValue = "-1";
                }

                FeedbackCart.OtherNVC.Add(_Questions.QId, _selectedValue);
            }
            else
            {
                FeedbackCart.OtherNVC.Remove(_Questions.QId);
                AddToFeedbackCart();
            }
        }

        private void RestoreFeedbackData()
        {
            string previousFeedback = FeedbackCart.OtherNVC[_Questions.QId];

            if(previousFeedback != null)
            {
                _selectedValue = previousFeedback;

                int i = 0;

                foreach (var item in GlobalModel.MultiOptionsLabelList)
                {
                    if (i.ToString() != previousFeedback)
                    {
                        item.Opacity = 0.5;
                    }
                    else
                    {
                        item.Opacity = 1;
                    }

                    i++;
                }
            }
        }

        private bool CheckNextHasFeedback()
        {
            var nextQuestion = QuestionJsonDeserializer.GetNextQuestion(_currQuestionindex);

            if (nextQuestion != null)
            {
                var nextQId = nextQuestion.QId;
                var GivenNextFeedback = FeedbackCart.OtherNVC[nextQId];

                if(GivenNextFeedback != null)
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
