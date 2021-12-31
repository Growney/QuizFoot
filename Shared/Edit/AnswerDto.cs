using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizFoot.Shared.Edit
{
    public class AnswerDto
    {
        public OptionDto Option { get; set; }
        public StringAnswerDto StringAnswer { get; set; }
        public IntegerAnswerDto IntegerAnswer { get; set; }
        public DateAnswerDto DateAnswer { get; set; }

    }
}
