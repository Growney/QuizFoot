using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizFoot.Server.Contexts.Models
{
    public class Player
    {
        public Guid AccountId { get; set; }
        public Guid PlayerId { get; set; }
        public string Name { get; set; }
    }
}
