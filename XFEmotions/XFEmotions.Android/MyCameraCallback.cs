using System.Collections.Generic;
using Android.Graphics;
using Android.Hardware.Camera2;
using Android.Media;
using Android.OS;
using Android.Views;

namespace XFEmotions.Droid
{
    public class MyCameraCallback : CameraDevice.StateCallback
    {
        private readonly Surface _surface;
        private readonly ImageReader _reader;
        private readonly CameraPreviewView _cameraPreviewView;

        public MyCameraCallback(Surface surface, CameraPreviewView cameraPreviewView)
        {
            _surface = surface;
            _cameraPreviewView = cameraPreviewView;

            _reader = ImageReader.NewInstance(800, 450, ImageFormatType.Jpeg, 1);
            var stillImageListener = new StillImageListener(cameraPreviewView);
            HandlerThread handlerThread = new HandlerThread("CameraWorker");
            handlerThread.Start();
            _reader.SetOnImageAvailableListener(stillImageListener, new Handler(handlerThread.Looper));
        }

        public override void OnDisconnected(CameraDevice camera)
        {

        }

        public override void OnError(CameraDevice camera, CameraError error)
        {
        }

        public override void OnOpened(CameraDevice camera)
        {
            CaptureRequest.Builder builder = camera.CreateCaptureRequest(CameraTemplate.Preview);
            builder.AddTarget(_surface);
            builder.AddTarget(_reader.Surface);
            CaptureRequest captureRequest = builder.Build();

            camera.CreateCaptureSession(new List<Surface> { _surface, _reader.Surface }, new MySessionCallback(captureRequest, _cameraPreviewView, _reader), new Handler(msg => { }));
        }
    }
}