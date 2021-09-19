using QuizFoot.Shared.Edit.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace QuizFoot.Shared
{
    public enum QuestionType
    {
        MultipleChoice,
        String,
        ClosestNumber,
    }
}

namespace QuizFoot.Shared.Edit
{
    [JsonConverter(typeof(QuestionJsonConverter))]
    public class Question
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public string Code { get; set; }
        public QuestionType Type { get; set; }
        public object QuestionObject { get; set; }
    }
}