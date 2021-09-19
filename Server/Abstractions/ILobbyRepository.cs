using QuizFoot.Shared.Live;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizFoot.Server.Abstractions
{
    public interface ILobbyRepository
    {
        Task<string> CreateLobby(string quizCode,string hostId);
        Task<LobbyInfo> GetLobby(string lobbyCode);
    }
}
