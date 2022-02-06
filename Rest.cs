using System;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Web;
using System.Linq;
using Firestar4.OpenTDBWrapper.Entities;
namespace Firestar4.OpenTDBWrapper
{
	public static class OpenTDBWrapper
	{
		public static async Task<TriviaQuestion> GetQuestionAsync()
		{
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://opentdb.com/api.php?amount=1&type=multiple");
			request.ContentType = "application/json";
			string data;
			WebResponse response = request.GetResponse();
			using (Stream responseStream = response.GetResponseStream() ?? Stream.Null)
			using (StreamReader responseReader = new StreamReader(responseStream))
			{
				data = await responseReader.ReadToEndAsync();
			}
			var outResponse = JsonConvert.DeserializeObject<TriviaQuestionResponse>(data);
			if (outResponse.response_code == ResponseCode.Success)
			{
				var first = outResponse.results.First();
				first.Question = HttpUtility.HtmlDecode(first.Question);
				first.CorrectAnswer = HttpUtility.HtmlDecode(first.CorrectAnswer);
				first.IncorrectAnswers = first.IncorrectAnswers.Select(xr => HttpUtility.HtmlDecode(xr)).ToList();
				first.Difficulty = first.Difficulty[0].ToString().ToUpper() + first.Difficulty.Substring(1);
				return first;
			}

			throw new Exception($"Error code: {outResponse.response_code} from the server");
			
		}
	}
}
