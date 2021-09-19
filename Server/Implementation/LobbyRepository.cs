using Microsoft.EntityFrameworkCore;
using QuizFoot.Server.Abstractions;
using QuizFoot.Server.Contexts;
using QuizFoot.Server.Contexts.Models;
using QuizFoot.Shared.Live;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizFoot.Server.Implementation
{
    public class LobbyRepository : ILobbyRepository
    {
        private readonly QuizDbContext _context;
        private readonly ICodeGenerator _codeGenerator;
        private readonly IQuizRepository _quizRepo;
        public LobbyRepository(QuizDbContext context, ICodeGenerator generator,IQuizRepository quizRepo)
        {
            _context = context;
            _codeGenerator = generator;
            _quizRepo = quizRepo;
        }
        public async Task<string> CreateLobby(string quizCode, string hostId)
        {
            var quiz = await _quizRepo.GetDetails(quizCode);
            if (quiz == null)
            {
                throw new KeyNotFoundException();
            }

            var lobby = new Lobby()
            {
                Id = Guid.NewGuid(),
                Code = await _codeGenerator.Generate(7, x => _context.Lobbies.AnyAsync(y => y.Code == x)),
                HostId = hostId,
                QuizId = quiz.Id
            };

            await _context.AddAsync(lobby);

            await _context.SaveChangesAsync();

            return lobby.Code;
        }

        public async Task<LobbyInfo> GetLobby(string lobbyCode)
        {
            try
            {
                var lobby = await _context.Lobbies.SingleOrDefaultAsync(x => x.Code == lobbyCode);
                var quiz = await _quizRepo.GetDetails(lobby.QuizId);
                var host = await _context.Users.SingleOrDefaultAsync(x => x.Id == lobby.HostId);

                return new LobbyInfo()
                {
                    Code = lobbyCode,
                    HostName = host.UserName,
                    Id = lobby.Id,
                    Quiz = quiz
                };
            }
            catch
            {
                return null;
            }
        }
    }
}
