using System;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;

namespace SpeechToText
{
	class Program
	{
		static async Task Main()
		{
			// Build up our speech config
			string key = "<apiKeyGoesHere>";
			string region = "<regionGoesHere, eg: australiaeast>";
			SpeechConfig speechConfig = SpeechConfig.FromSubscription(key, region);
			bool showRealtime = true;
			TextAnalysis ta = new TextAnalysis();

			// Set up our speech recogniser
			using (SpeechRecognizer sr = new SpeechRecognizer(speechConfig))
			{
				// When it has heard something, output it to the screen starting with ~
				sr.Recognizing += (object _, SpeechRecognitionEventArgs e) =>
				{
					if (showRealtime)
						Console.WriteLine($"~ {e.Result.Text}");
				};
				
				// When it has recognized something, output it to the screen starting with !
				sr.Recognized += (object _, SpeechRecognitionEventArgs e) =>
				{
					Console.WriteLine($"! {e.Result.Text}");
					showRealtime = false;
					ta.AnalyseText(e.Result.Text);
					showRealtime = true;
				};

				// Start recognition
				await sr.StartContinuousRecognitionAsync();

				// Keep the application running
				Console.WriteLine("Ready!");
				await Task.Delay(-1);
			}
		}
	}
}
