using System;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Web;
using System.Linq;
using Firestar4.OpenTDBWrapper.Entities;
using System.Net.Http;
using System.Net.Http.Headers;
namespace Firestar4.OpenTDBWrapper
{
	public static class OpenTDBWrapper
	{
		public static async Task<TriviaQuestion> GetQuestionAsync()
		{
			HttpClient client = new();
			HttpRequestMessage request = new(HttpMethod.Get, "https://opentdb.com/api.php?amount=1&type=multiple");
			request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			var response = await client.SendAsync(request);
			string data;
			response.EnsureSuccessStatusCode();
			using (Stream responseStream = response.Content.ReadAsStream() ?? Stream.Null)
			using (StreamReader responseReader = new(responseStream))
			{
				data = await responseReader.ReadToEndAsync();
			}
			var outResponse = JsonConvert.DeserializeObject<TriviaQuestionResponse>(data);
			if (outResponse.Response_code == ResponseCode.Success)
			{
				var first = outResponse.Results.First();
				first.Question = HttpUtility.HtmlDecode(first.Question);
				first.CorrectAnswer = HttpUtility.HtmlDecode(first.CorrectAnswer);
				first.IncorrectAnswers = first.IncorrectAnswers.Select(xr => HttpUtility.HtmlDecode(xr)).ToList();
				first.Category = HttpUtility.HtmlDecode(first.Category);
				first.Difficulty = string.Concat(first.Difficulty[0].ToString().ToUpper(), first.Difficulty.AsSpan(1));
				return first;
			}

			throw new Exception($"Error code: {outResponse.Response_code} from the server");
			
		}
	}
}
