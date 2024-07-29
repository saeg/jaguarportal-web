using WebJaguarPortal.Repository.Interfaces;
using WebJaguarPortal.Models;
using NuGet.Protocol.Core.Types;

namespace WebJaguarPortal.Services
{
    public class ProjectService
    {
        private readonly IRepository<Project> repository;

        public ProjectService(IRepository<Project> repository)
        {
            this.repository = repository;
        }

        public void Add(Project project)
        {
            if (string.IsNullOrWhiteSpace(project.Name))
            {
                throw new ArgumentException($"'{nameof(project.Name)}' cannot be null or whitespace.", nameof(project.Name));
            }

            if (string.IsNullOrWhiteSpace(project.Description))
            {
                throw new ArgumentException($"'{nameof(project.Description)}' cannot be null or whitespace.", nameof(project.Description));
            }

            repository.Add(new Project()
            {
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Key = Guid.NewGuid().ToString(),
                Name = project.Name,
                Description = project.Description
            });
            repository.SaveChanges();
        }

        public void Update(Project project)
        {
            if (string.IsNullOrWhiteSpace(project.Name))
            {
                throw new ArgumentException($"'{nameof(project.Name)}' cannot be null or whitespace.", nameof(project.Name));
            }

            if (string.IsNullOrWhiteSpace(project.Description))
            {
                throw new ArgumentException($"'{nameof(project.Description)}' cannot be null or whitespace.", nameof(project.Description));
            }

            Project? proj = repository.GetById(project.Id);
            if (proj != null)
            {
                proj.UpdatedAt = DateTime.UtcNow;
                proj.Name = project.Name;
                proj.Description = project.Description;
                repository.SaveChanges();
            }
        }

        public void Delete(long id)
        {
            Project? proj = repository.GetById(id);

            if (proj != null)
            {
                repository.Delete(proj);
                repository.SaveChanges();
            }
        }

        public Project? GetById(long id)
        {
            return repository.GetById(id);
        }

        public IEnumerable<Project> GetAll()
        {
            return repository.GetAll();
        }
    }
}
