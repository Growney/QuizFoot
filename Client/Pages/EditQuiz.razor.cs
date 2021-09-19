using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using QuizFoot.Shared;
using System.Net.Http.Json;
using QuizFoot.Shared.Edit;
using Microsoft.AspNetCore.Authorization;

namespace QuizFoot.Client.Pages
{
    [Authorize]
    public partial class EditQuiz : ComponentBase
    {
        [Inject]
        public HttpClient Client { get; set; }
        [Inject]
        public NavigationManager Nav { get; set; }

        public Quiz Quiz { get; set; }

        [Parameter]
        public string Code { get;set; }

        protected async override Task OnInitializedAsync()
        {
            if (Code == null)
            {
                Quiz = new Quiz() { Name = "New Quiz" };
            }
            else
            {
                Quiz = await Client.GetFromJsonAsync<Quiz>($"/api/quiz/{Code}");
            }
        }

        public void NewRound_OnClick()
        {
            int position = Quiz.Rounds.Count;

            var newRound = new Round() { Name = $"Round {position}" };
            Quiz.Rounds.Add(newRound);
        }
        public async Task Save_OnClick()
        {
            var response = await Client.PostAsJsonAsync("/api/quiz/create", Quiz);
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                Nav.NavigateTo(response.Headers.Location.ToString());
                Quiz = await Client.GetFromJsonAsync<Quiz>($"/api/quiz/{Code}");
            }
        }
    }
}
