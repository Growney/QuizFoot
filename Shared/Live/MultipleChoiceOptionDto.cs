using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizFoot.Shared.Live
{
    public class MultipleChoiceOptionDto
    {
        public Guid AnswerId { get; set; }
        public string StringValue { get; set; }
        public DateTime? DateValue { get; set; }
        public double NumericValue { get; set; }
    }
}
