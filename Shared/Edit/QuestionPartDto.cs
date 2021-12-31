using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizFoot.Shared.Edit
{
    public class QuestionPartDto
    {
        public string? Text { get; set; }
        public List<OptionDto> Options { get; set; } = new List<OptionDto>();
        public List<AnswerDto> Answers { get; set; } = new List<AnswerDto>();
    }
}
