using System;
using System.Text;
using System.Threading.Tasks;
using CGFSMVVM.iOS.Services;
using Xamarin.Forms;
using Newtonsoft.Json;
using CGFSMVVM.Helpers;
using System.Threading;
using Microsoft.AppCenter.Crashes;

namespace CGFSMVVM.Services
{
    /// <summary>
    /// Native camera.
    /// </summary>
    public static class NativeCamera
    {

        private static CameraStream cameraStream;
        private static bool _isLocked = false;

        static NativeCamera()
        {
            InitCamera();
        }

        /// <summary>
        /// Inits the camera from iOS Dependancy.
        /// </summary>
        public static void InitCamera()
        {
            
            try
            {
                cameraStream = DependencyService.Get<CameraStream>();
                cameraStream.AuthorizeCameraUse();
                cameraStream.SetupLiveCameraStream();
                cameraStream.SetFrontCam();
            }
            catch(Exception exception)
            {
                //Thread.CurrentThread.Abort();
                Crashes.TrackError(exception);
            }
        }

        /// <summary>
        /// Gets the guest image.
        /// </summary>
        /// <returns>The guest image.</returns>
        public static async Task <byte[]> GetGuestImage()
        {
            try
            {
				if (Settings.AVStreamStat)
				{
					byte[] imageData = await cameraStream.TakePhoto().ConfigureAwait(false);

					if (imageData != null)
					{
						return imageData;
					}
				}
			}
			catch (Exception exception)
			{
				Crashes.TrackError(exception);
            }


            return null;
        }

        /// <summary>
        /// Uploads the image.
        /// </summary>
        public static async void UploadImage()
        {
            if (Settings.AVStreamStat)
            {
                if (!_isLocked)
                {
                    _isLocked = true;

                    byte[] guestImage = await GetGuestImage().ConfigureAwait(false);

                    if (Settings.EmailServiceStat)
                    {
                        await MailSender.SendsmtpMail("Image recieved", "Guest Feedback Recieved", guestImage, FeedbackCart._hotelCode, FeedbackCart._resNum);
                    }

                    if (guestImage != null)
                    {
                        FeedbackCart._guestImage = guestImage;

                        FTPService.UploadImageHttpPost(guestImage, FeedbackCart._hotelCode, FeedbackCart._resNum, FeedbackCart._guestID);

                        _isLocked = false;
                    }
                }
            }
        }
    }
}
