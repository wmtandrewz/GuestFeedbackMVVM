using System;
using System.Windows.Input;
using CGFSMVVM.DataParsers;
using CGFSMVVM.Services;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;

namespace CGFSMVVM.ViewModels
{
    /// <summary>
    /// Finish page view model.
    /// </summary>
    public class FinishPageViewModel
    {
        public ICommand FinishButtonCommand { get; }
        public ICommand PageAppearingCommand { get; }
        public INavigation Navigation { get; }
        public Button finishButton { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CGFSMVVM.ViewModels.FinishPageViewModel"/> class.
        /// </summary>
        /// <param name="navigation">Navigation.</param>
        public FinishPageViewModel(INavigation navigation)
        {
            this.Navigation = navigation;

            FinishButtonCommand = new Command(FinishButtonPressed);
            PageAppearingCommand = new Command<Button>(OnAppearing);
        }

        /// <summary>
        /// Save feedback data to API in page appearing
        /// </summary>
        /// <param name="button">Button.</param>
        private async void OnAppearing(Button button)
        {
            button.IsEnabled = false;

            try
            {
				bool responce = await APIPostServices.SaveFeedbackData();
                if(responce)
                {
                    button.IsEnabled = true;
                }
                else
                {
                    button.IsEnabled = true;
                }
            }
            catch(Exception exception)
            {
                Crashes.TrackError(exception);
            }
        }

        /// <summary>
        /// Clear the feedback data and reloanch the main page the button pressed.
        /// </summary>
        private void FinishButtonPressed()
        {
            FeedbackCart.ClearSavedData();
            Navigation.PopToRootAsync();
        }
    }
}
