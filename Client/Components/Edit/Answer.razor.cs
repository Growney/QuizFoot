using Microsoft.AspNetCore.Components;
using QuizFoot.Shared.Edit;

namespace QuizFoot.Client.Components.Edit
{
    public partial class Answer : ComponentBase
    {
        private enum AnswerType
        {
            Text,
            Number,
            Date,
            Option
        }
        [Parameter]
        public AnswerDto Root { get; set; }
        [Parameter]
        public QuestionPartDto Parent { get; set; }

        private AnswerType Type { get; set; } = 0;

        protected override void OnParametersSet()
        {
            if(Parent?.Options.Count > 0)
            {
                Type = AnswerType.Option;
            }
        }

        private void TypeChanged()
        {
            Root.StringAnswer = null;
            Root.DateAnswer = null;
            Root.IntegerAnswer = null;
            Root.Option = null;

            switch (Type)
            {
                case AnswerType.Text:
                    Root.StringAnswer = new StringAnswerDto();
                    break;
                case AnswerType.Number:
                    Root.IntegerAnswer = new IntegerAnswerDto();
                    break;
                case AnswerType.Date:
                    Root.DateAnswer = new DateAnswerDto();
                    break;
                case AnswerType.Option:
                    break;
                default:
                    break;
            }
        }
    }
}
