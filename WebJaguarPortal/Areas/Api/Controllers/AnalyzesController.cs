using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Intrinsics.Arm;
using WebJaguarPortal.Areas.Api.Models;
using WebJaguarPortal.Infrastructure;
using WebJaguarPortal.Models;
using WebJaguarPortal.Services;

namespace WebJaguarPortal.Areas.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RolesUtil.AnalyzesNew)]
    [Route("api/[controller]")]
    [ApiController]
    public class AnalyzesController : ControllerBase
    {
        private readonly AnalysisService service;
        private readonly IMapper mapper;

        public AnalyzesController(AnalysisService service, IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(Models.AnalysisControlFlowResponseModel))]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RolesUtil.AnalyzesNew)]
        public IActionResult Initialize(Models.AnalysisControlFlowModel model)
        {
            ControlFlowAnalysis analysis = new ControlFlowAnalysis()
            {
                TestsFail = model.TestsFail,
                TestsPass = model.TestsPass,
                Provider = model.Provider,
                Repository = model.Repository,
                PullRequestBase = model.PullRequestBase ?? string.Empty,
                PullRequestBranch = model.PullRequestBranch ?? string.Empty,
                PullRequestNumber = model.PullRequestNumber ?? string.Empty
            };

            long id = service.New(model.ProjectKey, analysis);
            return Ok(new Models.AnalysisControlFlowResponseModel { Id = id });
        }

        [HttpPost]
        [Route("{id}/Finalize")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RolesUtil.AnalyzesNew)]
        public IActionResult Finalize([FromRoute] long id, [FromBody] FinalizeAnalysisModel model)
        {
            switch (model.Status)
            {
                case FinalizeAnalysisModel.StatusFinalize.Success:
                    service.FinalizeSuccess(id);
                    break;
                case FinalizeAnalysisModel.StatusFinalize.Error:
                    if (model?.MessageError != null)
                    {
                        service.FinalizeError(id, model.MessageError);
                    }
                    else
                    {
                        ModelState.AddModelError(nameof(model.MessageError), $"{nameof(model.MessageError)} is required to finalize with error.");
                        return BadRequest(ModelState);
                    }
                    break;
            }
            return Ok();
        }

        [HttpPost]
        [Route("{id}/Class")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RolesUtil.AnalyzesNew)]
        public IActionResult Post([FromRoute] long id, [FromBody] Models.ClassAnalysisModel model)
        {
            ClassAnalysis classAnalysis = new()
            {
                AnalysisId = id,
                FullName = model.FullName,
                Lines = new List<LineAnalysis>()
            };

            if (model.Code != null && model.Code.Length > 0)
            {
                if (service.VerifyExistsFile(model.Code, out long? idFile) && idFile.HasValue)
                {
                    classAnalysis.FileAnalyzeId = idFile.Value;
                }
                else
                {
                    classAnalysis.Code = new FileAnalysis()
                    {
                        Data = model.Code,
                        Hash = Util.GenerateMD5(model.Code)
                    };
                }
            }

            foreach (var item in model.Lines)
            {
                classAnalysis.Lines.Add(new LineAnalysis()
                {
                    Cef = item.Cef,
                    Cep = item.Cep,
                    Cnf = item.Cnf,
                    Cnp = item.Cnp,
                    NumberLine = item.NumberLine,
                    SuspiciousValue = item.SuspiciousValue,
                    Method = item.Method,
                    ClassAnalysisId = id
                });
            }

            service.AddClass(classAnalysis);
            return Ok();
        }
    }
}
