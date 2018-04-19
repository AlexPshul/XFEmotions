using Android.Hardware.Camera2;
using Android.Media;
using Android.OS;
using Timer = System.Timers.Timer;

namespace XFEmotions.Droid
{
    public class PreviewCallback : CameraCaptureSession.CaptureCallback
    {
        private readonly ImageReader _reader;
        private readonly StillImageListener _stillImageListener;
        private readonly Timer _timer = new Timer(100);
        private CameraCaptureSession _session;
        private bool _isReady;
        private readonly EmptyCaptureCallback _emptyCaptureCallback;

        public PreviewCallback(CameraPreviewView cameraPreviewView, ImageReader reader)
        {
            _reader = reader;
            //_stillImageListener = new StillImageListener(cameraPreviewView);
            //HandlerThread handlerThread = new HandlerThread("CameraWorker");
            //handlerThread.Start();
            //_reader.SetOnImageAvailableListener(_stillImageListener, new Handler(handlerThread.Looper));
            _emptyCaptureCallback = new EmptyCaptureCallback();

            Handler handler = new Handler(Looper.MainLooper);
            //_timer.Elapsed += (_, __) => handler.Post(TakePicture);
        }

        public override void OnCaptureStarted(CameraCaptureSession session, CaptureRequest request, long timestamp, long frameNumber)
        {
            _session = session;
            //_isReady = true;

            if (!_timer.Enabled)
                _timer.Start();
        }

        //public override void OnCaptureCompleted(CameraCaptureSession session, CaptureRequest request, TotalCaptureResult result)
        //{
        //    //if (_isReady)
        //    //    TakePicture();
        //}

        private void TakePicture()
        {
            //_isReady = false;
            if (_stillImageListener.IsBusy)
                return;

            CaptureRequest.Builder captureRequest = _session.Device.CreateCaptureRequest(CameraTemplate.StillCapture);
            captureRequest.Set(CaptureRequest.ControlAfMode, (int)ControlAFMode.ContinuousPicture);
            captureRequest.Set(CaptureRequest.JpegOrientation, 90);

            captureRequest.AddTarget(_reader.Surface);
            CaptureRequest stillImageRequest = captureRequest.Build();

            _session.Capture(stillImageRequest, _emptyCaptureCallback, new Handler(message => { }));
        }

        private class EmptyCaptureCallback : CameraCaptureSession.CaptureCallback
        {
            //public event Action Done;

            //public override void OnCaptureCompleted(CameraCaptureSession session, CaptureRequest request, TotalCaptureResult result)
            //{
            //    base.OnCaptureCompleted(session, request, result);
            //    Done?.Invoke();
            //}
        }
    }

}