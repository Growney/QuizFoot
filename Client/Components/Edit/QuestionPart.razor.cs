using Microsoft.AspNetCore.Components;
using QuizFoot.Shared.Edit;

namespace QuizFoot.Client.Components.Edit
{
    public partial class QuestionPart : ComponentBase
    {
        [Parameter]
        public QuestionPartDto Root { get; set; }

        private void NewOption()
        {
            Root.Options.Add(new OptionDto());
        }
        private void NewAnswer()
        {
            Root.Answers.Add(new AnswerDto());
        }
    }
}
