using Microsoft.AspNetCore.Components;
using QuizFoot.Shared.Edit;

namespace QuizFoot.Client.Components.Edit
{
    public partial class Quiz :ComponentBase
    {
        [Parameter]
        public QuizDto Root { get; set; }

        public void NewRound_OnClick()
        {
            int position = Root.Rounds.Count;

            var newRound = new RoundDto() { Name = $"Round {position}" };
            Root.Rounds.Add(newRound);
        }
    }
}
