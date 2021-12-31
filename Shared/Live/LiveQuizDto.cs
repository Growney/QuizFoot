using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizFoot.Shared.Live
{
    public enum LiveQuizState
    {
        Lobby,

    }
    public class LiveQuizDto
    {
        public Guid Id { get; set; }
        public Guid Quiz { get; set; }
        public PlayerDto Host { get; set; }
        public LiveQuizState State { get; set; }
    }
}
