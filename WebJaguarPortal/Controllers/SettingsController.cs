using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using WebJaguarPortal.Infrastructure;
using WebJaguarPortal.Models;
using WebJaguarPortal.Services;
using WebJaguarPortal.ViewModels;

namespace WebJaguarPortal.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class SettingsController : Controller
    {
        private readonly ILogger<SettingsController> _logger;
        private readonly SettingsService settingsService;
        private readonly IMapper mapper;

        public SettingsController(ILogger<SettingsController> logger, SettingsService settingsService, IMapper mapper)
        {
            _logger = logger;
            this.settingsService = settingsService;
            this.mapper = mapper;
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = RolesUtil.Admin)]
        public IActionResult Index()
        {
            SettingsViewModel settings = mapper.Map<SettingsViewModel>(settingsService.Get());
            return View(settings);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = RolesUtil.Admin)]
        public IActionResult Index(SettingsViewModel model)
        {
            if (ModelState.IsValid)
            {
                Settings settings = mapper.Map<Settings>(model);
                settingsService.Update(settings);
                TempData["AlertFade"] = "Settings successfully updated";
                
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
    }
}