using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Authorization;
using QuizFoot.Shared.Common;

namespace QuizFoot.Client.Pages
{
    [Authorize]
    public partial class GameHost :ComponentBase
    {
        private enum State
        {
            NoCode,
            NoLobby,
            FailedToJoin,
            GettingInfo,
            InLobby,
            InGame,
            HostingLobby,

            RoundDisplay,

            WaitingForNextQuestion,
            AnsweringQuestion,

            PreviewingQuestion,

        }
    }
}
