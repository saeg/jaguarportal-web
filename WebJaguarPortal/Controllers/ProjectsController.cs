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
    public class ProjectsController : Controller
    {
        private readonly ILogger<ProjectsController> _logger;
        private readonly ProjectService projectService;
        private readonly IMapper mapper;

        public ProjectsController(ILogger<ProjectsController> logger, ProjectService projectService, IMapper mapper)
        {
            _logger = logger;
            this.projectService = projectService;
            this.mapper = mapper;
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = RolesUtil.ProjectsList)]
        public IActionResult Index()
        {
            IEnumerable<ProjectGridViewModel> list = mapper.Map<IEnumerable<ProjectGridViewModel>>(projectService.GetAll());

            return View(list);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = RolesUtil.ProjectsNew)]
        public IActionResult Add()
        {
            return View("Add", new ProjectAddViewModel());
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = RolesUtil.ProjectsNew)]
        public IActionResult Add(ProjectAddViewModel model)
        {
            if (ModelState.IsValid)
            {
                Project proj = mapper.Map<Project>(model);
                projectService.Add(proj);

                TempData["AlertFade"] = "Project successfully added";
                return RedirectToAction("Index");
            }

            return View("Add", model);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = RolesUtil.ProjectsDetail)]
        public IActionResult Detail(long id)
        {
            var project = projectService.GetById(id);

            ProjectDetailViewModel proj = mapper.Map<ProjectDetailViewModel>(project);

            return View("Detail", proj);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = RolesUtil.ProjectsEdit)]
        public IActionResult Edit(long id)
        {
            var project = projectService.GetById(id);

            ProjectEditViewModel proj = mapper.Map<ProjectEditViewModel>(project);

            return View("Edit", proj);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = RolesUtil.ProjectsEdit)]
        public IActionResult Edit(ProjectEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Project proj = mapper.Map<Project>(model);
                projectService.Update(proj);

                TempData["AlertFade"] = "Project successfully edited";
                return RedirectToAction("Index");
            }

            return View("Add", model);
        }


        [HttpGet]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = RolesUtil.ProjectsDelete)]
        public IActionResult Delete(long id)
        {
            var project = projectService.GetById(id);

            ProjectDeleteViewModel proj = mapper.Map<ProjectDeleteViewModel>(project);

            return View("Delete", proj);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = RolesUtil.ProjectsDelete)]
        public IActionResult Delete(ProjectDeleteViewModel model)
        {
            if (ModelState.IsValid)
            {
                projectService.Delete(model.Id);

                TempData["AlertFade"] = "Project successfully deleted";
                return RedirectToAction("Index");
            }

            return View("Delete", model);
        }
    }
}