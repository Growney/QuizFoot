using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using QuizFoot.Shared.Common;
using QuizFoot.Shared.Edit;

namespace QuizFoot.Client.Pages
{
    public partial class Index : ComponentBase
    {
        [Inject]
        public AuthenticationStateProvider AuthStateProvider { get; set; }
        [Inject]
        public HttpClient Client { get; set; }
        private List<QuizDetailsDto> _userQuizes = new List<QuizDetailsDto>();
        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthStateProvider?.GetAuthenticationStateAsync();
            if(authState != null && (authState.User?.Identity?.IsAuthenticated ?? false))
            {
                _userQuizes = await Client.GetFromJsonAsync<List<QuizDetailsDto>>($"/api/quiz/all");
                if(_userQuizes == null)
                {
                    _userQuizes = new List<QuizDetailsDto>();
                }
            }

            await base.OnInitializedAsync();   
        }
    }
}
