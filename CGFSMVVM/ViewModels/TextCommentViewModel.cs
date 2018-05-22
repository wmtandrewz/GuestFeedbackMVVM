using System;
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
    public class TextCommentViewModel
    {
        public ICommand BackCommand { get; }
        public ICommand NextCommand { get; }
        public ICommand LoadQuestionCommand { get; }
        public ICommand LoadMessageTextCommand { get; }
        public ICommand EntryCompletedCommand { get; }

        public INavigation _navigation { get; }
        public string _currQuestionindex { get; }

        private string _selectedValue = "";
        private QuestionsModel _Questions;
        private bool _nextHasPreviousFeedback = false;

        public TextCommentViewModel(INavigation iNavigation, string currQuestionIndex)
        {
            this._navigation = iNavigation;
            this._currQuestionindex = currQuestionIndex;

            this._Questions = QuestionJsonDeserializer.GetQuestion(currQuestionIndex);

            BackCommand = new Command(BackButtonTapped);
            NextCommand = new Command(NextButtonTapped);
            LoadQuestionCommand = new Command<Label>(LoadData);
            LoadMessageTextCommand = new Command<Label>(SetMessageText);
            EntryCompletedCommand = new Command<Editor>(SetTempCommentData);

            RestoreFeedbackData();
        }

        private void SetTempCommentData(Editor comment)
        {

            if (!string.IsNullOrWhiteSpace(comment.Text))
            {
                TempData.CommentText = comment.Text;
                Console.WriteLine(comment.Text);
            }
        }

        private void SetCommentText(string text)
        {
            _selectedValue = text;
        }

        private void SetMessageText(Label label)
        {
            CommonPropertySetter.SetMessageLabelText(label, _Questions.Optional);
        }

        private void BackButtonTapped()
        {
            AddToFeedbackCart();
            _navigation.PopAsync();
        }

        private void LoadData(Label label)
        {
            CommonPropertySetter.SetQuestionLabelText(label, _Questions.QDesc);
        }

        private void NextButtonTapped()
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

        private void LoadNextPage()
        {
            AddToFeedbackCart();
            PageLoadHandler.LoadNextPage(_navigation, _currQuestionindex, "");
        }

        private void AddToFeedbackCart()
        {
            if (FeedbackCart.CommentNVC[_Questions.QId] == null)
            {
                FeedbackCart.CommentNVC.Add(_Questions.QId, !string.IsNullOrEmpty(TempData.CommentText) ? TempData.CommentText:"");
                TempData.CommentText = "";
            }
            else
            {
                FeedbackCart.CommentNVC.Remove(_Questions.QId);
                AddToFeedbackCart();
            }
        }

        private void RestoreFeedbackData()
        {
            string previousFeedback = FeedbackCart.CommentNVC[_Questions.QId];

            if (previousFeedback != null)
            {
                _selectedValue = previousFeedback;

                GlobalModel.CommentEditor.Text = previousFeedback;
            }
        }

        private bool CheckNextHasFeedback()
        {
            var nextQuestion = QuestionJsonDeserializer.GetNextQuestion(_currQuestionindex);

            if (nextQuestion != null)
            {
                var nextQId = nextQuestion.QId;
                var GivenNextFeedback = FeedbackCart.CommentNVC[nextQId];

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
