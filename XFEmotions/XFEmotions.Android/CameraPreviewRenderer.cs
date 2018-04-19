using System;
using System.Linq;
using Android.Content;
using Android.Graphics;
using Android.Hardware.Camera2;
using Android.Hardware.Camera2.Params;
using Android.Media;
using Android.OS;
using Android.Views;
using Java.Lang;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XFEmotions;
using XFEmotions.Droid;
using Size = Android.Util.Size;

[assembly: ExportRenderer(typeof(CameraPreviewView), typeof(CameraPreviewRenderer))]
namespace XFEmotions.Droid
{
    public class CameraPreviewRenderer : ViewRenderer<CameraPreviewView, SurfaceView>, ISurfaceHolderCallback
    {
        private ImageReader _imageReader;

        public CameraPreviewRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<CameraPreviewView> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                SurfaceView surfaceView = new SurfaceView(Context);
                surfaceView.Holder.AddCallback(this);
                SetNativeControl(surfaceView);
            }
        }

        public void SurfaceChanged(ISurfaceHolder holder, Format format, int width, int height)
        {
        }

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            try
            {
                if (!(Context.GetSystemService(Class.FromType(typeof(CameraManager))) is CameraManager manager))
                    return;

                string frontCameraId = manager.GetCameraIdList().FirstOrDefault(id => IsFrontFacingCamera(manager, id));
                CameraCharacteristics frontCamera = manager.GetCameraCharacteristics(frontCameraId);

                if (!(frontCamera?.Get(CameraCharacteristics.ScalerStreamConfigurationMap) is StreamConfigurationMap map))
                    return;

                Size[] outputSizes = map.GetOutputSizes(Class.FromType(holder.GetType()));
                Size firstSmall = outputSizes
                    .Where(size => (double)size.Width / size.Height == 16d / 9)
                    .First(size => size.Height < 900 && size.Width < 900);

                holder.SetFixedSize(firstSmall.Width, firstSmall.Height);
                
                CameraDevice.StateCallback stateCallback = new MyCameraCallback(holder.Surface, Element);
                manager.OpenCamera(frontCameraId, stateCallback, new Handler(msg => { }));
            }
            catch (Java.IO.IOException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
        }

        private bool IsFrontFacingCamera(CameraManager manager, string cameraId)
        {
            int lensFacingDirection = (int)manager.GetCameraCharacteristics(cameraId).Get(CameraCharacteristics.LensFacing);
            return lensFacingDirection == (int)LensFacing.Front;
        }
    }    
}