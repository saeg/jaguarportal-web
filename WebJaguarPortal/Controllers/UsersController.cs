using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebJaguarPortal.Infrastructure;
using WebJaguarPortal.Models;
using WebJaguarPortal.Services;
using WebJaguarPortal.ViewModels;

namespace WebJaguarPortal.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly UserService userService;
        private readonly IMapper mapper;

        public UsersController(ILogger<UsersController> logger, UserService userService, IMapper mapper)
        {
            _logger = logger;
            this.userService = userService;
            this.mapper = mapper;
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = RolesUtil.UsersList)]
        public IActionResult Index()
        {
            IEnumerable<UserGridViewModel> users = mapper.Map<IEnumerable<UserGridViewModel>>(userService.GetAll());

            return View(users);
        }


        [HttpGet]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = RolesUtil.UsersNew)]
        public IActionResult Add()
        {
            return View("Add", new UserAddViewModel());
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = RolesUtil.UsersNew)]
        public IActionResult Add(UserAddViewModel model)
        {
            if (!userService.VerifyUniqueEmail(model.Email))
                ModelState.AddModelError(String.Empty, "This email is already being used by another user");
            if (!userService.VerifyUniqueUsername(model.Username))
                ModelState.AddModelError(String.Empty, "This username is already being used by another user");

            if (ModelState.IsValid)
            {
                User user = mapper.Map<User>(model);
                userService.Add(user);

                
                TempData["AlertFade"] = "User successfully added, an e-mail was sent with a link to complete the first login.";
                return RedirectToAction("Index");
            }

            return View("Add", model);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = RolesUtil.UsersDetail)]
        public IActionResult Detail(long id)
        {
            UserDetailViewModel user = mapper.Map<UserDetailViewModel>(userService.GetById(id));

            return View("Detail", user);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = RolesUtil.UsersEdit)]
        public IActionResult Edit(long id)
        {
            UserEditViewModel user = mapper.Map<UserEditViewModel>(userService.GetById(id));

            return View("Edit", user);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = RolesUtil.UsersEdit)]
        public IActionResult Edit(UserEditViewModel model)
        {
            if (!userService.VerifyUniqueEmail(model.Email, model.Id))
                ModelState.AddModelError(String.Empty, "This email is already being used by another user");

            if (!userService.VerifyUniqueAdmin(model.Id))
            {
                ModelState.AddModelError(String.Empty, "You must have at least one admin user");
            }

            if (ModelState.IsValid)
            {
                User user = mapper.Map<User>(model);
                userService.Update(user);

                TempData["AlertFade"] = "User successfully edited";
                return RedirectToAction("Index");
            }

            return View("Edit", model);
        }


        [HttpGet]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = RolesUtil.UsersDelete)]
        public IActionResult Delete(long id)
        {
            UserDeleteViewModel user = mapper.Map<UserDeleteViewModel>(userService.GetById(id));

            return View("Delete", user);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = RolesUtil.UsersDelete)]
        public IActionResult Delete(UserDeleteViewModel model)
        {
            if (model.Username == this.UsernameAuthenticated())
            {
                ModelState.AddModelError(String.Empty, "Unable to delete own user");
            }

            if (!userService.VerifyDeleteUniqueAdmin(model.Id))
            {
                ModelState.AddModelError(String.Empty, "Unable to delete all admin users");
            }

            if (ModelState.IsValid)
            {
                userService.Delete(model.Id);

                TempData["AlertFade"] = "User successfully deleted";
                return RedirectToAction("Index");
            }

            return View("Delete", model);
        }
    }
}