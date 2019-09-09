using System.Linq;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;

namespace System
{
	public static class TextAnalyticsExtensions
	{
		public static string ToDescription(this SentimentResult sentiment)
		{
			if (!sentiment.Score.HasValue) return "Unknown";
			if (sentiment.Score > 0.6) return "Positive";
			if (sentiment.Score < 0.4) return "Negative";
			return "Neutral";
		}
		public static string ToDescription(this LanguageResult item) =>
			string.Join(", ", item.DetectedLanguages
				.OrderByDescending(x => x.Score)
				.Take(3)
				.Select(x => $"{x.Name} [{x.Iso6391Name} - {x.Score * 100}%]"));

		public static string HighestLanguageCode(this LanguageResult item) =>
			item.DetectedLanguages.OrderByDescending(x => x.Score).FirstOrDefault()?.Iso6391Name;
	}
}
