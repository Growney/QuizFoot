using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizFoot.Shared.Edit
{
    public class DateAnswerDto
    {
        public DateTime Value { get; set; }
        public bool MatchOnDay { get; set; }
        public bool MatchOnMonth { get; set; }
        public bool MatchOnYear { get; set; }
    }
}
