using QuizFoot.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizFoot.Shared.Edit
{
    
    public class QuestionPartDto
    {
        public Guid Id { get; set; }
        public Guid QuestionId { get; set; }
        public string? Text { get; set; }
        public bool IsMultipleChoice { get; set; }
        public AnswerType AnswerType { get; set; }
        public List<AnswerDto> Answers { get; set; } = new List<AnswerDto>();
    }
}
