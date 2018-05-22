using System;
using Foundation;
using KeyboardOverlap.Forms.Plugin.iOSUnified;
using UIKit;
using UserNotifications;

namespace CGFSMVVM.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {

        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            KeyboardOverlapRenderer.Init();

            return base.FinishedLaunching(uiApplication, launchOptions);
        }


    }
}
