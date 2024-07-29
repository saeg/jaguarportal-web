using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Text;
using WebJaguarPortal.Infrastructure;
using WebJaguarPortal.Models;
using WebJaguarPortal.Repository.Interfaces;
using WebJaguarPortal.Services;
using WebJaguarPortal.ViewModels;

namespace WebJaguarPortal.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class AnalyzesController : Controller
    {
        private readonly ILogger<AnalyzesController> _logger;
        private readonly AnalysisService analysisService;
        private readonly IMapper mapper;

        public AnalyzesController(ILogger<AnalyzesController> logger, AnalysisService analysisService, IMapper mapper)
        {
            _logger = logger;
            this.analysisService = analysisService;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = RolesUtil.AnalyzesList)]
        public IActionResult Index()
        {
            List<AnalyzeGridItemViewModel> list = new List<AnalyzeGridItemViewModel>();
            foreach (var item in analysisService.GetAll())
            {
                list.Add(new AnalyzeGridItemViewModel()
                {
                    CreatedAt = item.StartAnalysis,
                    Status = item.Status == Models.StatusAnalysis.InProgress ? "In progess" : item.Status == Models.StatusAnalysis.Completed ? "Completed" : "Error",
                    ProjectName = item.Project.Name,
                    Id = item.Id,
                    TestsFail = item.TestsFail,
                    TestsPass = item.TestsPass,
                    PullRequestBase = item.PullRequestBase,
                    PullRequestBranch = item.PullRequestBranch,
                    PullRequestNumber = item.PullRequestNumber,
                    Provider = item.Provider,
                    Repository = item.Repository,
                    UpdatedAt = item.EndAnalysis
                });
            }

            return View("Index", list);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = RolesUtil.AnalyzesList)]
        public IActionResult Project(long id)
        {
            List<AnalyzeGridItemViewModel> list = new List<AnalyzeGridItemViewModel>();
            foreach (var item in analysisService.GetAll().Where(x => x.ProjectId == id))
            {
                list.Add(new AnalyzeGridItemViewModel()
                {
                    CreatedAt = item.StartAnalysis,
                    Status = item.Status == Models.StatusAnalysis.InProgress ? "In progess" : item.Status == Models.StatusAnalysis.Completed ? "Completed" : "Error",
                    ProjectName = item.Project.Name,
                    Id = item.Id,
                    TestsFail = item.TestsFail,
                    TestsPass = item.TestsPass,
                    PullRequestBase = item.PullRequestBase,
                    PullRequestBranch = item.PullRequestBranch,
                    PullRequestNumber = item.PullRequestNumber,
                    Provider = item.Provider,
                    Repository = item.Repository,
                    UpdatedAt = item.EndAnalysis,

                });
            }

            return View("Index", list);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = RolesUtil.AnalyzesDetail)]
        public IActionResult Detail(long id)
        {
            AnalyzeDetailViewModel detail = new AnalyzeDetailViewModel();

            IEnumerable<ClassAnalysis> classes = analysisService.GetById(id).Classes;

            IEnumerable<string> files = classes.Select(x => x.FullName);
            
            detail.FilesAndPaths = analysisService.Convert(classes, files);

            List<ViewModels.LineAnalysisViewModel> lines = new List<ViewModels.LineAnalysisViewModel>();

            foreach (var item in analysisService.GetById(id).Classes)
            {
                foreach (var lineX in item.Lines)
                {
                    LineAnalysisViewModel line = mapper.Map<ViewModels.LineAnalysisViewModel>(lineX);
                    line.ClassName = item.FullName;
                    lines.Add(line);
                }
            }

            detail.Susp = lines;

            return View(detail);
        }


        [HttpGet]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = RolesUtil.AnalyzesDelete)]
        public IActionResult Delete(long id)
        {
            var item = analysisService.GetById(id);
            if (item == null)
            {
                TempData["AlertFade"] = "Analysis not found";
                return RedirectToAction("Index");
            }

            AnalyzeDeleteViewModel obj = new()
            {
                CreatedAt = item.StartAnalysis,
                ProjectName = item.Project.Name,
                Id = item.Id,
                Repository = item.Repository,
            };
            return View("Delete", obj);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = RolesUtil.AnalyzesDelete)]
        public IActionResult Delete(AnalyzeDeleteViewModel model)
        {
            if (ModelState.IsValid)
            {
                analysisService.Delete(model.Id);

                TempData["AlertFade"] = "Analysis successfully deleted";
                return RedirectToAction("Index");
            }

            return View("Delete", model);
        }

        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles = RolesUtil.AnalyzesDetail)]
        public PartialViewResult Code(long id)
        {
            var @class = analysisService.GetClassById(id);
            var file = @class.Code;

            string path = string.Empty;
            string className = string.Empty;

            int indiceUltimoBarra = @class.FullName.LastIndexOf('/');
            if (indiceUltimoBarra >= 0)
            {
                path = @class.FullName.Substring(0, indiceUltimoBarra + 1);
                className = @class.FullName.Substring(indiceUltimoBarra + 1, @class.FullName.Length - indiceUltimoBarra - 1);
            }

            CodeViewModel code = new CodeViewModel()
            {
                ClassName = className,
                Content = file != null ? Encoding.UTF8.GetString(file.Data) : string.Empty,
                FileName = $"{className}.java",
                Path = path,
                Lines = @class.Lines
            };

            return PartialView("_code", code);
        }
    }
}