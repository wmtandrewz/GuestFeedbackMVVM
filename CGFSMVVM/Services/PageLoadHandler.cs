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
                //QuestionsModel currQuestion = QuestionJsonDeserializer.GetQuestion(_currQuestionindex);

                /////<summary>
                ///// If curent question object dependantQNo is empty loads the next page sequently
                ///// else if (If there is dependant Quesion) curent question object DependantQValue contains current dependant value, loads the dependant question
                ///// else skip dependant quesion
                ///// </summary>
                //if (currQuestion.DependantQNo == null)
                //{
                //    _nextQuestion = QuestionJsonDeserializer.GetNextQuestion(_currQuestionindex);
                //}
                //else if (currQuestion.DependantQValue.Contains(_selectedValue))
                //{
                //    _nextQuestion = QuestionJsonDeserializer.GetQuestion(currQuestion.DependantQNo);
                //}
                //else
                //{
                //    _nextQuestion = QuestionJsonDeserializer.SkipDependantQuestion(_currQuestionindex);
                //}

                //=====
                _nextQuestion = HandleDependancyQuestions(_currQuestionindex);

                //=======


                LoadPage(_navigation, _currQuestionindex);
            }
            catch (Exception)
            {
                _navigation.PushAsync(new NewsLetterView());
            }

        }


        private static QuestionsModel HandleDependancyQuestions(string currQIndex)
        {
            try
            {
                QuestionsModel currQuestion = QuestionJsonDeserializer.GetQuestion(currQIndex);
                QuestionsModel nextQuestion = QuestionJsonDeserializer.GetNextQuestion(currQIndex);

                var dependancyQNo = nextQuestion.DependantQNo;

                if(string.IsNullOrEmpty(dependancyQNo))
                {
                    return QuestionJsonDeserializer.GetNextQuestion(currQIndex);
                }

                QuestionsModel dependantParent = QuestionJsonDeserializer.GetQuestion(dependancyQNo);

                var dependancyQid = dependantParent.QId;
                var dependancyCriteria = nextQuestion.DependantQValue;
                var dependancyQType = dependantParent.QType;
                var dependancyQuestionRating = string.Empty;

                if (dependancyQType == null)
                {
                    dependancyQuestionRating = FeedbackCart.RatingNVC[dependancyQid];
                }
                else if (dependancyQType == "O")
                {
                    dependancyQuestionRating = FeedbackCart.OtherNVC[dependancyQid];
                }

                if(string.IsNullOrEmpty(dependancyQuestionRating))
                {
                    var afterSkippedQuestion = QuestionJsonDeserializer.SkipDependantQuestion(currQIndex);

                    if (afterSkippedQuestion.DependantQNo != null)
                    {
                        return HandleDependancyQuestions(nextQuestion.QNo);
                    }
                    else
                    {
                        return afterSkippedQuestion;
                    }
                }



                if (dependancyCriteria.Contains(dependancyQuestionRating) || dependancyQuestionRating.Contains(dependancyCriteria))
                {
                    return QuestionJsonDeserializer.GetNextQuestion(currQIndex);
                }
                else
                {
                    var afterSkippedQuestion = QuestionJsonDeserializer.SkipDependantQuestion(currQIndex);

                    if (afterSkippedQuestion.DependantQNo != null)
                    {
                        return HandleDependancyQuestions(nextQuestion.QNo);
                    }
                    else
                    {
                        return afterSkippedQuestion;
                    }
                }
            }
            catch(Exception)
            {
                return QuestionJsonDeserializer.SkipDependantQuestion(currQIndex);
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
                        if (_nextQuestion.RatingScale != null)
                        {
                            var ratingScale = _nextQuestion.RatingScale.Count;
                            _navigation.PushAsync(new HeatBarView(_currQuestionindex, _nextQuestion.QNo, ratingScale));
                        }

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
