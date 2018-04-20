using System.Linq;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using Xamarin.Forms;

namespace XFEmotions
{
	public partial class MainPage : ContentPage
	{
        private readonly FaceServiceClient _client = new FaceServiceClient("Your key goes here!", "Your regional base url goes here!");

		public MainPage()
		{
			InitializeComponent();
            Recognize();
		}

	    private async void Recognize()
	    {
	        while (CameraPreview.ImageBytes == null)
	            await Task.Delay(100);

	        while (true)
	        {
	            Face[] faces = await _client.DetectAsync(CameraPreview.ImageBytes, false, false, new[] { FaceAttributeType.Emotion });
	            if (faces.Length == 0)
	                continue;

	            string currentEmotion = faces[0].FaceAttributes.Emotion.ToRankedList().FirstOrDefault().Key;
	            EmotionLabel.Text = currentEmotion;
	            EmotionFrame.BackgroundColor = GetEmotionColor(currentEmotion);
	        }
	    }

	    private Color GetEmotionColor(string emotion)
	    {
	        switch (emotion.ToLower())
	        {
	            case "neutral": return Color.Transparent;
	            case "happiness": return Color.Yellow;
	            case "contempt": return Color.Green;
	            case "disgust": return Color.Green;
	            case "fear": return Color.White;
	            case "sadness": return Color.Blue;
	            case "anger": return Color.Red;
	            case "surprise": return Color.White;
	            default: return Color.Transparent;
	        }
	    }
    }
}