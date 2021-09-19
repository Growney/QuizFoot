using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.SignalR.Client;
using QuizFoot.Shared.Live;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace QuizFoot.Client.Pages
{
    [Authorize]
    public partial class Lobby : ComponentBase
    {
        [Inject]
        public IAccessTokenProvider TokenProvider { get; set; }
        [Inject]
        public HttpClient Client { get; set; }
        [Inject]
        public NavigationManager Nav { get; set; }
        [Parameter]
        public string Code { get; set; }
        public LobbyInfo Info { get; set; }


        private HubConnection _hubConnection;

        protected override async Task OnInitializedAsync()
        {
            if(Code != null)
            {
                Info = await Client.GetFromJsonAsync<LobbyInfo>($"/api/lobby/lobbyinfo/{Code}");
            }
            if(Info != null)
            {
                _hubConnection = new HubConnectionBuilder()
                .WithUrl(Nav.ToAbsoluteUri("/lobbyhub"), options =>
                {
                    options.AccessTokenProvider = async () =>
                    {
                        var token = await TokenProvider.RequestAccessToken();
                        if (token.Status == AccessTokenResultStatus.Success)
                        {
                            if(token.TryGetToken(out var access))
                            {
                                return access.Value;
                            }
                        }
                        return string.Empty;
                    };
                        
                })
                .Build();

                _hubConnection.On<string>("PlayerJoinedLobby",OnPlayerJoined);
                await _hubConnection.StartAsync();
                await _hubConnection.SendAsync("JoinLobby", Code);
            }

            await base.OnInitializedAsync();
        }
        
        private void OnPlayerJoined(string name)
        {

        }

    }
}
