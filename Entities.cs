using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Firestar4.OpenTDBWrapper.Entities
{
	public class TriviaQuestion
	{
		[JsonProperty]
		public string Category { get; internal set; }

		[JsonProperty]
		public string Type { get; internal set; }

		[JsonProperty]
		public string Difficulty { get; internal set; }

		[JsonProperty]
		public string Question { get; internal set; }

		[JsonProperty("correct_answer")]
		public string CorrectAnswer { get; internal set; }

		[JsonProperty("incorrect_answers")]
		public List<string> IncorrectAnswers { get; internal set; }

		[JsonConstructor]
		TriviaQuestion(string category, string type, string difficulty, string question, string correctAnswer, List<string> incorrectAnswers)
		{
			this.Category = category;
			this.Type = type;
			this.Difficulty = difficulty;
			this.Question = question;
			this.CorrectAnswer = correctAnswer;
			this.IncorrectAnswers = incorrectAnswers;
		}
	}

	internal class TriviaQuestionResponse
	{
		[JsonProperty]
		public ResponseCode Response_code { get; internal set; }

		[JsonProperty]
		public List<TriviaQuestion> Results { get; internal set; }

		[JsonConstructor]
		internal TriviaQuestionResponse(int response_code, List<TriviaQuestion> results)
		{
			this.Response_code = (ResponseCode)response_code;
			this.Results = results;
		}

	}

	enum ResponseCode
	{
		Success = 0,
		NoResults = 1,
		InvalidParameter = 2,
		TokenNotFound = 3,
		TokenEmpty = 4
	}
}
