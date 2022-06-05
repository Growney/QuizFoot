using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizFoot.Shared.Live
{
    public class RoundDto
    {
        public Guid Id { get; set; }
        public Guid RoundStatusId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
