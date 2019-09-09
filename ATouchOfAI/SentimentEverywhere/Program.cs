using System;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;

namespace SentimentEverywhere
{
	class Program
	{
		public static async Task Main()
		{
			string text = "SQLSaturday is Awesome!";

			string[] endpoints = new string[] {
				"<endpointGoesHere>",
				"http://localhost:5000"
			};
			string key = "<apiKeyGoesHere>";

			ApiKeyServiceClientCredentials credentials = new ApiKeyServiceClientCredentials(key);

			foreach (string endpoint in endpoints)
			{
				using (TextAnalyticsClient client = new TextAnalyticsClient(credentials)
				{
					Endpoint = endpoint
				})
				{
					SentimentResult sentiment = await client.SentimentAsync(text);

					Console.WriteLine($"Sentiment for text: '{text}'\n On endpoint: '{endpoint}'\n is: {sentiment.Score}\n");
				}
			}

			Console.ReadLine();
		}
	}
}
