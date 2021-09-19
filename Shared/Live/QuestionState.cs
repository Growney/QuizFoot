
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizFoot.Shared.Live
{
    public enum eQuestionState
    {
        Pending,
        Asked,
        Answering,
        Answered
    }
    public class QuestionState
    {
        public Guid Id { get; set; }
        public Guid LobbyId { get; set; }
        public Guid QuestionId { get; set; }
        public eQuestionState State { get; set; }
    }
}
