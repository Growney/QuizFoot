using QuizFoot.Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizFoot.Shared.Live
{
    public class GameLobbyInfoDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public GameState State { get; set; }
        public PlayerDto Host { get; set; }
        public QuizDetailsDto Quiz { get; set; }
        public Guid? PlayerId { get; set; }
        public List<PlayerDto> Players { get; set; } 
        public RoundDto CurrentRound { get; set; }
        public QuestionDto CurrentQuestion { get; set; }
    }
}
