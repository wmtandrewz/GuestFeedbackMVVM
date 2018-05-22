using System;
using System.Text.RegularExpressions;
using System.Windows.Input;
using CGFSMVVM.Services;
using Xamarin.Forms;

namespace CGFSMVVM.ViewModels
{
    /// <summary>
    /// Contact details view model.
    /// </summary>
    public class ContactDetailsViewModel
    {
        public ICommand BackCommand { get; }
        public ICommand NextCommand { get; }
        public ICommand MobileTextChangedCommand { get; }
        public ICommand MailTextChangedCommand { get; }
        public INavigation _navigation { get; }

        private Entry _mobileEntry, _mailEntry;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CGFSMVVM.ViewModels.ContactDetailsViewModel"/> class.
        /// </summary>
        /// <param name="iNavigation">I navigation stack reference.</param>
        public ContactDetailsViewModel(INavigation iNavigation)
        {
            this._navigation = iNavigation;

            BackCommand = new Command(BackButtonTapped);
            NextCommand = new Command(NextButtonTapped);
            MobileTextChangedCommand = new Command<Entry>(MobileTextChanged);
            MailTextChangedCommand = new Command<Entry>(MailTextChanged);

        }

        /// <summary>
        /// If Mail Entry text changed.
        /// </summary>
        /// <param name="entry">Mail Entry.</param>
        private void MailTextChanged(Entry entry)
        {

            if (_mailEntry == null)
            {
                this._mailEntry = entry;
            }

            if (EmailValidator(entry.Text))
            {
                entry.BackgroundColor = Color.FromRgb(205, 255, 196);
            }
            else
            {
                entry.BackgroundColor = Color.FromRgb(255, 153, 168);
            }
        }

        /// <summary>
        /// Mobiles entry text changed.
        /// </summary>
        /// <param name="entry"> Mobile Entry.</param>
        private void MobileTextChanged(Entry entry)
        {
            if(_mobileEntry==null)
            {
                this._mobileEntry = entry;
            }

            if (entry.Text.Length > 13)
            {
                entry.Text = entry.Text.Remove(entry.Text.Length - 1);
            }

            if (MobileNumberValidator(entry.Text))
            {
                entry.BackgroundColor = Color.FromRgb(205, 255, 196); ;
            }
            else
            {
                entry.BackgroundColor = Color.FromRgb(255, 153, 168);
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
        /// Load FinishPage if next button tapped.
        /// </summary>
        private void NextButtonTapped()
        {
            if (!string.IsNullOrEmpty(_mobileEntry.Text) && !string.IsNullOrEmpty(_mailEntry.Text))
            {
                FeedbackCart._guestPhone = _mobileEntry.Text;
                FeedbackCart._guestEmail = _mailEntry.Text;
            }

            _navigation.PushAsync(new FinishPageView());
        }

        /// <summary>
        /// Mobile number validator.
        /// </summary>
        /// <returns><c>true</c>, if number validated, <c>false</c> otherwise.</returns>
        /// <param name="strNumber">mobile number.</param>
        private bool MobileNumberValidator(String strNumber)
        {
            //Regex mobilePattern = new Regex("(3|4|5|6|7|8|9){1}[0-9]{9}"); 

            Regex plus = new Regex("^[+][1-9][0-9]{9,13}$");
            Regex zero = new Regex("0[1-9][0-9]{10}$");
            Regex zeroes = new Regex("^00[1-9][0-9]{9,13}$");

            if (plus.IsMatch(strNumber) || zero.IsMatch(strNumber) || zeroes.IsMatch(strNumber))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// Email  validator.
        /// </summary>
        /// <returns><c>true</c>, if validated, <c>false</c> otherwise.</returns>
        /// <param name="strEmail"> email.</param>
        private bool EmailValidator(string strEmail)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(strEmail);
            if (match.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
