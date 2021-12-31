using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QuizFoot.Server.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizFoot.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HostController : Controller
    {
        //private readonly UserManager<ApplicationUser> _userManager;
        //private readonly IQuizRepository _repo;
        //public HostController(IQuizRepository repo, UserManager<Contexts.Models.ApplicationUser> userManager)
        //{
        //    _repo = repo;
        //    _userManager = userManager;
        //}
        //[HttpGet("[action]/{code}")]
        //public async Task<IActionResult> QuizDetails(string code)
        //{
        //    var quiz = await _repo.GetDetails(code);
        //    return new JsonResult(quiz);
        //}
    }
}
