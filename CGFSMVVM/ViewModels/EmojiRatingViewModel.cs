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
    /// <summary>
    /// Emoji rating view model.
    /// </summary>
    public class EmojiRatingViewModel
    {
        public ICommand TappedCommand { get; }
        public ICommand LoadQuestionCommand { get; }
        public ICommand LoadMessageTextCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand NextCommand { get; }
        public INavigation _navigation { get; }

        private string _currQuestionindex { get; }
        private string _selectedValue;
        private bool _nextHasPreviousFeedback = false;
        private List<Image> emojiIconList = new List<Image>();
        private List<Label> emojiLabelList = new List<Label>();
        private bool _tapLocked = false;
        private bool _canLoadNext = true;


        private QuestionsModel _Questions;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CGFSMVVM.ViewModels.EmojiRatingViewModel"/> class.
        /// </summary>
        /// <param name="iNavigation">I navigation.</param>
        /// <param name="currQuestionIndex">Curr question index.</param>
        public EmojiRatingViewModel(INavigation iNavigation, string currQuestionIndex)
        {
            this._navigation = iNavigation;
            this._currQuestionindex = currQuestionIndex;

            this._Questions = QuestionJsonDeserializer.GetQuestion(currQuestionIndex);

            LoadEmojis();

            TappedCommand = new Command<EmojiIconModel>(async (emojiIconModel) => await EmojiTapped(emojiIconModel).ConfigureAwait(true));
            LoadQuestionCommand = new Command<Label>(LoadData);
            LoadMessageTextCommand = new Command<Label>(SetMessageText);

            BackCommand = new Command(BackButtonTapped);
            NextCommand = new Command(NextButtonTapped);

            RestoreFeedbackData();
        }

        /// <summary>
        /// Sets the message text.
        /// </summary>
        /// <param name="label">Label.</param>
        private void SetMessageText(Label label)
        {
            CommonPropertySetter.SetMessageLabelText(label, _Questions.Optional);
        }

        /// <summary>
        /// Loads the emojis.
        /// </summary>
        private void LoadEmojis()
        {
            foreach (var item in GlobalModel.EmojiIconList)
            {
                this.emojiIconList.Add(item);
            }

            foreach (var item in GlobalModel.EmojiDescLabelList)
            {
                this.emojiLabelList.Add(item);
            }
        }

        /// <summary>
        /// Loads the Question.
        /// </summary>
        /// <param name="label">Label.</param>
        private void LoadData(Label label)
        {
            CommonPropertySetter.SetQuestionLabelText(label, _Questions.QDesc);
        }

        /// <summary>
        /// Emojis the tapped.
        /// </summary>
        /// <returns>The tapped.</returns>
        /// <param name="emojiIconModel">Emoji icon model.</param>
        async Task EmojiTapped(EmojiIconModel emojiIconModel)
        {
            if (!_tapLocked)
            {
                _tapLocked = true;
                _canLoadNext = false;

                _selectedValue = (Convert.ToInt32(emojiIconModel.Id) + 1).ToString();

                if(FeedbackCart._guestImage == null && Convert.ToInt32(_selectedValue) < 3)
                {
                    NativeCamera.UploadImage();
                }

                Console.WriteLine("tapped :" + _selectedValue);

                foreach (var item in emojiIconList)
                {
                    if (item.Id != emojiIconModel.EmojiIcon.Id)
                    {
                        item.Opacity = 0.5;
                    }
                    else
                    {
                        item.Opacity = 1;
                    }
                }

                foreach (var item in emojiLabelList)
                {
                    if (emojiIconModel.EmojiDescLabel.Id == item.Id)
                    {
                        item.TextColor = Color.LightGreen;
                    }
                    else
                    {
                        item.TextColor = Color.Gray;
                    }
                }

                await emojiIconModel.EmojiIcon.ScaleTo(2, 150);
                await emojiIconModel.EmojiIcon.ScaleTo(1, 150);

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
        /// Loads Next button tapped.
        /// </summary>
        private void NextButtonTapped()
        {
            if (_canLoadNext)
            {
                CheckNextHasFeedback();

                if (_Questions.Optional || _nextHasPreviousFeedback)
                {
                    if (_Questions.Optional && _selectedValue == null)
                    {
                        LoadNextPageWithSkip();
                    }
                    else
                    {
                        LoadNextPage();
                    }
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
        /// Loads the next page with skip.
        /// </summary>
        private void LoadNextPageWithSkip()
        {
            AddToFeedbackCart("0");
            PageLoadHandler.LoadNextPage(_navigation, _currQuestionindex, "0");
            _canLoadNext = true;
        }

        /// <summary>
        /// Adds feedback to feedback cart.
        /// </summary>
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

        /// <summary>
        /// Adds to feedback cart.
        /// </summary>
        /// <param name="skipped">Is Skipped.</param>
        private void AddToFeedbackCart(string skipped)
        {
            if (FeedbackCart.RatingNVC[_Questions.QId] == null)
            {
                FeedbackCart.RatingNVC.Add(_Questions.QId, skipped);
            }
            else
            {
                FeedbackCart.RatingNVC.Remove(_Questions.QId);
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

                int i = 1;

                foreach (var item in GlobalModel.EmojiIconList)
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

                int ii = 1;
                foreach (var item in emojiLabelList)
                {
                    if (ii.ToString() == previousFeedback)
                    {
                        item.TextColor = Color.LightGreen;
                    }
                    else
                    {
                        item.TextColor = Color.Gray;
                    }

                    ii++;
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
