using System;
using CGFSMVVM.DataParsers;
using CGFSMVVM.Models;
using CGFSMVVM.Views;
using Xamarin.Forms;

namespace CGFSMVVM.Services
{
    /// <summary>
    /// Page load handler.
    /// </summary>
    public static class PageLoadHandler
    {

        private static QuestionsModel _nextQuestion;

        /// <summary>
        /// Loads the next page.
        /// </summary>
        /// <param name="_navigation">Navigation stack reference</param>
        /// <param name="_currQuestionindex">Current questionindex.</param>
        /// <param name="_selectedValue">Selected value.</param>
        public static void LoadNextPage(INavigation _navigation, string _currQuestionindex, string _selectedValue)
        {

            GlobalModel.CleanGlobalModel();

            try
            {
                QuestionsModel currQuestion = QuestionJsonDeserializer.GetQuestion(_currQuestionindex);

                ///<summary>
                /// If curent question object dependantQNo is empty loads the next page sequently
                /// else if (If there is dependant Quesion) curent question object DependantQValue contains current dependant value, loads the dependant question
                /// else skip dependant quesion
                /// </summary>
                if (currQuestion.DependantQNo == null)
                {
                    _nextQuestion = QuestionJsonDeserializer.GetNextQuestion(_currQuestionindex);
                }
                else if (currQuestion.DependantQValue.Contains(_selectedValue))
                {
                    _nextQuestion = QuestionJsonDeserializer.GetQuestion(currQuestion.DependantQNo);
                }
                else
                {
                    _nextQuestion = QuestionJsonDeserializer.SkipDependantQuestion(_currQuestionindex);
                }


                LoadPage(_navigation, _currQuestionindex);
            }
            catch (Exception)
            {
                _navigation.PushAsync(new NewsLetterView());
            }

        }

        /// <summary>
        /// Loads the first page.
        /// </summary>
        /// <param name="_navigation">Navigation stack reference</param>
        /// <param name="_currQuestionindex">Current questionindex.</param>
        private static void LoadPage(INavigation _navigation, string _currQuestionindex)
        {
            switch (_nextQuestion.QType)
            {
                case null:
                    if (_nextQuestion.UIControl == "sli")
                    {
                        _navigation.PushAsync(new HeatBarView(_currQuestionindex, _nextQuestion.QNo));
                    }
                    else
                    {
                        _navigation.PushAsync(new EmojiRatingView(_currQuestionindex, _nextQuestion.QNo));
                    }

                    break;

                case "Y":
                    _navigation.PushAsync(new DualOptionView(_currQuestionindex, _nextQuestion.QNo));
                    break;

                case "O":
                    if (_nextQuestion.UIControl == "cbl")
                    {
                        _navigation.PushAsync(new MultiSelectionView(_currQuestionindex, _nextQuestion.QNo));
                    }
                    else
                    {
                        _navigation.PushAsync(new MultiOptionView(_currQuestionindex, _nextQuestion.QNo));
                    }

                    break;

                case "C":
                    _navigation.PushAsync(new TextCommentView(_currQuestionindex, _nextQuestion.QNo));
                    break;
            }
        }

    }
}
