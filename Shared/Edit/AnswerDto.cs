using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizFoot.Shared.Edit
{
    public class AnswerDto
    {
        public Guid Id { get; set; }
        public Guid QuestionPartId { get; set; }
        public bool IsCorrect { get; set; }
        public StringAnswerDto StringAnswer { get; set; } = new StringAnswerDto();
        public DateAnswerDto DateAnswer { get; set; } = new DateAnswerDto();
        public NumericalAnswerDto NumericalAnswer { get; set; } = new NumericalAnswerDto();
    }
}
