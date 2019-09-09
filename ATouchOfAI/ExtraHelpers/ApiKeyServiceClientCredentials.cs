using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Rest;

namespace System
{
	public class ApiKeyServiceClientCredentials : ServiceClientCredentials
	{
		private readonly string subscriptionKey;
		public ApiKeyServiceClientCredentials(string subscriptionKey)
		{
			if (string.IsNullOrWhiteSpace(subscriptionKey))
				throw new ArgumentNullException("subscriptionKey");

			this.subscriptionKey = subscriptionKey;
		}
		public override Task ProcessHttpRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			if (request == null)
				throw new ArgumentNullException("request");

			request.Headers.Add("Ocp-Apim-Subscription-Key", this.subscriptionKey);

			return Task.CompletedTask;
		}
	}
}
