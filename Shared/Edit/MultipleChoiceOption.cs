using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizFoot.Shared.Edit
{
    public class MultipleChoiceOption
    {
        public Guid Id { get; set; }
        public string Answer { get; set; }
        public bool IsCorrect { get; set; }
    }
}
