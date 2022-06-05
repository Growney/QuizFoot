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
    public partial class HostQuiz :ComponentBase
    {
        [Inject]
        public HttpClient Client { get; set; }
        [Inject]
        public NavigationManager Nav { get; set; }

        [Parameter]
        public string Code { get; set; }

        private List<QuizDetailsDto> _quizzes;

        protected override async Task OnInitializedAsync()
        {
            if(Code != null)
            {
                await Host(Code);
            }
            else
            {
                _quizzes = await Client.GetFromJsonAsync<List<QuizDetailsDto>>($"/api/quiz/all");
            }
        }

        private async void Host_OnClick(string quizCode)
        {
            await Host(quizCode);
        }

        private async Task Host(string code)
        {
            var response = await Client.PostAsJsonAsync($"/api/game?quizCode={code}", string.Empty);
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                Nav.NavigateTo(response.Headers.Location.ToString());
            }
        }
    }
}
