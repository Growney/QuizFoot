using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizFoot.Shared.Edit
{
    public class NumericalAnswerDto
    {
        public double Value { get; set; }
        public double? MaximumValue { get; set; }
        public double? MinimumValue { get; set; }
        public byte DecimalPlaces { get; set; }
    }
}
