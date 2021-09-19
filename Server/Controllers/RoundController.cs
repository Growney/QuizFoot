using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuizFoot.Shared.Edit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuizFoot.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoundController : ControllerBase
    {
        public ActionResult Create(Round round)
        {
            return Ok();
        }
    }
}
