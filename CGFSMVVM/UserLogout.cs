using System;
using CGFSMVVM.Helpers;
using Xamarin.Forms;

namespace CGFSMVVM
{
    public class UserLogout
    {
        
		public async void logout()
        {
            //Delete Stored Setting
            Settings.Username = string.Empty;
            Settings.Password = string.Empty;
            Settings.AccessToken = string.Empty;
            Settings.HotelCode = string.Empty;
            Settings.Username = string.Empty;

            MessagingCenter.Send<UserLogout>(this, "logout");
        }
    }
}
