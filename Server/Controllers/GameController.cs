using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuizFoot.Abstractions;
using QuizFoot.Domain;
using QuizFoot.Server.Abstractions;
using QuizFoot.Shared.Live;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizFoot.Server.Controllers
{

    [Route("api/[controller]")]
    public class GameController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IQuizFootUnitOfWork _unitOfWork;
        private readonly ICodeGenerator _generator;
        private readonly IQuizProjector _quizProjector;
        public GameController(IQuizFootUnitOfWork unitOfWOrk,IQuizProjector quizProjector, ICodeGenerator generator, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWOrk;
            _userManager = userManager;
            _generator = generator;
            _quizProjector = quizProjector;
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromQuery] string quizCode)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var quiz = await _unitOfWork.Quizzes.Get(quizCode);
            if(quiz == null)
            {
                return NotFound();
            }
            var gameId = Guid.NewGuid();
            var hostPlayer = new Player(Guid.NewGuid(), user.AccountId, gameId, user.Email);
            var game = new Game(Guid.NewGuid(), hostPlayer.Id, _generator.Generate(4), quiz.Id);
            game.AddPlayer(hostPlayer);
            foreach(var round in quiz.Rounds)
            {
                var roundStatus = new RoundStatus(Guid.NewGuid(),game.Id, round.Id);
                foreach(var question in round.Questions)
                {
                    var questionStatus = new QuestionStatus(Guid.NewGuid(),roundStatus.Id,question.Id);
                    roundStatus.AddQuestionStatus(questionStatus);
                }
                game.AddRoundStatus(roundStatus);
            }

            _unitOfWork.Games.Add(game);
            await _unitOfWork.CommitAsync();

            return Created($"game/{game.Code}", new { code = game.Code });
        }
    }
}
