using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServerWebAPI.Controllers
{
    [Route("api/clock")] // http://localhost:5001/api/clock
    public class ClockController : Controller
    {
        [HttpGet("time")] // http://localhost:5001/api/clock/time
        [Authorize]
        public string Time()
        {
            return DateTime.UtcNow.ToString("hh:mm");
        }
    }
}
