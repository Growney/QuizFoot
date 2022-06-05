using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuizFoot.Abstractions;
using QuizFoot.Domain;
using QuizFoot.Server.Abstractions;
using QuizFoot.Shared.Common;
using QuizFoot.Shared.Edit;
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
        public QuizController(IQuizFootUnitOfWork unitOfWOrk, IQuizProjector projector, ICodeGenerator generator, UserManager<ApplicationUser> userManager)
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
            if (quiz == null)
            {
                return NotFound();
            }
            var dto = _projector.GetQuizDto(quiz);
            return dto;
        }
        [Authorize]
        [HttpPut]
        public async Task<ActionResult> Edit(QuizDto quizDto)
        {
            var quiz = await _unitOfWork.Quizzes.Get(quizDto.Id);
            if (quiz == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);
            if (user == null || user.AccountId != quiz.CreatorAccountId)
            {
                return Forbid();
            }

            _projector.UpdateQuiz(quiz, quizDto);
            quiz = _unitOfWork.Quizzes.Update(quiz);

            await _unitOfWork.CommitAsync();

            return Ok();
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Create(QuizDto quizDto)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Forbid();
            }

            var quiz = new Quiz(Guid.NewGuid(), user.AccountId, quizDto.Name, _generator.Generate(10));
            _projector.UpdateQuiz(quiz, quizDto);
            _unitOfWork.Quizzes.Add(quiz);

            await _unitOfWork.CommitAsync();

            return Created($"quiz/edit/{quiz.Code}", new { code = quiz.Code });
        }

        [Authorize]
        [HttpGet("[action]")]
        public async Task<ActionResult<List<QuizDetailsDto>>> All()
        {
            var quizDetails = new List<QuizDetailsDto>();
            var user = await _userManager.GetUserAsync(User);
            await foreach (var details in _unitOfWork.Quizzes.GetForCreator(user.AccountId))
            {
                var detailsDto = _projector.GetDetails(details);
                quizDetails.Add(detailsDto);
            }

            return quizDetails;
        }
    }
}
