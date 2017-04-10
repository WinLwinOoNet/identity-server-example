using System.Threading.Tasks;
using IdentityServer.Context;
using IdentityServer.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;

        public AccountController(SignInManager<ApplicationUser> signInManager,
            IIdentityServerInteractionService interaction, 
            IClientStore clientStore)
        {
            _signInManager = signInManager;
            _interaction = interaction;
            _clientStore = clientStore;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            var vm = new LoginViewModel
            {
                ReturnUrl = returnUrl
            };
            return View(vm);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var returnUrl = model.ReturnUrl;

            var result = await _signInManager.PasswordSignInAsync(
                model.Email, model.Password, true, lockoutOnFailure: false);

            return Redirect(returnUrl);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Logout(string logoutId)
        {
            return await Logout(new LogoutViewModel {LogoutId = logoutId});
        }
        
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Logout(LogoutViewModel model)
        {
            await _signInManager.SignOutAsync();

            var logoutContext = await _interaction.GetLogoutContextAsync(model.LogoutId);

            return Redirect(logoutContext?.PostLogoutRedirectUri);
        }
    }
}
