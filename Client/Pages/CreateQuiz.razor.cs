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
    public partial class CreateQuiz : ComponentBase
    {
        [Inject]
        public HttpClient Client { get; set; }
        [Inject]
        public NavigationManager Nav { get; set; }
        public QuizDto Quiz { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            Quiz = new QuizDto();
        }
        public async Task Save_OnClick()
        {
            var response = await Client.PostAsJsonAsync("/api/quiz", Quiz);
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                Nav.NavigateTo(response.Headers.Location.ToString());
            }
        }
    }
}
