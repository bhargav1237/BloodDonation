using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminOnly()
        {
            return Ok("Welcome, Admin!");
        }

        [HttpGet("moderator")]
        [Authorize(Roles = "Moderator")]
        public IActionResult ModeratorOnly()
        {
            return Ok("Welcome, Moderator!");
        }

        [HttpGet("user")]
        [Authorize(Roles = "User")]
        public IActionResult UserOnly()
        {
            return Ok("Welcome, User!");
        }
    }

}