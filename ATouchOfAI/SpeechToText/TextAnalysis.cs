using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;

namespace SpeechToText
{
	public class TextAnalysis : IDisposable
	{
		private const string Endpoint = "<endpointGoesHere>";
		private const string Key = "<apiKeyGoesHere>";

		private readonly TextAnalyticsClient textAnalyticsClient;
		private readonly Dictionary<string, int> keywordCounts;

		public TextAnalysis()
		{
			ApiKeyServiceClientCredentials credentials = new ApiKeyServiceClientCredentials(Key);
			textAnalyticsClient = new TextAnalyticsClient(credentials)
			{
				Endpoint = Endpoint
			};

			keywordCounts = new Dictionary<string, int>();
		}

		public void AnalyseText(string text)
		{
			if (string.IsNullOrWhiteSpace(text))
				return;

			SentimentResult sentiment = textAnalyticsClient.Sentiment(text);
			KeyPhraseResult keywords = textAnalyticsClient.KeyPhrases(text);

			foreach(string keyword in keywords.KeyPhrases.Select(x=>x.ToLower().Trim()))
			{
				if(!keywordCounts.ContainsKey(keyword))
				{
					keywordCounts.Add(keyword, 1);
				}
				else
				{
					keywordCounts[keyword]++;
				}
			}

			string sentimentWord = sentiment.ToDescription();
			string commonKeywords = string.Join(", ", 
											keywordCounts.OrderByDescending(x => x.Value)
															.Take(4)
															.Select(x => $"{x.Key} [{x.Value}]"));

			Console.WriteLine($"Sentiment is {sentimentWord} [{sentiment.Score}]");
			Console.WriteLine($" Common keywords are: {commonKeywords}");
			Console.WriteLine();
		}

		public void Dispose()
		{
			textAnalyticsClient?.Dispose();
		}
	}
}
