using Business.Dtos;
using Data.Entities;
using System.Linq.Expressions;

namespace Business.Interfaces;

public interface IProjectService
{
    public Task<IResponseResult> GetAllProjectsAsync();
    public Task<IResponseResult> CreateProjectAsync(ProjectRegistrationForm projectForm);
}
