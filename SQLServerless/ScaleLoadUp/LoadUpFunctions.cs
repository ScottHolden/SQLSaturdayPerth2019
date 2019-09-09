using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace ScaleLoadUp
{
	public class LoadUpFunctions
	{
		private const string Queue1 = "queue1";
		private const string Queue2 = "queue2";
		private const string Queue3 = "queue3";

		[FunctionName(nameof(StartUp))]
		public static Task StartUp(
			[HttpTrigger(AuthorizationLevel.Anonymous)] HttpRequest req,
			[Queue(Queue1)] IAsyncCollector<string> queue1,
			[Queue(Queue2)] IAsyncCollector<string> queue2,
			[Queue(Queue3)] IAsyncCollector<string> queue3) => Enqueue(queue1, queue2, queue3);

		[FunctionName(nameof(QueueFunction1))]
		public static Task QueueFunction1(
			[QueueTrigger(Queue1)] string myQueueItem,
			[Queue(Queue1)] IAsyncCollector<string> queue1,
			[Queue(Queue2)] IAsyncCollector<string> queue2,
			[Queue(Queue3)] IAsyncCollector<string> queue3) => Enqueue(queue1, queue2, queue3);

		[FunctionName(nameof(QueueFunction2))]
		public static Task QueueFunction2(
			[QueueTrigger(Queue2)] string myQueueItem,
			[Queue(Queue1)] IAsyncCollector<string> queue1,
			[Queue(Queue2)] IAsyncCollector<string> queue2,
			[Queue(Queue3)] IAsyncCollector<string> queue3) => Enqueue(queue1, queue2, queue3);

		[FunctionName(nameof(QueueFunction3))]
		public static Task QueueFunction3(
			[QueueTrigger(Queue3)] string myQueueItem,
			[Queue(Queue1)] IAsyncCollector<string> queue1,
			[Queue(Queue2)] IAsyncCollector<string> queue2,
			[Queue(Queue3)] IAsyncCollector<string> queue3) => Enqueue(queue1, queue2, queue3);

		private static async Task Enqueue(IAsyncCollector<string> queue1,
											IAsyncCollector<string> queue2,
											IAsyncCollector<string> queue3)
		{
			await Task.WhenAll(
				queue1.AddAsync(Guid.NewGuid().ToString()),
				queue2.AddAsync(Guid.NewGuid().ToString()),
				queue3.AddAsync(Guid.NewGuid().ToString()),
				// Hard work to simulate heavy load
				Task.Delay(250)
			);
		}
    }
}
