using System.IO;
using Android.Graphics;
using Android.Media;
using Java.Nio;

namespace XFEmotions.Droid
{
    public class StillImageListener : Java.Lang.Object, ImageReader.IOnImageAvailableListener
    {
        private readonly CameraPreviewView _cameraPreviewView;
        private Image _currentImage;

        public bool IsBusy { get; set; }

        public StillImageListener(CameraPreviewView cameraPreviewView)
        {
            _cameraPreviewView = cameraPreviewView;
        }

        public void OnImageAvailable(ImageReader reader)
        {
            if (_currentImage != null)
                return;

            _currentImage = reader.AcquireLatestImage();
            ByteBuffer byteBuffer = _currentImage.GetPlanes()[0].Buffer;

            byte[] imageBytes = new byte[byteBuffer.Remaining()];
            byteBuffer.Get(imageBytes);

            FixImage(imageBytes);
            _currentImage.Close();
            _currentImage = null;
        }

        private void FixImage(byte[] imageBytes)
        {
            Bitmap decodeByteArray = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);

            Matrix matrix = new Matrix();
            matrix.PostRotate(-90);
            matrix.PostScale(-1/2f, 1/2f);
            Bitmap correctedBitmap = Bitmap.CreateBitmap(decodeByteArray, 0, 0, decodeByteArray.Width, decodeByteArray.Height, matrix, true);

            MemoryStream stream = new MemoryStream(correctedBitmap.ByteCount);
            correctedBitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
            stream.Position = 0;
            _cameraPreviewView.ImageBytes = stream;
            
            decodeByteArray.Dispose();
            correctedBitmap.Dispose();
        }
    }
}