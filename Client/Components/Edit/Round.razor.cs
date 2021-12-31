using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuizFoot.Shared.Edit;

namespace QuizFoot.Client.Components.Edit
{
    public partial class Round : ComponentBase
    {

        [Parameter]
        public RoundDto Root { get; set; }

        private void NewQuestion()
        {
            Root.Questions.Add(new QuestionDto());
        }
    }
}