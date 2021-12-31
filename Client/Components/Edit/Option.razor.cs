
using Microsoft.AspNetCore.Components;
using QuizFoot.Shared.Edit;

namespace QuizFoot.Client.Components.Edit
{
    public partial class Option : ComponentBase
    {
        [Parameter]
        public OptionDto Root { get; set; }
    }
}
