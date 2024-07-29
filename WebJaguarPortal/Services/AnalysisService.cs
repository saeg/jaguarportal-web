using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Permissions;
using WebJaguarPortal.Models;
using WebJaguarPortal.Repository;
using WebJaguarPortal.Repository.Interfaces;
using WebJaguarPortal.ViewModels;

namespace WebJaguarPortal.Services
{
    public class AnalysisService
    {
        private readonly IRepository<ControlFlowAnalysis> analysisRepo;
        private readonly IRepository<ClassAnalysis> classRepo;
        private readonly ProjectRepository projectRepo;
        private readonly FileRepository fileRepo;

        public AnalysisService(IRepository<Models.ControlFlowAnalysis> analysisRepo, IRepository<Models.ClassAnalysis> classRepo, ProjectRepository projectRepo, FileRepository fileRepo)
        {
            this.analysisRepo = analysisRepo;
            this.classRepo = classRepo;
            this.projectRepo = projectRepo;
            this.fileRepo = fileRepo;
        }

        public long New(string projectKey, ControlFlowAnalysis analysis)
        {
            var proj = projectRepo.GetByKey(projectKey);

            if (proj == null)
                throw new Exception($"ProjectKey '{projectKey}' not found.");

            ControlFlowAnalysis obj = new ControlFlowAnalysis()
            {
                ProjectId = proj.Id,
                Status = StatusAnalysis.InProgress,
                StartAnalysis = DateTime.UtcNow,
                TestsFail = analysis.TestsFail,
                TestsPass = analysis.TestsPass,
                Provider = analysis.Provider,
                Repository = analysis.Repository,
                PullRequestBase = analysis.PullRequestBase,
                PullRequestBranch = analysis.PullRequestBranch,
                PullRequestNumber = analysis.PullRequestNumber
            };

            analysisRepo.Add(obj);
            analysisRepo.SaveChanges();

            return obj.Id;
        }

        public void FinalizeSuccess(long analysisId)
        {
            var analysis = analysisRepo.GetById(analysisId);

            if (analysis == null)
                throw new Exception("Analysis not found");

            if (analysis.Status == StatusAnalysis.InProgress)
                analysis.Status = StatusAnalysis.Completed;
            analysis.EndAnalysis = DateTime.UtcNow;

            analysisRepo.SaveChanges();
        }

        public void FinalizeError(long analysisId, string message)
        {
            var analysis = analysisRepo.GetById(analysisId);

            if (analysis == null)
                throw new Exception("Analysis not found");

            if (analysis.Status == StatusAnalysis.InProgress)
                analysis.Status = StatusAnalysis.Error;
            analysis.EndAnalysis = DateTime.UtcNow;
            analysis.MessageError = message;

            analysisRepo.SaveChanges();
        }

        internal void AddClass(ClassAnalysis classAnalysis)
        {
            var analysis = analysisRepo.GetById(classAnalysis.AnalysisId);

            if (analysis == null)
                throw new Exception("Analysis not found");

            if (analysis.Status != StatusAnalysis.InProgress)
                throw new Exception("It is only allowed to add files under analysis in progress.");

            classRepo.Add(classAnalysis);
            classRepo.SaveChanges();
        }

        internal bool VerifyExistsFile(byte[] file, out long? id)
        {
            return fileRepo.GetIdFileByHash(Util.GenerateMD5(file), out id);
        }

        internal ControlFlowAnalysis? GetById(long id)
        {
            return analysisRepo.GetById(id);
        }
        internal IEnumerable<ControlFlowAnalysis> GetAll()
        {
            return analysisRepo.GetAll();
        }

        internal ClassAnalysis? GetClassById(long id)
        {
            return classRepo.GetById(id);
        }
        internal void Delete(long id)
        {
            var analysis = analysisRepo.GetById(id);
            if (analysis == null)
                throw new Exception("Analysis not found");
            analysisRepo.Delete(analysis);
            analysisRepo.SaveChanges();
        }

        public IEnumerable<ViewModels.FileManagerItemViewModel> Convert(IEnumerable<ClassAnalysis> classes, IEnumerable<string> files)
        {
            FileManagerItemViewModel root = new FileManagerItemViewModel()
            {
                FilesAndPaths = new List<FileManagerItemViewModel>()
            };

            foreach (string path in files)
            {
                AddFileToHierarchy(classes, root, path, path);
            }

            return root.FilesAndPaths;
        }

        static void AddFileToHierarchy(IEnumerable<ClassAnalysis> classes, FileManagerItemViewModel current, string path, string pathComplete)
        {
            string[] parts = path.Split('/');
            string name = parts[0];

            FileManagerItemViewModel child = current.FilesAndPaths.Find(obj => obj.Name == name);

            if (child == null)
            {
                child = new FileManagerItemViewModel()
                {
                    Name = name,
                    FilesAndPaths = new List<FileManagerItemViewModel>(),
                    IsPath = parts.Length > 1,
                    IsFile = parts.Length == 1,
                    Extension = Path.GetExtension(name),
                    Path = pathComplete,
                    Fullname = pathComplete,
                    IdClass = classes.FirstOrDefault(x => x.FullName == pathComplete)?.Id,
                };
                current.FilesAndPaths.Add(child);
            }

            if (parts.Length > 1)
            {
                string remainingPath = string.Join("/", parts, 1, parts.Length - 1);
                AddFileToHierarchy(classes, child, remainingPath, pathComplete);
            }
        }

    }
}
