using Android.Hardware.Camera2;
using Android.Media;
using Android.OS;

namespace XFEmotions.Droid
{
    public class MySessionCallback : CameraCaptureSession.StateCallback
        {
            private readonly CaptureRequest _captureRequest;
            private readonly CameraPreviewView _cameraPreviewView;
            private readonly ImageReader _reader;

            public MySessionCallback(CaptureRequest captureRequest, CameraPreviewView cameraPreviewView, ImageReader reader)
            {
                _captureRequest = captureRequest;
                _cameraPreviewView = cameraPreviewView;
                _reader = reader;
            }

            public override void OnConfigured(CameraCaptureSession session)
            {
                session.SetRepeatingRequest(_captureRequest, new PreviewCallback(_cameraPreviewView, _reader), new Handler(msg => { }));
            }

            public override void OnConfigureFailed(CameraCaptureSession session)
            {
            }
        }
    
}