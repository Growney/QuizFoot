using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using QuizFoot.Shared.Live;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Authorization;

namespace QuizFoot.Client.Pages
{
    [Authorize]
    public partial class HostQuiz :ComponentBase
    {
        [Inject]
        public HttpClient Client { get; set; }
        [Inject]
        public NavigationManager Nav { get; set; }

        private List<QuizGeneralDetails> _quizzes;

        protected override async Task OnInitializedAsync()
        {
            _quizzes = await Client.GetFromJsonAsync<List<QuizGeneralDetails>>($"/api/quiz/userquizdetails");
        }

        private async void Host_OnClick(string quizCode)
        {
            var response = await Client.PostAsJsonAsync($"/api/Lobby/Create?quizCode={quizCode}",string.Empty);
            if(response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                Nav.NavigateTo(response.Headers.Location.ToString());
            }
        }
    }
}
