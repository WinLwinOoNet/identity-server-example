using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using IdentityServerWebSite.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServerWebSite.Controllers
{
    [Route("")]
    public class HomeController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("private")]
        [Authorize]
        public async Task<IActionResult> Private()
        {
            var model = new TimeViewModel
            {
                Time = await GetTimeAsync()
            };
            return View(model);
        }

        public async Task<string> GetTimeAsync()
        {
            using (var httpClient = new HttpClient())
            {
                string accessToken = await HttpContext.Authentication.GetTokenAsync("access_token");

                httpClient.BaseAddress = new Uri("http://localhost:5001");
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                return await httpClient.GetStringAsync("api/clock/time");
            }
        }
    }
}
