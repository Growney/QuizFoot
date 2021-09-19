using Microsoft.AspNetCore.Components;
using QuizFoot.Shared.Edit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizFoot.Client.Components.Edit
{
    public partial class EditMultipleChoiceQuestion : ComponentBase
    {
        [Parameter]
        public MultipleChoiceQuestion Root { get; set; }

        public void OnOption_DoubleClick(MultipleChoiceOption option)
        {
            option.IsCorrect = !option.IsCorrect;
        }
    }
}
