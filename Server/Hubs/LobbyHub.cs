using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using QuizFoot.Server.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizFoot.Server.Hubs
{
    public class LobbyHub : Hub
    {
        private readonly UserManager<Contexts.Models.ApplicationUser> _userManager;
        private readonly ILobbyRepository _repo;

        public LobbyHub(ILobbyRepository repo, UserManager<Contexts.Models.ApplicationUser> userManager)
        {
            _repo = repo;
            _userManager = userManager;
        }
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public async Task JoinLobby(string code)
        {
            var userId = _userManager?.GetUserId(Context.User);
            var lobby = await _repo.GetLobby(code);
            await Groups.AddToGroupAsync(Context.ConnectionId, lobby.Code);
            await Clients.OthersInGroup(lobby.Code).SendAsync("PlayerJoinedLobby", Context.User.Identity.Name);
        } 
    }
}
