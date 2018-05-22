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
    /// <summary>
    /// Dual option view model.
    /// </summary>
    public class DualOptionViewModel
    {
        public ICommand TappedCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand NextCommand { get; }
        public ICommand LoadQuestionCommand { get; }
        public ICommand LoadMessageTextCommand { get; }

        public INavigation _navigation { get; }
        public string _currQuestionindex { get; }
        private string _selectedValue;
        private bool _nextHasPreviousFeedback = false;
        private List<Image> DualOptionList = new List<Image>();
        private bool _tapLocked = false;
        private bool _canLoadNext = true;

        private QuestionsModel _Questions;


        /// <summary>
        /// Initializes a new instance of the <see cref="T:CGFSMVVM.ViewModels.DualOptionViewModel"/> class.
        /// </summary>
        /// <param name="iNavigation">navigation stack reference.</param>
        /// <param name="currQuestionIndex">Current question index.</param>
        public DualOptionViewModel(INavigation iNavigation, string currQuestionIndex)
        {
            this._navigation = iNavigation;
            this._currQuestionindex = currQuestionIndex;

            this._Questions = QuestionJsonDeserializer.GetQuestion(currQuestionIndex);

            TappedCommand = new Command<DualOptionModel>(async (dualOptionModel) => await IconTapped(dualOptionModel).ConfigureAwait(true));
            BackCommand = new Command(BackButtonTapped);
            NextCommand = new Command(NextButtonTapped);
            LoadQuestionCommand = new Command<Label>(LoadData);
            LoadMessageTextCommand = new Command<Label>(SetMessageText);

            RestoreFeedbackData();

            LoadImages();
        }

        /// <summary>
        /// Sets the message text.
        /// </summary>
        /// <param name="label">Message Label.</param>
        private void SetMessageText(Label label)
        {
            CommonPropertySetter.SetMessageLabelText(label, _Questions.Optional);
        }

        /// <summary>
        /// Loads the button icons.
        /// </summary>
        private void LoadImages()
        {
            foreach (var item in GlobalModel.DualOptionList)
            {
                this.DualOptionList.Add(item);
            }
        }

        /// <summary>
        /// Loads the question.
        /// </summary>
        /// <param name="label">Question Label.</param>
        private void LoadData(Label label)
        {
            CommonPropertySetter.SetQuestionLabelText(label, _Questions.QDesc);
        }

        /// <summary>
        /// Icon tapped event.
        /// </summary>
        /// <returns>Tapped index.</returns>
        /// <param name="dualOptionModel">Dual option model.</param>
        async Task IconTapped(DualOptionModel dualOptionModel)
        {
          
            if (!_tapLocked)
            {
                _tapLocked = true;
                _canLoadNext = false;

                if (dualOptionModel.ID == "1")
                {
                    DualOptionList[1].Opacity = 0.5;
                    DualOptionList[0].Opacity = 1;

                    _selectedValue = dualOptionModel.ID;
                }

                if (dualOptionModel.ID != "1")
                {
                    DualOptionList[1].Opacity = 1;
                    DualOptionList[0].Opacity = 0.5;

                    _selectedValue = dualOptionModel.ID;
                }

                await dualOptionModel.image.ScaleTo(2, 150);
                await dualOptionModel.image.ScaleTo(1, 150);

                Device.StartTimer(TimeSpan.FromSeconds(GlobalModel.TimeSpan), () =>
                {
                    LoadNextPage();
                    _tapLocked = false;
                    return false;
                });

            }
        }

        /// <summary>
        /// Backs the button tapped.
        /// </summary>
        private void BackButtonTapped()
        {
            _navigation.PopAsync();
        }

        /// <summary>
        /// Load Next if button tapped.
        /// </summary>
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

        /// <summary>
        /// Loads the next page.
        /// </summary>
        private void LoadNextPage()
        {
            AddToFeedbackCart();
            PageLoadHandler.LoadNextPage(_navigation, _currQuestionindex, _selectedValue);
            _canLoadNext = true;
        }

        /// <summary>
        /// Adds feedbacks to feedback cart.
        /// </summary>
        private void AddToFeedbackCart()
        {
            if (FeedbackCart.RatingNVC[_Questions.QId] == null)
            {
                FeedbackCart.RatingNVC.Add(_Questions.QId, _selectedValue);
            }
            else
            {
                FeedbackCart.OtherNVC.Remove(_Questions.QId);
                AddToFeedbackCart();
            }
        }

        /// <summary>
        /// Restores the feedback data.
        /// </summary>
        private void RestoreFeedbackData()
        {
            string previousFeedback = FeedbackCart.RatingNVC[_Questions.QId];

            if (previousFeedback != null)
            {
                _selectedValue = previousFeedback;

                if (previousFeedback == "1")
                {
                    GlobalModel.DualOptionList[1].Opacity = 0.5;
                    GlobalModel.DualOptionList[0].Opacity = 1;

                    _selectedValue = "1";
                }

                if (previousFeedback != "1")
                {
                    GlobalModel.DualOptionList[1].Opacity = 1;
                    GlobalModel.DualOptionList[0].Opacity = 0.5;

                    _selectedValue = "0";
                }
            }
        }

        /// <summary>
        /// Checks the next has feedback.
        /// </summary>
        /// <returns><c>true</c>, if next has feedback, <c>false</c> otherwise.</returns>
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
