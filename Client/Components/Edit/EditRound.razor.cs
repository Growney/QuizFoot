using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuizFoot.Shared;
using QuizFoot.Shared.Edit;

namespace QuizFoot.Client.Components.Edit
{
    public partial class EditRound : ComponentBase
    {

        [Parameter]
        public Round Root { get; set; }

        private void NewMultipleChoice_OnClick()
        {
            var options = new List<MultipleChoiceOption> {
                new MultipleChoiceOption() { Answer = "Option A", IsCorrect = true},
                new MultipleChoiceOption() { Answer = "Option B" },
                new MultipleChoiceOption() { Answer = "Option C" },
                new MultipleChoiceOption() { Answer = "Option D"},
            };
            var question = new Question()
            {
                Text = "Is this a multiple choice question ?",
                Type = QuestionType.MultipleChoice,
                QuestionObject = new MultipleChoiceQuestion() { Options = options }
            };

            Root.Questions.Add(question);
        }

        private void NewStringQuestion_OnClick()
        {
            var answers = new List<StringAnswer>
            {
                new StringAnswer(){ Answer = "Correct answer 1" },
                new StringAnswer(){ Answer = "Correct answer 2" }
            };
            var question = new Question()
            {
                Text = "Is this a string question ?",
                Type = QuestionType.String,
                QuestionObject = new StringQuestion() {  Answers = answers }
            };

            Root.Questions.Add(question);
        }
    }
}