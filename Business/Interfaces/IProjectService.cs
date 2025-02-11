using Business.Dtos;
using Data.Entities;
using System.Linq.Expressions;

namespace Business.Interfaces;

public interface IProjectService
{
    public Task<IResponseResult> GetAllProjectsAsync();
    public Task<IResponseResult> GetProjectAsync(int id);
    public Task<IResponseResult> CreateProjectAsync(ProjectRegistrationForm projectForm);
    public Task<IResponseResult> UpdateProjectAsync(int id, ProjectRegistrationForm projectForm);
    public Task<IResponseResult> DeleteProjectAsync(int id);

    public IResponseResult GetAllStatuses();
}
