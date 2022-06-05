using Microsoft.AspNetCore.Components;
using QuizFoot.Shared.Edit;

namespace QuizFoot.Client.Components.Edit
{
    public partial class Answer : ComponentBase
    {
        [Parameter]
        public AnswerType AnswerType { get; set; }
        [Parameter]
        public AnswerDto Root { get; set; }
        [Parameter]
        public bool ShowNonExactParameters { get; set; }
        [Parameter]
        public bool ShowCorrect { get; set; }

        protected override void OnParametersSet()
        {
        }

        private void TypeChanged()
        {
        }
    }
}
