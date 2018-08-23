using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
    public class MultiSelectionsViewModel
    {
        public ICommand OptionTappedCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand NextCommand { get; }
        public ICommand LoadQuestionCommand { get; }
        public ICommand LoadMessageTextCommand { get; }
        public ICommand LoadOptionsDescCommand { get; }
        public INavigation _navigation { get; }

        private string _currQuestionindex { get; }
        private string _selectedValue;
        private bool _nextHasPreviousFeedback = false;
        private bool _tapLocked = false;

        private QuestionsModel _Questions;
        private NameValueCollection SelectedOptionsNVC = new NameValueCollection();
        private List<Label> MultiSelectionsLabelList = new List<Label>();
        private List<Image> MultiSelectionsImageList = new List<Image>();
        private Label _messageLabel;

        public MultiSelectionsViewModel(INavigation iNavigation,string currQuestionIndex)
        {
            this._navigation = iNavigation;
            this._currQuestionindex = currQuestionIndex;

            this._Questions = QuestionJsonDeserializer.GetQuestion(currQuestionIndex);

            LoadLabels();

            OptionTappedCommand = new Command<MultiOpsLabelModel>(async (multiOpsModel) => await OptionTapped(multiOpsModel).ConfigureAwait(true));
            LoadQuestionCommand = new Command<Label>(LoadData);
            LoadMessageTextCommand = new Command<Label>(SetMessageText);
            LoadOptionsDescCommand = new Command(LoadOptionsDesc);
            BackCommand = new Command(BackButtonTapped);
            NextCommand = new Command(NextButtonTapped);

            RestoreFeedbackData();

        }

        private void SetMessageText(Label label)
        {
            _messageLabel = label;
            CommonPropertySetter.SetMessageLabelText(label, _Questions.Optional);
        }

        private void LoadLabels()
        {
            foreach (var item in GlobalModel.MultiSelectionsLabelList)
            {
                this.MultiSelectionsLabelList.Add(item);
            }

            foreach (var item in GlobalModel.MultiSelectionsIconlList)
            {
                this.MultiSelectionsImageList.Add(item);
            }
        }

        private void LoadOptionsDesc()
        {
            ////remove not applicable facilities
            //if (_Questions.OtherQuestions != null && _Questions.OtherQValuesNA != null)
            //{
            //    foreach (var item in _Questions.OtherQuestions)
            //    {
            //        if (_Questions.OtherQValuesNA.Contains(item.QOId))
            //        {
            //            _Questions.OtherQuestions.Remove(item);
            //        }
            //    }

            //}

            List<OtherQuestionsModel> QDesc = _Questions.OtherQuestions;

            int _index = 0;

            foreach (var item in GlobalModel.MultiSelectionsLabelList)
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

            try
            {
                //set message
                _messageLabel.Text = "Please Tap on Next button to continue";

                //_selectedValue = multiOpsModel.ID;
                //Console.WriteLine("tapped :" + _selectedValue);
                if (!_tapLocked)
                {
                    _tapLocked = true;

                    int iconSeq = 0;

                    foreach (var item in MultiSelectionsLabelList)
                    {
                        if (item.Id == multiOpsModel.OptionLabel.Id)
                        {
                            if (item.BackgroundColor == Color.FromRgb(60, 0, 70))
                            {
                                item.BackgroundColor = Color.Purple;
                                MultiSelectionsImageList[iconSeq].Source = ImageSource.FromFile("Images/checked.png");
                            }
                            else
                            {
                                item.BackgroundColor = Color.FromRgb(60, 0, 70);
                                MultiSelectionsImageList[iconSeq].Source = ImageSource.FromFile("Images/unchecked.png");
                            }
                        }
                        iconSeq++;
                    }

                    await multiOpsModel.OptionLabel.ScaleTo(2, 150);
                    await multiOpsModel.OptionLabel.ScaleTo(1, 150);

                    string[] selectedOptionsArray = new string[MultiSelectionsLabelList.Count];

                    int x = 0;
                    int loop = 0;
                    foreach (var item in MultiSelectionsLabelList)
                    {
                        if (item.BackgroundColor == Color.Purple)
                        {
                            selectedOptionsArray[loop] = x.ToString();
                            loop++;
                        }

                        x++;
                    }

                    _selectedValue = null;

                    if (string.IsNullOrEmpty(selectedOptionsArray[0]))
                    {
                        //set message
                        CommonPropertySetter.SetMessageLabelText(_messageLabel, _Questions.Optional);
                    }

                    foreach (var item in selectedOptionsArray)
                    {

                        if (!string.IsNullOrEmpty(item))
                        {
                            if (string.IsNullOrEmpty(_selectedValue))
                            {
                                _selectedValue = item;
                            }
                            else
                            {
                                _selectedValue += "," + item;
                            }
                        }
                    }
                }
                _tapLocked = false;
            }
            catch(Exception)
            {

            }
        }

        private void BackButtonTapped()
        {
            _navigation.PopAsync();
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
            PageLoadHandler.LoadNextPage(_navigation, _currQuestionindex, _selectedValue);
        }

        private void AddToFeedbackCart()
        {
            if (FeedbackCart.OtherNVC[_Questions.QId] == null)
            {
                if(string.IsNullOrEmpty(_selectedValue))
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

            if (previousFeedback != null)
            {
                _selectedValue = previousFeedback;

                string[] values = _selectedValue.Split(',');

                for (int a = 0; a < values.Length; a++)
                {
                    SelectedOptionsNVC.Add(values[a], values[a]);
                }

                int i = 0;
                int ii = 0;

                foreach (var item in GlobalModel.MultiSelectionsLabelList)
                {
                    if (ii < SelectedOptionsNVC.Count)
                    {
                        if (i.ToString() == SelectedOptionsNVC[ii])
                        {
                            if (item.BackgroundColor == Color.FromRgb(60, 0, 70))
                            {
                                item.BackgroundColor = Color.Purple;
                                MultiSelectionsImageList[i].Source = ImageSource.FromFile("Images/checked.png");
                            }
                            else
                            {
                                item.BackgroundColor = Color.FromRgb(60, 0, 70);
                                MultiSelectionsImageList[i].Source = ImageSource.FromFile("Images/unchecked.png");
                            }
                            ii++;
                        }
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
