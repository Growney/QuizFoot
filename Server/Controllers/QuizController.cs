using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuizFoot.Abstractions;
using QuizFoot.Domain;
using QuizFoot.Server.Abstractions;
using QuizFoot.Shared.Edit;
using QuizFoot.Shared.Live;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizFoot.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IQuizFootUnitOfWork _unitOfWork;
        private readonly IQuizProjector _projector;
        private readonly ICodeGenerator _generator;
        public QuizController(IQuizFootUnitOfWork unitOfWOrk,IQuizProjector projector,ICodeGenerator generator, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWOrk;
            _userManager = userManager;
            _projector = projector;
            _generator = generator;
        }

        [HttpGet("{code}")]
        public async Task<ActionResult<QuizDto>> Get(string code)
        {
            var quiz = await _unitOfWork.Quizzes.Get(code);
            var dto = _projector.GetQuizDto(quiz);
            return dto;
        }
        [Authorize]
        [HttpPost("[action]")]
        public async Task<ActionResult> Create(QuizDto quizDto)
        {
            var user = await _userManager.GetUserAsync(User);
            var quiz = new Quiz(Guid.NewGuid(), user.AccountId, quizDto.Name, _generator.Generate(10));
            foreach(var roundDto in quizDto.Rounds)
            {
                var round = new Round(roundDto.Name)
                {
                    Description = roundDto.Description,
                };
                foreach(var questionDto in roundDto.Questions)
                {
                    var question = new Question(questionDto.Text);
                    foreach(var questionPartDto in questionDto.Parts)
                    {
                        var part = new QuestionPart(questionPartDto.Text);
                        foreach(var optionDto in questionPartDto.Options)
                        {
                            if(optionDto.StringAnswer != null)
                            {
                                var answer = new StringAnswer(optionDto.StringAnswer.Text, optionDto.StringAnswer.CaseSensitive);
                                part.AddOption(new Option(answer));
                            }
                            else if(optionDto.IntegerAnswer != null)
                            {
                                var answer = new IntegerAnswer(optionDto.IntegerAnswer.Value, optionDto.IntegerAnswer.DecimalPlaces, optionDto.IntegerAnswer.MaximumValue, optionDto.IntegerAnswer.MinimumValue);
                                part.AddOption(new Option(answer));
                            }
                            else if(optionDto.DateAnswer != null)
                            {
                                var answer = new DateAnswer(optionDto.DateAnswer.Value)
                                {
                                    MatchOnDay = optionDto.DateAnswer.MatchOnDay,
                                    MatchOnMonth = optionDto.DateAnswer.MatchOnMonth,
                                    MatchOnYear = optionDto.DateAnswer.MatchOnYear,
                                };
                                part.AddOption(new Option(answer));
                            }
                        }
                        foreach(var answerDto in questionPartDto.Answers)
                        {
                            if (answerDto.StringAnswer != null)
                            {
                                var answer = new StringAnswer(answerDto.StringAnswer.Text, answerDto.StringAnswer.CaseSensitive);
                                part.AddAnswer(new Answer(answer));
                            }
                            else if (answerDto.IntegerAnswer != null)
                            {
                                var answer = new IntegerAnswer(answerDto.IntegerAnswer.Value, answerDto.IntegerAnswer.DecimalPlaces, answerDto.IntegerAnswer.MaximumValue, answerDto.IntegerAnswer.MinimumValue);
                                part.AddAnswer(new Answer(answer));
                            }
                            else if (answerDto.DateAnswer != null)
                            {
                                var answer = new DateAnswer(answerDto.DateAnswer.Value)
                                {
                                    MatchOnDay = answerDto.DateAnswer.MatchOnDay,
                                    MatchOnMonth = answerDto.DateAnswer.MatchOnMonth,
                                    MatchOnYear = answerDto.DateAnswer.MatchOnYear,
                                };
                                part.AddAnswer(new Answer(answer));
                            }
                        }
                        question.AddQuestionPart(part);
                    }
                    round.AddQuestion(question);
                }
                quiz.AddRound(round);
            }

            _unitOfWork.Quizzes.Add(quiz);
            await _unitOfWork.CommitAsync();
            return Created($"editQuiz/{quiz.Code}", new { code = quiz.Code });
        }

        [Authorize]
        [HttpGet("[action]")]
        public async Task<ActionResult<List<QuizDetailsDto>>> UserQuizDetails()
        {
            var quizDetails = new List<QuizDetailsDto>();
            var user = await _userManager.GetUserAsync(User);
            await foreach(var details in _unitOfWork.Quizzes.GetForCreator(user.AccountId))
            {
                var detailsDto = _projector.GetDetails(details);
                quizDetails.Add(detailsDto);
            }

            return quizDetails;
        }
    }
}
