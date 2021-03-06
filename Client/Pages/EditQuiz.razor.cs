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

        public QuizDto Quiz { get; set; }
        private string _message = "Getting Quiz Info...";
        [Parameter]
        public string Code { get;set; }

        protected async override Task OnInitializedAsync()
        {
            try
            {
                Quiz = await Client.GetFromJsonAsync<QuizDto>($"/api/quiz/{Code}");
            }
            catch
            {

            }
            if(Quiz == null)
            {
                _message = "Quiz Not Found";
            }
        }
        public async Task Save_OnClick()
        {
            var response = await Client.PutAsJsonAsync("/api/quiz", Quiz);
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                Nav.NavigateTo(response.Headers.Location.ToString());
                Quiz = await Client.GetFromJsonAsync<QuizDto>($"/api/quiz/{Code}");
            }
        }
    }
}
