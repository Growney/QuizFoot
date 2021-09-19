using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizFoot.Server.Contexts.Models
{
    public class QuestionState
    {
        public Guid Id { get; set; }
        public Guid LobbyId { get; set; }
        public Guid QuestionId { get; set; }
        public int State { get; set; }
    }
}
