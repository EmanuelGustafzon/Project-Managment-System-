using Data.Entities;
using System.Linq.Expressions;

namespace Business.Interfaces;

public interface IProjectService
{
    public Task<IEnumerable<ProjectEntity>> GetAllProjectsAsync();
    public Task<ProjectEntity> GetProjectAsync(Expression<Func<ProjectEntity, bool>> predicate);
    public Task<ProjectEntity> CreateProjectAsync(ProjectEntity entity);
    public Task<ProjectEntity> UpdateProjectAsync(Expression<Func<ProjectEntity, bool>> predicate, ProjectEntity updatedEntity);
    public Task<bool> DeleteProjectAsync(Expression<Func<ProjectEntity, bool>> predicate);
}
