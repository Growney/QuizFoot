using Microsoft.AspNetCore.Components;
using QuizFoot.Shared.Edit;

namespace QuizFoot.Client.Components.Edit
{
    public partial class QuestionPart : ComponentBase
    {
        [Parameter]
        public QuestionPartDto Root { get; set; }
        [Parameter]
        public bool ShowText { get; set; } = false;

        private void OnAnswersCountChange(ChangeEventArgs args)
        {
            if(int.TryParse(args?.Value?.ToString(),out int count))
            {
                if(count < 1)
                {
                    count = 1;
                    args.Value = 1;
                }

                if(count > Root.Answers.Count)
                {
                    var toAdd = count - Root.Answers.Count;
                    for (int i = 0; i < toAdd; i++)
                    {
                        Root.Answers.Add(
                            new AnswerDto()
                            {
                                StringAnswer = new StringAnswerDto(),
                                DateAnswer = new DateAnswerDto(),
                                NumericalAnswer = new NumericalAnswerDto(),
                            });
                    }
                }
                else if(count < Root.Answers.Count)
                {
                    var toRemove = Root.Answers.Count - count;
                    for (int i = 0;i < toRemove; i++)
                    {
                        if(Root.Answers.Count > 0)
                        {
                            Root.Answers.RemoveAt(Root.Answers.Count - 1);
                        }
                    }
                }
            }
        }
    }
}
