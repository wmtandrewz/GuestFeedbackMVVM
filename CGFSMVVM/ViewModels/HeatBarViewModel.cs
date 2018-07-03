using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
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
        public ICommand ChildHeatBarTappedCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand NextCommand { get; }
        public ICommand LoadQuestionCommand { get; }
        public ICommand LoadChildQuestionCommand { get; }
        public ICommand LoadMessageTextCommand { get; }
        public INavigation _navigation { get; }

        public string _currQuestionindex { get; }
        private string _selectedValue;
        private bool _tapLocked = false;
        private bool _canLoadNext = true;
        private bool _nextHasPreviousFeedback = false;
        private bool _autofoward = false;

        private StackLayout _childLayout;
        private ScrollView _scrollView;
        private List<Button> buttonList = new List<Button>();
        private List<Color> _colorList;
        private QuestionsModel _Questions;
        Dictionary<string, QuestionsModel> children = new Dictionary<string, QuestionsModel>();
        List<ChildHeatListModel> childHeatLists = new List<ChildHeatListModel>();
        private Label _messageLabel;


        public HeatBarViewModel(INavigation iNavigation, string currQuestionIndex, StackLayout childLayout, ScrollView scrollView)
        {
            this._navigation = iNavigation;
            this._currQuestionindex = currQuestionIndex;
            this._childLayout = childLayout;
            this._scrollView = scrollView;

            this._Questions = QuestionJsonDeserializer.GetQuestion(currQuestionIndex);
            this.children = QuestionJsonDeserializer.GetChildQuestionSet(_currQuestionindex);


            LoadButtons();

            SetChildVisibility("All");

            LoadChildGlobalList();

            _colorList = GlobalModel.ColorList;

            HeatBarTappedCommand = new Command<HeatButtonModel>(ButtonTapped);
            ChildHeatBarTappedCommand = new Command<Heat_ChildModel>(ChildButtonTapped);
            BackCommand = new Command(BackButtonTapped);
            NextCommand = new Command(NextButtonTapped);
            LoadQuestionCommand = new Command<Label>(LoadData);
            LoadChildQuestionCommand = new Command(LoadChildData);
            LoadMessageTextCommand = new Command<Label>(SetMessageText);

            RestoreFeedbackData();
            RestoreChildFeedbackData();
        }

        /// <summary>
        /// Sets the child layer visibility.
        /// </summary>
        /// <param name="criteria">Criteria.</param>
        private void SetChildVisibility(string criteria)
        {
            try
            {
                if (!string.IsNullOrEmpty(criteria))
                {
                    if (_Questions.SubQuestionCriteria == criteria || _Questions.SubQuestionCriteria.Contains(criteria))
                    {
                        _childLayout.IsVisible = true;
                        _autofoward = false;
                    }
                    else
                    {
                        _childLayout.IsVisible = false;
                        _autofoward = true;
                    }
                }
                else
                {
                    _childLayout.IsVisible = false;
                    _autofoward = true;

                }
            }
            catch (Exception)
            {
                _autofoward = true;
                Console.WriteLine("SetChildVisibility failed");
            }
        }

        /// <summary>
        /// Sets the message text.
        /// </summary>
        /// <param name="label">Label.</param>
        private void SetMessageText(Label label)
        {
            _messageLabel = label;
            CommonPropertySetter.SetMessageLabelText(label, _Questions.Optional);
        }

        /// <summary>
        /// Loads the children from  global list.
        /// </summary>
        private void LoadChildGlobalList()
        {
            foreach (var item in GlobalModel.ChildHeatListCollection)
            {
                this.childHeatLists.Add(item);
            }
        }

        /// <summary>
        /// Loads the buttons fro GLobal model.
        /// </summary>
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

        /// <summary>
        /// Loads the child data.
        /// </summary>
        private void LoadChildData()
        {
            try
            {
                var childQuestions = GlobalModel.ChildHeatListCollection;

                if (childQuestions.Count > 1)
                {
                    //headerImage.IsVisible = false;

                    int seq = 0;

                    foreach (var item in children)
                    {

                        if (item.Value.QType == "L")
                        {
                            childQuestions[seq].titleLabel.Text = item.Value.QDesc;
                            seq--;
                        }
                        else
                        {
                            childQuestions[seq].childTitleLabel.Text = item.Value.QDesc;
                        }

                        seq++;
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Buttons the tapped.
        /// </summary>
        /// <param name="heatButtonModel">Heat button model.</param>
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

                int _seq = buttonList.Count - 1;

                if(_seq > 5)
                {
                    _colorList = GlobalModel.ColorList;
                }
                else
                {
                    _colorList = GlobalModel.ColorListSeconary;
                }

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

                //Check Criterias

                var subCriteria = _Questions.SubQuestionCriteria;
                if (!string.IsNullOrEmpty(subCriteria) && subCriteria != "All")
                {
                    if (subCriteria.Contains(_selectedValue))
                    {
                        SetChildVisibility(_selectedValue);
                    }
                    else
                    {
                        
                        _childLayout.IsVisible = false;
                        ResetChildRatingsFromCart();
                        SetChildRatingtoZero();
                        _autofoward = true;
                    }
                }

                Device.StartTimer(TimeSpan.FromSeconds(GlobalModel.TimeSpan), () =>
                {
                    if (_autofoward)
                    {
                        LoadNextPage();
                    }
                    _tapLocked = false;
                    _canLoadNext = true;
                    return false;
                });



            }

        }

        /// <summary>
        /// Childs the button tapped.
        /// </summary>
        /// <param name="heat_ChildModel">Heat child model.</param>
        private void ChildButtonTapped(Heat_ChildModel heat_ChildModel)
        {
            //check wheather parent has a feedback

            if (string.IsNullOrEmpty(_selectedValue))
            {
                Application.Current.MainPage.DisplayAlert("Attention!", "Please rate above question first.", "OK");
                return;
            }

            //set message 
            _messageLabel.Text = "Please Tap on next button to continue";


            int childRawId = Convert.ToInt32(heat_ChildModel.ItemID);
            int childColumnId = Convert.ToInt32(heat_ChildModel.ButtonModel.ID);
            Debug.WriteLine(heat_ChildModel.ItemID + " : " + heat_ChildModel.ButtonModel.ID);

            var childQId = "";

            if(children.ElementAt(0).Value.QType == "L")
            {
                childQId = children.ElementAt(childRawId + 1).Value.QId;
            }
            else
            {
                childQId = children.ElementAt(childRawId).Value.QId;
            }


            var childSelectedValue = (Convert.ToInt32(childColumnId) + 1).ToString();
            Debug.WriteLine(childQId + " : " + childSelectedValue);

            //Add selected ratings to cart
            AddChildFeedbackToCart(childQId, childSelectedValue);

            //current button row list
            var currentModel = childHeatLists[Convert.ToInt32(heat_ChildModel.ItemID)];


            //Button animations
            foreach (var item in currentModel.buttonList)
            {
                item.BackgroundColor = Color.White;
                item.TextColor = Color.Black;
            }

            int _seq = 4;

            foreach (var item in currentModel.buttonList)
            {
                item.BackgroundColor = GlobalModel.ColorListSeconary[_seq];
                item.TextColor = Color.White;

                if (item.Id == heat_ChildModel.ButtonModel.button.Id)
                {
                    //Auto scolling
                    _scrollView.ScrollToAsync(item, ScrollToPosition.Start, true);

                    //ending the button animations when selected is reached
                    break;
                }

                _seq--;
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
        /// Nexts the button tapped.
        /// </summary>
        private void NextButtonTapped()
        {
            if (_canLoadNext)
            {
                CheckNextHasFeedback();

                bool childFeedbacks = CheckIsGivenFeedbackToChild();
                bool isChildVisible = _childLayout.IsVisible;

                if(!childFeedbacks && isChildVisible)
                {
                    Application.Current.MainPage.DisplayAlert("Attention!", "Please give your feedbacks to continue.", "OK");
                    return;
                }

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

            if (children != null)
            {
                if (children.Count > 0)
                {
                    foreach (var item in children)
                    {
                        if (item.Value.QType != "L")
                        {
                            AddToFeedbackCart(item.Value.QId, "0");
                        }
                    }
                }
            }

            PageLoadHandler.LoadNextPage(_navigation, _currQuestionindex, "0");
            _canLoadNext = true;
        }

        /// <summary>
        /// Adds to feedback cart.
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
        /// <param name="skipped">Skipped.</param>
        private void AddToFeedbackCart(string skipped)
        {
            if (FeedbackCart.RatingNVC[_Questions.QId] == null)
            {
                FeedbackCart.RatingNVC.Add(_Questions.QId, skipped);
            }
            else
            {
                FeedbackCart.RatingNVC.Remove(_Questions.QId);
                AddToFeedbackCart("0");
            }
        }

        /// <summary>
        /// Adds to feedback cart.
        /// </summary>
        /// <param name="qid">Qid.</param>
        /// <param name="skipped">Skipped.</param>
        private void AddToFeedbackCart(string qid, string skipped)
        {
            if (FeedbackCart.RatingNVC[qid] == null)
            {
                FeedbackCart.RatingNVC.Add(qid, skipped);
            }
            else
            {
                FeedbackCart.RatingNVC.Remove(qid);
                AddToFeedbackCart(qid, "0");
            }
        }

        /// <summary>
        /// Adds the child feedback to cart.
        /// </summary>
        /// <param name="childId">Child identifier.</param>
        /// <param name="childRatingValue">Child rating value.</param>
        private void AddChildFeedbackToCart(string childId, string childRatingValue)
        {
            if (FeedbackCart.RatingNVC[childId] == null)
            {
                FeedbackCart.RatingNVC.Add(childId, childRatingValue);
            }
            else
            {
                FeedbackCart.RatingNVC.Remove(childId);
                AddChildFeedbackToCart(childId, childRatingValue);
            }
        }

        /// <summary>
        /// Restores the feedback data.
        /// </summary>
        private void RestoreFeedbackData()
        {
            string previousFeedback = FeedbackCart.RatingNVC[_Questions.QId];

            if (previousFeedback != null && previousFeedback != (0).ToString())
            {
                _selectedValue = previousFeedback;

                foreach (var item in GlobalModel.HeatButonList)
                {
                    item.BackgroundColor = Color.White;
                    item.TextColor = Color.Black;
                }

                int _seq = GlobalModel.HeatButonList.Count-1;

                if (_seq > 5)
                {
                    _colorList = GlobalModel.ColorList;
                }
                else
                {
                    _colorList = GlobalModel.ColorListSeconary;
                }

                foreach (var item in GlobalModel.HeatButonList)
                {
                    item.BackgroundColor = _colorList[_seq];
                    item.TextColor = Color.White;

                    if ((GlobalModel.HeatButonList.Count - _seq).ToString() == _selectedValue)
                    {
                        break;
                    }

                    _seq--;
                }


            }
        }

        /// <summary>
        /// Restores the child feedback data.
        /// </summary>
        private void RestoreChildFeedbackData()
        {
            try
            {
                string[] childArray = new string[children.Count];

                int x = 0;
                foreach (var item in children)
                {
                    if (item.Value.QType != "L")
                    {
                        childArray[x] = item.Value.QId;
                        x++;
                    }
                }

                int y = 0;
                foreach (var item in childArray)
                {
                    string previousChildRating = FeedbackCart.RatingNVC[item];

                    if (previousChildRating != null && previousChildRating != (0).ToString() )
                    {
                        _childLayout.IsVisible = true;// Visible child layout if feedbacks already given

                        //Button animations
                        var currentModel = childHeatLists[y];

                        foreach (var items in currentModel.buttonList)
                        {   items.BackgroundColor = Color.White;
                            items.TextColor = Color.Black;
                        }

                        int _seqe = 4;

                        foreach (var items in currentModel.buttonList)
                        {
                            items.BackgroundColor = GlobalModel.ColorListSeconary[_seqe];
                            items.TextColor = Color.White;

                            if ((currentModel.buttonList.Count - _seqe).ToString() == previousChildRating)
                            {
                                break;
                            }

                            _seqe--;
                        }
                    }
                    y++;
                }
            }
            catch(Exception)
            {
                Console.WriteLine("Child restore ratings failed");
            }
        }

        /// <summary>
        /// Checks the next has feedback.
        /// </summary>
        /// <returns><c>true</c>, if next has feedback was checked, <c>false</c> otherwise.</returns>
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

        /// <summary>
        /// Checks the is given feedback to child.
        /// </summary>
        /// <returns><c>true</c>, if is given feedback to child was checked, <c>false</c> otherwise.</returns>
        private bool CheckIsGivenFeedbackToChild()
        {
            try
            {
                var childQId = "";

                for (int i = 0; i < children.Count; i++)
                {
                    if (children.ElementAt(0).Value.QType == "L")
                    {
                        childQId = children.ElementAt(i + 1).Value.QId;
                        i++;
                    }
                    else
                    {
                        childQId = children.ElementAt(i).Value.QId;
                    }

                    var feedback = FeedbackCart.RatingNVC[childQId];

                    if (string.IsNullOrEmpty(feedback))
                    {
                        return false;
                    }
                }
                return true;
            }
            catch(Exception)
            {
                return true;
            }
        }

        /// <summary>
        /// Resets the child ratings from cart.
        /// </summary>
        private void ResetChildRatingsFromCart()
        {
            try
            {
                foreach (var item in children)
                {
                    FeedbackCart.RatingNVC.Remove(item.Value.QId);
                }
            }
            catch(Exception)
            {
                Console.WriteLine("Reset Child Exeption");
            }
        }

        private void SetChildRatingtoZero()
        {
            try
            {
                foreach (var item in children)
                {
                    if (item.Value.QType != "L")
                    {
                        AddChildFeedbackToCart(item.Value.QId, "0");
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Zero rating Set Child Exeption");
            }
        }

    }
}
