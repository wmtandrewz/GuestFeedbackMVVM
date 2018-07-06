using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CGFSMVVM.Helpers;
using CGFSMVVM.Models;
using CGFSMVVM.Services;
using Microsoft.AppCenter.Analytics;
using Newtonsoft.Json;

namespace CGFSMVVM.DataParsers
{
    /// <summary>
    /// Question json deserializer.
    /// </summary>
    public static class QuestionJsonDeserializer
    {
        public static string CurrQuestion;

        private static Dictionary<string, QuestionsModel> HotelQuestionDictionary = new Dictionary<string, QuestionsModel>();

        private static List<string> QuestionNumberList = new List<string>();

        private static Dictionary<string, Dictionary<string, QuestionsModel>> ChildQuestionDictionary = new Dictionary<string, Dictionary<string, QuestionsModel>>();

        /// <summary>
        /// Deserializes the questions.
        /// </summary>
        /// <returns>The questions.<see cref="T:CGFSMVVM.Models.Questions"/> class</returns>
        public static async Task<bool> DeserializeQuestions()
        {
            HotelQuestionDictionary.Clear();
            ChildQuestionDictionary.Clear();
            QuestionNumberList.Clear();

            if (!string.IsNullOrEmpty(Settings.HotelIdentifier))
            {
                string _result = await APIGetServices.GetQuestionsFromAPI(Settings.HotelIdentifier, "en-US", "GC").ConfigureAwait(true);

                FeedbackCart._hotelIdentifier = Settings.HotelIdentifier;

                List<QuestionsModel> HotelQuestionsList = JsonConvert.DeserializeObject<List<QuestionsModel>>(_result);

                if (HotelQuestionsList != null)
                {
                    foreach (var item in HotelQuestionsList)
                    {
                        HotelQuestionDictionary.Add(item.QNo, item);
                        QuestionNumberList.Add(item.QNo);
                    }

                    FilterChildQuestions();

                    return true;
                }
            }
            else
            {
                return false;
            }
            return false;

        }

