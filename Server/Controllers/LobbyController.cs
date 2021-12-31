using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuizFoot.Domain;
using QuizFoot.Server.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizFoot.Server.Controllers
{

    [Route("api/[controller]")]
    public class LobbyController : Controller
    {
        //private readonly UserManager<ApplicationUser> _userManager;
        //private readonly ILobbyRepository _repo;

        //public LobbyController(ILobbyRepository repo, UserManager<ApplicationUser> userManager)
        //{
        //    _repo = repo;
        //    _userManager = userManager;
        //}
        //[Authorize]
        //[HttpPost("[action]")]
        //public async Task<IActionResult> Create([FromQuery]string quizCode)
        //{
        //    var user = _userManager.GetUserId(User);
        //    if(user == null)
        //    {
        //        return Unauthorized();
        //    }
        //    var lobbyCode = await _repo.CreateLobby(quizCode, user);
        //    return Created($"lobby/{lobbyCode}",new { code = lobbyCode });
        //}

        //[HttpGet("[action]/{code}")]
        //public async Task<IActionResult> LobbyInfo(string code)
        //{
        //    var lobby = await _repo.GetLobby(code);

        //    if(lobby == null)
        //    {
        //        return NotFound();
        //    }

        //    return new JsonResult(lobby);
        //}

        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
