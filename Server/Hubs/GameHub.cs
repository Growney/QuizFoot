using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using QuizFoot.Abstractions;
using QuizFoot.Domain;
using QuizFoot.Server.Abstractions;
using QuizFoot.Shared.Live;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizFoot.Server.Hubs
{
    public class GameHub : Hub
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IQuizFootUnitOfWork _unitOfWork;
        private readonly IGameProjector _gameProjector;

        public GameHub(IQuizFootUnitOfWork unitOfWork, IGameProjector gameProjector, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _gameProjector = gameProjector;
        }
        public async Task<GameLobbyInfoDto?> EnterGameLobby(string code)
        {
            var user = await _userManager.GetUserAsync(Context.User);
            if (user == null)
            {
                return null;
            }
            var game = await _unitOfWork.Games.Get(code);
            if (game == null)
            {
                return null;
            }
            await Groups.AddToGroupAsync(Context.ConnectionId, game.Code);

            var lobbyInfo = _gameProjector.GetGameLobbyInfo(user.AccountId, game);

            return lobbyInfo;
           
        }

        public async Task<Guid?> JoinGame(Guid id, string name)
        {
            var user = await _userManager.GetUserAsync(Context.User);
            if (user == null)
            {
                return null;
            }
            var game = await _unitOfWork.Games.Get(id);
            if (game == null)
            {
                return null;
            }

            var player = game.Players.FirstOrDefault(x => x.AccountId == user.AccountId);

            if (player == null)
            {
                player = new Player(Guid.NewGuid(), user.AccountId, game.Id, name);
                game.AddPlayer(player);
                await _unitOfWork.CommitAsync();
            }
            await Clients.Group(game.Code).SendAsync("PlayerJoined", player.Id, name);
            return player.Id;
        }
        public async Task<RoundDto?> StartGame(Guid id)
        {
            var user = await _userManager.GetUserAsync(Context.User);
            if (user == null)
            {
                return null;
            }
            var game = await _unitOfWork.Games.Get(id);
            if (game == null)
            {
                return null;
            }
            var host = game.GetHost();
            if(host.AccountId != user.AccountId)
            {
                return null;
            }

            game.Start();

            _unitOfWork.Games.Update(game);

        }
    }
}
