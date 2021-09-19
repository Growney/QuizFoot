using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizFoot.Shared.Edit
{
    public class Quiz
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public List<Round> Rounds { get; set; } = new List<Round>();

    }
}
