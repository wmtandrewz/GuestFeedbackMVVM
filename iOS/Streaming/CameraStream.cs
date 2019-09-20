using System;
using System.IO;
using System.Threading.Tasks;
using AVFoundation;
using Foundation;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(CGFSMVVM.iOS.Streaming.CameraStream))]

namespace CGFSMVVM.iOS.Streaming
{
    public class CameraStream
    {

        AVCaptureSession captureSession;
        AVCaptureDeviceInput captureDeviceInput;
        AVCaptureStillImageOutput stillImageOutput;

		AVAudioPlayer aVPlayer;
        
        public CameraStream()
        {
            AuthorizeCameraUse();

        }

        public async Task<byte[]> TakePhoto()
        {
			var session = AVAudioSession.SharedInstance();
			session.SetCategory(AVAudioSessionCategory.Record, AVAudioSessionCategoryOptions.DuckOthers);
			session.SetActive(true);

			NSError nSError;
			aVPlayer =new AVAudioPlayer(new NSUrl("Sounds/5minsilence.mp3"),"mp3",out nSError);
			aVPlayer.FinishedPlaying += delegate {
				aVPlayer = null;
            };
			aVPlayer.Volume = 10f;
            
			aVPlayer.Play();
         
            var videoConnection = stillImageOutput.ConnectionFromMediaType(AVMediaType.Video);
            var sampleBuffer = await stillImageOutput.CaptureStillImageTaskAsync(videoConnection);

            var jpegImageAsNsData = AVCaptureStillImageOutput.JpegStillToNSData(sampleBuffer);
            var jpegAsByteArray = jpegImageAsNsData.ToArray();

            return jpegAsByteArray;
        }


        public async void AuthorizeCameraUse()
        {
            try
            {
				
              
                var authorizationStatus = AVCaptureDevice.GetAuthorizationStatus(AVMediaType.Video);

                if (authorizationStatus != AVAuthorizationStatus.Authorized)
                {
                    await AVCaptureDevice.RequestAccessForMediaTypeAsync(AVMediaType.Video).ConfigureAwait(true);

                }
            }
            catch(Exception)
            {
            }
        }

        public void SetupLiveCameraStream()
        {
            captureSession = new AVCaptureSession();

            var captureDevice = AVCaptureDevice.GetDefaultDevice(AVMediaType.Video);
            ConfigureCameraForDevice(captureDevice);
            captureDeviceInput = AVCaptureDeviceInput.FromDevice(captureDevice);
            captureSession.AddInput(captureDeviceInput);

            var dictionary = new NSMutableDictionary();
            dictionary[AVVideo.CodecKey] = new NSNumber((int)AVVideoCodec.JPEG);
            stillImageOutput = new AVCaptureStillImageOutput()
            {
                OutputSettings = new NSDictionary()
            };

            captureSession.AddOutput(stillImageOutput);
            captureSession.StartRunning();

        }

        public void ConfigureCameraForDevice(AVCaptureDevice device)
        {
            var error = new NSError();
            if (device.IsFocusModeSupported(AVCaptureFocusMode.ContinuousAutoFocus))
            {
                device.LockForConfiguration(out error);
                device.FocusMode = AVCaptureFocusMode.ContinuousAutoFocus;
                device.UnlockForConfiguration();
            }
            else if (device.IsExposureModeSupported(AVCaptureExposureMode.ContinuousAutoExposure))
            {
                device.LockForConfiguration(out error);
                device.ExposureMode = AVCaptureExposureMode.ContinuousAutoExposure;
                device.UnlockForConfiguration();
            }
            else if (device.IsWhiteBalanceModeSupported(AVCaptureWhiteBalanceMode.ContinuousAutoWhiteBalance))
            {
                device.LockForConfiguration(out error);
                device.WhiteBalanceMode = AVCaptureWhiteBalanceMode.ContinuousAutoWhiteBalance;
                device.UnlockForConfiguration();
            }
        }

        public void SetFrontCam()
        {
            var devicePosition = AVCaptureDevicePosition.Front;

            var device = GetCameraForOrientation(devicePosition);

            ConfigureCameraForDevice(device);

            captureSession.BeginConfiguration();
            captureSession.RemoveInput(captureDeviceInput);
            captureDeviceInput = AVCaptureDeviceInput.FromDevice(device);
            captureSession.AddInput(captureDeviceInput);
            captureSession.CommitConfiguration();
        }

        public AVCaptureDevice GetCameraForOrientation(AVCaptureDevicePosition orientation)
        {
            var devices = AVCaptureDevice.DevicesWithMediaType(AVMediaType.Video);
            foreach (var device in devices)
            {
                if (device.Position == orientation)
                {
                    return device;
                }
            }

            return null;
        }
    }
}