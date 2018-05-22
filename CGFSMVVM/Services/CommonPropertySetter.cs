using System;
using Xamarin.Forms;

namespace CGFSMVVM.Services
{
    /// <summary>
    /// Common property setter.
    /// </summary>
    public static class CommonPropertySetter
    {
        /// <summary>
        /// Sets the message label text for the question pages.
        /// </summary>
        /// <param name="label">Label.</param>
        /// <param name="isOptional">If set to <c>true</c> is optional.</param>
        public  static void SetMessageLabelText(Label label,bool isOptional)
        {
            if (isOptional)
            {
                label.Text = "Please Tap on Next Button to Skip";
            }
            else
            {
                label.Text = "This Feedback is Mandatory";
            }
        }

        /// <summary>
        /// Sets the question label text.
        /// </summary>
        /// <param name="label">Label.</param>
        /// <param name="question">Question.</param>
        public static void SetQuestionLabelText(Label label,string question){
            label.Text = question;
        }
    }
}
