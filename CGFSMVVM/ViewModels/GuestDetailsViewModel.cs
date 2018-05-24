using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CGFSMVVM.DataParsers;
using CGFSMVVM.Helpers;
using CGFSMVVM.Models;
using CGFSMVVM.Services;
using CGFSMVVM.Views;
using Xamarin.Forms;

namespace CGFSMVVM.ViewModels
{
    /// <summary>
    /// Guest details view model.
    /// </summary>
    public class GuestDetailsViewModel : INotifyPropertyChanged
    {
        public INavigation _navigation { get; }
        public ICommand EntryUnfocusedCommand { get; }
        public ICommand EntryCompletedCommand { get; }
        public ICommand EntryTextChangedCommand { get; }
        public ICommand SetFormLayoutInstance { get; }
        public string _reservationNumber,_mobileNumber,_emailAddress;
        public event PropertyChangedEventHandler PropertyChanged;
        private StackLayout _formLayout;
        private ActivityIndicator _indicator;
        private List<Label> _formChildList = new List<Label>();
        private List<Label> _guestsList = new List<Label>();
        private string _currQuesNo = "GuestDetailView";

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CGFSMVVM.ViewModels.GuestDetailsViewModel"/> class.
        /// </summary>
        /// <param name="navigation">Navigation.</param>
        public GuestDetailsViewModel(INavigation navigation)
        {
            this._navigation = navigation;

            EntryUnfocusedCommand = new Command<Entry>(EntryUnfocused);
            EntryCompletedCommand = new Command<Entry>(EntryCompleted);
            EntryTextChangedCommand = new Command<Entry>(EntryTextChanged);
            SetFormLayoutInstance = new Command<StackLayout>(SetFormLayerIns);

        }

        /// <summary>
        /// Gets or sets the reservation number.
        /// </summary>
        /// <value>The reservation number.</value>
        public string ReservationNumber
        {
            set
            {
                if (_reservationNumber != value)
                {
                    _reservationNumber = value;
                    OnPropertyChanged("ReservationNumber");

                }
            }
            get
            {
                return _reservationNumber;
            }
        }

        /// <summary>
        /// Gets or sets the mobile number.
        /// </summary>
        /// <value>The mobile number.</value>
        public string MobileNumber
        {
            set
            {
                if (_mobileNumber != value)
                {
                    _mobileNumber = value;
                    OnPropertyChanged("MobileNumber");

                }
            }
            get
            {
                return _mobileNumber;
            }
        }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        /// <value>The email address.</value>
        public string EmailAddress
        {
            set
            {
                if (_emailAddress != value)
                {
                    _emailAddress = value;
                    OnPropertyChanged("EmailAddress");

                }
            }
            get
            {
                return _emailAddress;
            }
        }

        /// <summary>
        /// Room Number Enrty text changed.
        /// </summary>
        /// <param name="enrty">Room Number Enrty.</param>
        private void EntryTextChanged(Entry enrty)
        {
            if (!enrty.Text.All(char.IsDigit))
            {
                enrty.Text = enrty.Text.Remove(enrty.Text.Length - 1);
            }

            if (enrty.Text.Length > 4)
            {
                enrty.Text = enrty.Text.Remove(enrty.Text.Length - 1);
            }
        }

        /// <summary>
        /// Room Number Enrty completed.
        /// </summary>
        /// <param name="entry">Room Number Enrty.</param>
        private void EntryCompleted(Entry entry)
        {
            entry.Unfocus();
        }

        /// <summary>
        /// Sets the form layer ins.
        /// </summary>
        /// <param name="formLayout">Form layout.</param>
        private void SetFormLayerIns(StackLayout formLayout)
        {
            this._formLayout = formLayout;
        }

        /// <summary>
        /// Entries the unfocused.
        /// </summary>
        /// <param name="roomNoEntry">Room no entry.</param>
        private async void EntryUnfocused(Entry roomNoEntry)
        {
            RemoveFormLayoutChildren();

            GenerateIndicator();

            _indicator.IsVisible = true;
            _indicator.IsRunning = true;

            string date = DateTime.Now.ToString("yyyy-MM-dd");

			Dictionary<string, ReservationDetailsModel> ReservationDetailsDictionary = await ReservationDetailsDeserializer.DeserializeReservationDetails(Settings.HotelCode, roomNoEntry.Text, date).ConfigureAwait(true);

            if (ReservationDetailsDictionary != null)
            {
                ReservationNumber = ReservationDetailsDictionary["1"].BookingId;
                MobileNumber = ReservationDetailsDictionary["1"].Telephone;
                EmailAddress = ReservationDetailsDictionary["1"].Email;

                FeedbackCart._resNum = ReservationDetailsDictionary["1"].BookingId;
                FeedbackCart._country = ReservationDetailsDictionary["1"].Country;
                FeedbackCart._arrDate = ReservationDetailsDictionary["1"].ArrivalDate;
                FeedbackCart._depDate = ReservationDetailsDictionary["1"].DepartureDate;
                FeedbackCart._guestPhone = ReservationDetailsDictionary["1"].Mobile;
                FeedbackCart._guestEmail = ReservationDetailsDictionary["1"].Email;

                RemoveFormLayoutChildren();

                GenerateGuestNameLabel();

                for (int i = 1; i <= ReservationDetailsDictionary.Count; i++)
                {
                    GenerateGuestNameLabels(ReservationDetailsDictionary[i.ToString()].Name,roomNoEntry.Text,ReservationDetailsDictionary[i.ToString()].GuestId,ReservationNumber);
                }

                if(string.IsNullOrEmpty(ReservationDetailsDictionary["1"].BookingId))
                {
                    await Application.Current.MainPage.DisplayAlert("No Reservations!", "Please check your room number.", "OK");
                    roomNoEntry.Focus();
                    roomNoEntry.Text = "";
                    ReservationNumber = "";
                    MobileNumber = "";
                    EmailAddress="";

                }

                Console.WriteLine(ReservationNumber);

                _indicator.IsVisible = false;
                _indicator.IsRunning = false;
                _formLayout.Children.Remove(_indicator);
            }
            else
            {
                roomNoEntry.Focus();
                _indicator.IsVisible = false;
                _indicator.IsRunning = false;
                _formLayout.Children.Remove(_indicator);
            }

        }