        /// <summary>
        /// Filters the child questions.
        /// </summary>
        private static void FilterChildQuestions()
        {
            try
            {
                var lastVal = HotelQuestionDictionary.Values.Last();
                int lastPageId = Convert.ToInt32(lastVal.PageId);

                for (int i = 1; i <= lastPageId; i++)
                {
                    var res = HotelQuestionDictionary.Where(x => Convert.ToInt32(x.Value.PageId) == i).ToDictionary(x => x.Key, x => x.Value);


                    if (res.Values.Count > 1)
                    {

                        var indexObj = res.First();

                        ChildQuestionDictionary.Add(indexObj.Value.QNo, res);

                        foreach (var item in res.Values)
                        {
                            if (item != res.Values.First())
                            {
                                QuestionNumberList.Remove(item.QNo);
                            }
                        }

                        if (res.First().Value.ParentQNo == null)
                        {
                            res.Remove(res.First().Value.QNo);
                        }
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Filter Questions");
            }


        }

        /// <summary>
        /// Gets the question by index number.
        /// </summary>
        /// <returns>The question. <see cref="T:CGFSMVVM.Models.Questions"/> class</returns>
        /// <param name="index">Index of required question</param>
        public static QuestionsModel GetQuestion(string index)
        {
            return HotelQuestionDictionary[index];
        }

        /// <summary>
        /// Gets the first question.
        /// </summary>
        /// <returns>The first question. <see cref="T:CGFSMVVM.Models.Questions"/></returns>
        /// <param name="index">Index.</param>
        public static QuestionsModel GetFirstQuestion(int index)
        {
            CurrQuestion = QuestionNumberList[index];

            return HotelQuestionDictionary[QuestionNumberList[index]];           


        }

        /// <summary>
        /// Gets the previous question.
        /// </summary>
        /// <returns>The previous question. <see cref="T:CGFSMVVM.Models.Questions"/></returns>
        /// <param name="index">Index.</param>
        public static QuestionsModel GetPreviousQuestion(string index)
        {
            return HotelQuestionDictionary[index];
        }

        /// <summary>
        /// Gets the next question by prev quesion index.
        /// </summary>
        /// <returns>The next question.</returns>
        /// <param name="prvIndex">Prv index. <see cref="T:CGFSMVVM.Models.Questions"/></param>
        public static QuestionsModel GetNextQuestion(string prvIndex)
        {

            var key = GetNextQuestionNumber(prvIndex);

            if (!string.IsNullOrEmpty(key))
            {
                return HotelQuestionDictionary[key];
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// Skips the dependant question.
        /// </summary>
        /// <returns>The dependant question. <see cref="T:CGFSMVVM.Models.Questions"/></returns>
        /// <param name="prvIndex">Prv index.</param>
        public static QuestionsModel SkipDependantQuestion(string prvIndex)
        {
            var nextQ = GetNextQuestion(prvIndex);
            string key = "";

            if (nextQ != null)
            {
                //Add skipped to feedback cart

                AddSkippedToFeedbackCart(nextQ.QNo);
                AddToFeedbackCart(nextQ, "0");

                //Get after skiiped question key
                key = GetNextQuestionNumber(nextQ.QNo);
            }

            if (!string.IsNullOrEmpty(key))
            {
                return HotelQuestionDictionary[key];
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// Gets the next question number.
        /// </summary>
        /// <returns>The next question number.</returns>
        /// <param name="prvIndex">Prv index.</param>
        public static string GetNextQuestionNumber(string prvIndex)
        {
            for (int i = 0; i < QuestionNumberList.Count; i++)
            {
                if (QuestionNumberList[i] == prvIndex && QuestionNumberList.Count > (i + 1))
                {
                    CurrQuestion = QuestionNumberList[i + 1];
                    return QuestionNumberList[i + 1];
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the index of the current question.
        /// </summary>
        /// <returns>The current question index.</returns>
        public static int GetCurrentQuestionIndex()
        {
            if (QuestionNumberList != null)
            {
                for (int i = 0; i < QuestionNumberList.Count; i++)
                {
                    if (CurrQuestion == QuestionNumberList[i])
                    {
                        return i + 1;
                    }
                }
            }

            return 0;
        }

        /// <summary>
        /// Gets the question count.
        /// </summary>
        /// <returns>The question count.</returns>
        public static int GetQuestionCount()
        {
            if (QuestionNumberList != null)
            {
                return QuestionNumberList.Count;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Gets the child question set.
        /// </summary>
        /// <returns>The child question set.</returns>
        /// <param name="parentQNo">Parent QN.</param>
        public static Dictionary<string, QuestionsModel> GetChildQuestionSet(string parentQNo)
        {
            try
            {
                return ChildQuestionDictionary[parentQNo];
            }
            catch (Exception)
            {
                return null;
            }
        }


        private static void AddSkippedToFeedbackCart(string Qno)
        {
            try
            {
                var ChildrenSet = GetChildQuestionSet(Qno);

                foreach (var item in ChildrenSet)
                {
                    if (item.Value.QType != "L")
                    {
                        AddToFeedbackCart(item.Value, "0");
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("No Childrens for" + Qno);
            }

        }

        private static void AddToFeedbackCart(QuestionsModel model, string skipped)
        {
            var nvc = FeedbackCart.RatingNVC;

            switch (model.QType)
            {
                
                case "C":
                    nvc = FeedbackCart.CommentNVC;
                    return;
                case "O":
                    nvc = FeedbackCart.OtherNVC;
                    skipped = "-1";
                    break;
                default:
                    nvc = FeedbackCart.RatingNVC;
                    break;
            }


            if (nvc[model.QId] == null)
            {
                nvc.Add(model.QId, skipped);
            }
            else
            {
                nvc.Remove(model.QId);
                AddToFeedbackCart(model, "0");
            }
        }

    }
}
