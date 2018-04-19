using Android.Hardware.Camera2;

namespace XFEmotions.Droid
{
    public class MyStillPictureCallback : CameraCaptureSession.CaptureCallback
    {
        public override void OnCaptureCompleted(CameraCaptureSession session, CaptureRequest request, TotalCaptureResult result)
        {
            base.OnCaptureCompleted(session, request, result);

        }
    }
}