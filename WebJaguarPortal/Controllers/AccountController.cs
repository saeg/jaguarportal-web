using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Diagnostics;
using WebJaguarPortal.Models;
using WebJaguarPortal.Repository;
using WebJaguarPortal.Services;
using WebJaguarPortal.ViewModels;

namespace WebJaguarPortal.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly UserService userService;

        public AccountController(ILogger<AccountController> logger, UserService userService)
        {
            _logger = logger;
            this.userService = userService;
        }

        public IActionResult Index()
        {
            User? user = userService.GetByUsername(this.UsernameAuthenticated());

            MyAccountViewModel model = new MyAccountViewModel()
            {
                ClientId = user.ClientId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                Email = user.Email
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Personal(MyAccountPersonalDataViewModel model)
        {
            if (ModelState.IsValid)
            {
                userService.UpdatePersonalData(this.UsernameAuthenticated(), model.LastName, model.FirstName, model.Email);

                var resposta = new { status = "success", message = "Successfully saved" };
                return Json(resposta);
            }
            else
            {
                var respostaErro = new { status = "Error", message = ModelState.GetErrors() };

                return Json(respostaErro);
            }
        }

        [HttpPost]
        public IActionResult Password(MyAccountPasswordViewModel model)
        {
            if (userService.ValidateUserAndPassaword(this.UsernameAuthenticated(), model.CurrentPassword) == null)
                ModelState.AddModelError(nameof(model.Password), "Invalid current password");
            else if (model.Password != model.ConfirmPassword)
                ModelState.AddModelError(nameof(model.Password), "Password and confirm password must be the same");
            else if (!userService.ValidateEntropyPassword(model.Password))
                ModelState.AddModelError(nameof(model.Password), "Enter a strong password, using uppercase and lowercase letters, numbers and special characters");


            if (ModelState.IsValid)
            {
                userService.ChangePassword(this.UsernameAuthenticated(), model.Password);

                var resposta = new { status = "success", message = "Successfully password changed" };
                return Json(resposta);
            }
            else
            {
                var respostaErro = new { status = "Error", message = ModelState.GetErrors() };

                return Json(respostaErro);
            }
        }

        [HttpPost]
        public IActionResult Api()
        {
            string clientSecret = userService.GenerateClientSecret(this.UsernameAuthenticated());

            var resposta = new { status = "success", client_secret = clientSecret, message = "Client Secret generated successfully, make a copy of this one, as you will not be able to view it again" };
            return Json(resposta);
        }

        [HttpGet]
        public IActionResult AccessDenied(string returnUrl)
        {            
            return View("AccessDenied", returnUrl);
        }
    }
}