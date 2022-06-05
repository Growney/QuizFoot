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
            Root.Questions.Add(new QuestionDto()
            {
                Text = "Which one isn't a fruit?",
                Parts = new List<QuestionPartDto>()
                {
                    new QuestionPartDto()
                    {
                        IsMultipleChoice =true,
                        AnswerType = AnswerType.String,
                        Answers = new List<AnswerDto>()
                        {
                            new AnswerDto()
                            {
                                StringAnswer = new StringAnswerDto()
                                {
                                    Text = "Orange",
                                    CaseSensitive = false
                                },
                                DateAnswer = new DateAnswerDto(),
                                NumericalAnswer = new NumericalAnswerDto()
                            },
                            new AnswerDto()
                            {
                                StringAnswer = new StringAnswerDto()
                                {
                                    Text = "Apple",
                                    CaseSensitive = false
                                },
                                DateAnswer = new DateAnswerDto(),
                                NumericalAnswer = new NumericalAnswerDto()
                            },
                            new AnswerDto()
                            {
                                StringAnswer = new StringAnswerDto()
                                {
                                    Text = "Pear",
                                    CaseSensitive = false
                                },
                                DateAnswer = new DateAnswerDto(),
                                NumericalAnswer = new NumericalAnswerDto()
                            },
                            new AnswerDto()
                            {
                                IsCorrect = true,
                                StringAnswer = new StringAnswerDto()
                                {
                                    Text = "Dave",
                                    CaseSensitive = false
                                },
                                DateAnswer = new DateAnswerDto(),
                                NumericalAnswer = new NumericalAnswerDto()
                            }

                        }
                    }
                }
            }); 
        }
    }
}