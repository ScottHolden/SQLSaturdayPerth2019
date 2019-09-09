using System;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;

namespace TextAnalytics
{
	class Program
	{
		public static async Task Main()
		{
			string endpoint = "<endpointGoesHere>";
			string key = "<apiKeyGoesHere>";

			ApiKeyServiceClientCredentials credentials = new ApiKeyServiceClientCredentials(key);

			using (TextAnalyticsClient client = new TextAnalyticsClient(credentials)
			{
				Endpoint = endpoint
			})
			{
				while (true)
				{
					// Read the users input
					Console.Write("\n--------------------\nEnter some text: ");

					string input = Console.ReadLine();

					// Detect the language
					LanguageResult languageResult = await client.DetectLanguageAsync(input);

					Console.WriteLine("\n> Language: " + languageResult.ToDescription());

					string languageCode = languageResult.HighestLanguageCode();

					// Detect the sentiment
					SentimentResult sentimentResult = await client.SentimentAsync(input, languageCode);

					Console.WriteLine($"> Sentiment: {sentimentResult.Score} - {sentimentResult.ToDescription()}");

					// Detect the key phrases
					KeyPhraseResult keyPhraseResult = await client.KeyPhrasesAsync(input, languageCode);

					Console.WriteLine("> Key Phrases: " + string.Join(", ", keyPhraseResult.KeyPhrases));
				}
			}
		}
	}
}