        /// <summary>
        /// Ons the property changed.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Generates the guest name labels.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="roomNumber">Room number.</param>
        /// <param name="guestID">Guest identifier.</param>
        /// <param name="resNo">Res no.</param>
        private void GenerateGuestNameLabels(string name,string roomNumber,string guestID,string resNo)
        {
            var label = new Label()
            {
                BackgroundColor = Color.Purple,
                TextColor = Color.White,
                FontSize = 20,
                Text = name,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                HeightRequest = 40

            };

            var nameTapRecognizer = new TapGestureRecognizer();
            nameTapRecognizer.Tapped += async (s, e) =>
            {
                ResetSelectedGuesteffects();

                label.TextColor = Color.LightGreen;
                label.Opacity = 1;

                await label.ScaleTo(2, 200, Easing.CubicIn);
                await label.ScaleTo(1, 200, Easing.CubicOut);

				bool isGivenFeedback = await IsFeedbackGiven(Settings.HotelIdentifier, resNo, guestID);

                if(!isGivenFeedback)
                {
                    FeedbackCart._roomNum = roomNumber;
                    FeedbackCart._guestID = guestID;
                    FeedbackCart._guestName = label.Text;
                    FeedbackCart._startTime = DateTime.Now.ToString();

                    LoadQuestionViews();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert($"Hi!\n {name} ", "You have already given a feedback", "OK");    
                }

                Console.WriteLine(label.Text + ":" + guestID);
            };

            label.GestureRecognizers.Add(nameTapRecognizer);
                 
            _formChildList.Add(label);
            _guestsList.Add(label);
            _formLayout.Children.Add(label);
        }

        /// <summary>
        /// Generates the guest name label.
        /// </summary>
        private void GenerateGuestNameLabel()
        {
            var guestNameLabel = new Label
            {
                Text = "Tap On Your Name",
                FontSize = 18,
                TextColor = Color.White,
                Opacity = 0.5,
                VerticalOptions = LayoutOptions.Start,
                VerticalTextAlignment = TextAlignment.End,
                HeightRequest = 40,
            };

            _formChildList.Add(guestNameLabel);
            _formLayout.Children.Add(guestNameLabel);
        }

        /// <summary>
        /// Removes the form layout children.
        /// </summary>
        private void RemoveFormLayoutChildren()
        {
            
            if (_formChildList!=null)
            {
                foreach (var item in _formChildList)
                {
                    _formLayout.Children.Remove(item);
                }

                _formChildList.Clear();
                _guestsList.Clear();
            }
        }

        /// <summary>
        /// Resets the selected guesteffects.
        /// </summary>
        private void ResetSelectedGuesteffects()
        {
            if(_guestsList!=null)
            {
                foreach (var item in _guestsList)
                {
                    item.TextColor = Color.White;
                    item.Opacity = 0.5;
                }
            }
        }

        /// <summary>
        /// Loads the question views.
        /// </summary>
        private void LoadQuestionViews()
        {
            GlobalModel.CleanGlobalModel();

            QuestionsModel _nextQuestion = QuestionJsonDeserializer.GetFirstQuestion(0);

            if (_nextQuestion.QType == "")
            {
                if (_nextQuestion.DisplayType == "Slider")
                {
                    _navigation.PushAsync(new HeatBarView(_currQuesNo, _nextQuestion.QNo));
                }
                else
                {
                    _navigation.PushAsync(new EmojiRatingView(_currQuesNo, _nextQuestion.QNo));
                }

            }
            else if (_nextQuestion.QType == "Y")
            {
                _navigation.PushAsync(new DualOptionView(_currQuesNo, _nextQuestion.QNo));

            }
            else if (_nextQuestion.QType == "O")
            {
                if (_nextQuestion.UIControl == "cbl")
                {
                    _navigation.PushAsync(new MultiSelectionView(_currQuesNo, _nextQuestion.QNo));
                }
                else
                {
                    _navigation.PushAsync(new MultiOptionView(_currQuesNo, _nextQuestion.QNo));
                }

            }
            else if (_nextQuestion.QType == "C")
            {
                _navigation.PushAsync(new TextCommentView(_currQuesNo, _nextQuestion.QNo));
            }
        }

        private void GenerateIndicator()
        {
            _indicator = new ActivityIndicator()
            {
                IsVisible = false,
                Color = Color.LightBlue,
                IsRunning = false,
                HeightRequest = 100,
                Scale = 3

            };
            _formLayout.Children.Add(_indicator);
        }
    
        private async Task <bool> IsFeedbackGiven(string hotelCode, string resNo, string guestID)
        {
            string responce = await APIGetServices.GetIsFeedbackGiven(hotelCode, resNo, guestID);

            if(!string.IsNullOrEmpty(responce))
            {
                if(responce == "true")
                {
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}
