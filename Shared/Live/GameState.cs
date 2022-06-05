using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizFoot.Shared.Live
{
    public enum GameState
    {
        Unknown,
        Lobby,
        PreviewingRound,
        PreviewingQuestion,
        AskingQuestion,
        AnsweringQuestion,
        Completed,
        Results,
    }
}