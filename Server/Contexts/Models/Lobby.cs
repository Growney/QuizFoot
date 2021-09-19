using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizFoot.Server.Contexts.Models
{
    public class Lobby
    {
        public Guid Id { get; set; }
        public string HostId { get; set; }
        public string Code { get; set; }
        public Guid QuizId { get; set; }
    }
}
