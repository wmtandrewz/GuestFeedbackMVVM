using System;
using System.Collections.Generic;
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
    public class NewsLetterViewMoidel
    {
        public ICommand TappedCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand NextCommand { get; }

        public INavigation _navigation { get; }
        private List<Image> DualOptionList = new List<Image>();
        private bool _tapLocked = false;

        public NewsLetterViewMoidel(INavigation iNavigation)
        {
            this._navigation = iNavigation;

            TappedCommand = new Command<DualOptionModel>(async (dualOptionModel) => await IconTapped(dualOptionModel).ConfigureAwait(true));
            BackCommand = new Command(BackButtonTapped);
            NextCommand = new Command(NextButtonTapped);

            LoadImages();

        }

        private void NextButtonTapped()
        {
            Application.Current.MainPage.DisplayAlert("Attention!", "Please give your feedback to continue.", "OK");
        }

        private void LoadImages()
        {
            foreach (var item in GlobalModel.DualOptionList)
            {
                this.DualOptionList.Add(item);
            }
        }

        async Task IconTapped(DualOptionModel dualOptionModel)
        {
          
            if (!_tapLocked)
            {
                _tapLocked = true;

                if (dualOptionModel.ID == "1")
                {
                    DualOptionList[1].Opacity = 0.5;
                    DualOptionList[0].Opacity = 1;

                    await dualOptionModel.image.ScaleTo(2, 150);
                    await dualOptionModel.image.ScaleTo(1, 150);

                    await _navigation.PushAsync(new ContactDetailsView());
                    _tapLocked = false;
                }

                if (dualOptionModel.ID != "1")
                {
                    DualOptionList[1].Opacity = 1;
                    DualOptionList[0].Opacity = 0.5;

                    await dualOptionModel.image.ScaleTo(2, 150);
                    await dualOptionModel.image.ScaleTo(1, 150);

                    await _navigation.PushAsync(new FinishPageView());
                    _tapLocked = false;
                }

            }
        }

        private void BackButtonTapped()
        {
            _navigation.PopAsync();
        }

    }
}
