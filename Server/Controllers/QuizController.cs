using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuizFoot.Server.Abstractions;
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
        private readonly UserManager<Contexts.Models.ApplicationUser> _userManager;
        private readonly IQuizRepository _repo;
        public QuizController(IQuizRepository repo, UserManager<Contexts.Models.ApplicationUser> userManager)
        {
            _repo = repo;
            _userManager = userManager;
        }

        [HttpGet("{code}")]
        public async Task<ActionResult> Get(string code)
        {
            var quiz = await _repo.Get(code);
            return new JsonResult(quiz);
        }
        [Authorize]
        [HttpPost("[action]")]
        public async Task<ActionResult> Create(Shared.Edit.Quiz quiz)
        {
            var user = _userManager.GetUserId(User);

            var owner = await _repo.GetOwner(quiz.Id);
            if(owner != null && owner != user)
            {
                return Unauthorized();
            }

            var newCode = await _repo.SaveOrUpdate(quiz, user);
            return Created($"editQuiz/{newCode}", new { code = newCode });
        }

        [Authorize]
        [HttpGet("[action]")]
        public async Task<IActionResult> UserQuizDetails()
        {
            var user = _userManager.GetUserId(User);

            var quizzes = await _repo.GetUserQuizDetails(user);

            return new JsonResult(quizzes);
        }
    }
}
