using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizFoot.Shared.Edit
{
    public class StringQuestion
    {
        public Guid Id { get; set; }
        public bool IsCaseSensitive { get; set; }
        public bool IsOrdered { get; set; }
        public List<StringAnswer> Answers { get; set; }
    }
}
