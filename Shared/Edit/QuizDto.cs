using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizFoot.Shared.Edit
{
    public class QuizDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public List<RoundDto> Rounds { get; set; } = new List<RoundDto>();

    }
}
