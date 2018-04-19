using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Common.Contract;
using Microsoft.ProjectOxford.Emotion;
using Xamarin.Forms;

namespace XFEmotions
{
	public partial class MainPage : ContentPage
	{
        private readonly EmotionServiceClient _client = new EmotionServiceClient("Your Key Goes Here!");

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
	            Emotion[] emotions = await _client.RecognizeAsync(CameraPreview.ImageBytes);
	            if (emotions.Length == 0)
	                continue;

	            string currentEmotion = emotions[0].Scores.ToRankedList().FirstOrDefault().Key;
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