using Business.Interfaces;
using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using System.Linq.Expressions;

namespace Business.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;

    public ProjectService(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public Task<ProjectEntity> CreateProjectAsync(ProjectEntity entity)
    {
        return _projectRepository.CreateAsync(entity);
    }

    public Task<bool> DeleteProjectAsync(Expression<Func<ProjectEntity, bool>> predicate)
    {
        return _projectRepository.DeleteAsync(predicate);
    }

    public Task<IEnumerable<ProjectEntity>> GetAllProjectsAsync()
    {
        return _projectRepository.GetAllAsync();
    }

    public Task<ProjectEntity> GetProjectAsync(Expression<Func<ProjectEntity, bool>> predicate)
    {
        return _projectRepository.GetAsync(predicate);
    }

    public Task<ProjectEntity> UpdateProjectAsync(Expression<Func<ProjectEntity, bool>> predicate, ProjectEntity updatedEntity)
    {
        return _projectRepository.UpdateAsync(predicate, updatedEntity);
    }
}
