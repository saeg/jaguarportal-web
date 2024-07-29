using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using WebJaguarPortal.Models;
using WebJaguarPortal.Services;
using WebJaguarPortal.ViewModels;

namespace WebJaguarPortal.Areas.Api.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserService userService;

        public LoginController(UserService userService)
        {
            this.userService = userService;
        }
        public IActionResult Index()
        {
            ViewData["HiddenMenuPanel"] = true;
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = userService.ValidateUserAndPassaword(model.Username, model.Password);

                if (user != null)
                {
                    IEnumerable<Claim> claims = userService.GenerateClaims(user);

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        //AllowRefresh = <bool>,
                        // Refreshing the authentication session should be allowed.

                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                        // The time at which the authentication ticket expires. A 
                        // value set here overrides the ExpireTimeSpan option of 
                        // CookieAuthenticationOptions set with AddCookie.

                        IsPersistent = true,
                        // Whether the authentication session is persisted across 
                        // multiple requests. When used with cookies, controls
                        // whether the cookie's lifetime is absolute (matching the
                        // lifetime of the authentication ticket) or session-based.

                        //IssuedUtc = <DateTimeOffset>,
                        // The time at which the authentication ticket was issued.

                        //RedirectUri = <string>
                        // The full path or absolute URI to be used as an http 
                        // redirect response value.
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                    if (Request.Query.TryGetValue("ReturnUrl", out StringValues returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("Password", "Invalid Login or password.");
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Login");
        }


        [HttpGet]
        public IActionResult NewPassword(string id)
        {
            if (!string.IsNullOrWhiteSpace(id) && userService.ValidateForgotPasswordRequest(id, out _))
            {
                return View("NewPassword", new NewPasswordViewModel() { Key = id });
            }
            else
            {
                return View("NewPassword", new NewPasswordViewModel() { Message = "This link has expired or is invalid." });
            }
        }

        [HttpPost]
        public IActionResult NewPassword(NewPasswordViewModel model)
        {
            RenewPassword? request = null;

            if (model.Password != model.ConfirmPassword)
                ModelState.AddModelError(nameof(model.Password), "Password and confirm password must be the same");
            else if (!userService.ValidateEntropyPassword(model.Password))
                ModelState.AddModelError(nameof(model.Password), "Enter a strong password, using uppercase and lowercase letters, numbers and special characters");
            else if (string.IsNullOrWhiteSpace(model.Key) || !userService.ValidateForgotPasswordRequest(model.Key, out request))
                ModelState.AddModelError(string.Empty, "This link has expired or is invalid.");

            if (ModelState.IsValid)
            {
                if (request != null)
                {
                    userService.ChangePassword(request.Username, model.Password);
                    userService.DeleteForgotPasswordByKey(model.Key);
                }

                var resposta = new { status = "success", message = "Successfully password saved" };
                return Json(resposta);
            }
            else
            {
                var respostaErro = new { status = "Error", message = ModelState.GetErrors() };

                return Json(respostaErro);
            }
        }

        [HttpGet]
        public IActionResult FirstLogin()
        {
            return View(new FirstLoginPasswordViewModel());
        }

        [HttpPost]
        public IActionResult FirstLogin(FirstLoginPasswordViewModel model)
        {
            var user = userService.GetByUsernameOrEmail(model.UsernameOrEmail);

            if (user == null)
            {
                ModelState.AddModelError("email", "User not found");
            }

            if (!string.IsNullOrWhiteSpace(user?.Password))
            {
                ModelState.AddModelError("email", "This is not your first login, click on button \"Back\" and click on link \"Forgot password?\" to renew password.");
            }

            if (string.IsNullOrWhiteSpace(user?.Email))
            {
                ModelState.AddModelError("", "This user has no registered email.");
            }

            if (ModelState.IsValid)
            {
                userService.SendFirstLogin(user);                

                var resposta = new { status = "success", message = "Password new request successfully completed. Access your e-mail and click on the link to register a password" };
                return Json(resposta);
            }
            else
            {
                var respostaErro = new { status = "Error", message = ModelState.GetErrors() };

                return Json(respostaErro);
            }
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View(new ForgotPasswordViewModel());
        }

        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            var user = userService.GetByUsernameOrEmail(model.UsernameOrEmail);

            if (user == null)
            {
                ModelState.AddModelError("", "User not found");
            }else if (string.IsNullOrWhiteSpace(user?.Password))
            {
                ModelState.AddModelError("", "This is your first login, click on button \"Back\" and click on link \"First login?\" to first login.");
            }else if (string.IsNullOrWhiteSpace(user?.Email))
            {
                ModelState.AddModelError("", "This user has no registered email.");
            }

            if (ModelState.IsValid)
            {
                userService.SendForgotPassword(user);

                var resposta = new { status = "success", message = "Password renewal request successfully completed. Access your e-mail and click on the link to register a new password" };
                return Json(resposta);
            }
            else
            {
                var respostaErro = new { status = "Error", message = ModelState.GetErrors() };

                return Json(respostaErro);
            }
        }
    }
}
