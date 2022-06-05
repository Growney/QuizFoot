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
using QuizFoot.Client.Lib;

namespace QuizFoot.Client.Pages
{
    [Authorize]
    public partial class Game : ComponentBase
    {

        [Inject]
        public IAccessTokenProvider TokenProvider { get; set; }
        [Inject]
        public HttpClient Client { get; set; }
        [Inject]
        public NavigationManager Nav { get; set; }

        [Parameter]
        public string Code { get; set; }


        private GameState _currentState = GameState.GettingInfo;
        private string _name;
        private GameLobbyInfoDto _lobbyInfo;
        private bool IsHost => _lobbyInfo?.PlayerId == _lobbyInfo?.Host.Id;


        private RoundDto _currentRound;
        private QuestionDto _currentQuestion;

        private HubConnection _hubConnection;

        private HubConnection CreateConnection() => new HubConnectionBuilder()
                .WithUrl(Nav.ToAbsoluteUri("/gameHub"), options =>
                {
                    options.AccessTokenProvider = async () =>
                    {
                        var token = await TokenProvider.RequestAccessToken();
                        if (token.Status == AccessTokenResultStatus.Success)
                        {
                            if (token.TryGetToken(out var access))
                            {
                                return access.Value;
                            }
                        }
                        return string.Empty;
                    };

                })
                .Build();

        private void SetupConnection(HubConnection connection)
        {
            connection.On<Guid, string>("PlayerJoined", OnPlayerJoined);
        }
        protected override async Task OnInitializedAsync()
        {
            if (Code != null)
            {
                _hubConnection = CreateConnection();
                SetupConnection(_hubConnection);
                await _hubConnection.StartAsync();
                _lobbyInfo = await _hubConnection.InvokeAsync<GameLobbyInfoDto>("EnterGameLobby", Code);
                if (_lobbyInfo != null)
                {
                    switch (_lobbyInfo.State)
                    {
                        case GameState.Unknown:
                            break;
                        case GameState.Lobby:
                            break;
                        case GameState.PreviewingRound:
                            break;
                        case GameState.PreviewingQuestion:
                            break;
                        case GameState.AskingQuestion:
                            break;
                        case GameState.AnsweringQuestion:
                            break;
                        case GameState.Completed:
                            break;
                        case GameState.Results:
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    _currentState = State.NoLobby;
                }
            }
            else
            {
                _currentState = State.NoCode;
            }
            

            await base.OnInitializedAsync();
        }

        private async Task JoinGame()
        {
            var playerId = await _hubConnection.InvokeAsync<Guid?>("JoinGame", _lobbyInfo.Id, _name);
            if(playerId != null)
            {
                _lobbyInfo.PlayerId = playerId;
                _currentState = State.InGame;
            }
            else
            {
                _currentState = State.FailedToJoin;
            }
            StateHasChanged();
        }
        private void OnPlayerJoined(Guid playerId, string name)
        {
            _lobbyInfo.Players.Add(new PlayerDto()
            {
                Id = playerId,
                Name = name
            });

            StateHasChanged();
        }
        private async Task StartGame()
        {
            _currentRound = await _hubConnection.InvokeAsync<RoundDto>("StartGame", _lobbyInfo.Id);
            if (_currentRound != null)
            {
                _currentState = State.RoundDisplay;
            }
        }
    }
}
