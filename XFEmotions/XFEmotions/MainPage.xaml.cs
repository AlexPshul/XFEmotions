using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XFEmotions
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
            Recognize();
		}

	    private async void Recognize()
	    {

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