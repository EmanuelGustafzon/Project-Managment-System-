using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using System.Diagnostics;

namespace Business.Services;

public class ProjectService(IProjectRepository projectRepository) : IProjectService
{
    private readonly IProjectRepository _projectRepository = projectRepository;

    public async Task<IResponseResult> CreateProjectAsync(ProjectRegistrationForm projectForm)
    {
        try
        {
            var entity = ProjectFactory.CreateEntity(projectForm);
            ProjectEntity createdProject = await _projectRepository.CreateAsync(entity);
            if (createdProject == null) return Result.InternalError("Could not create project");

            return Result<ProjectEntity>.Created(createdProject);

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return Result.InternalError("Could not create project");
        }
    }

    public async Task<IResponseResult> GetAllProjectsAsync()
    {
        try
        {
            IEnumerable<ProjectEntity> entities = await _projectRepository.GetAllAsync();
            if (!entities.Any()) return Result<IEnumerable<ProjectDto>>.Ok([]);

            IEnumerable<ProjectDto> projectDtoList = entities.Select(projectEntity => ProjectFactory.CreateDto(projectEntity));
            return Result<IEnumerable<ProjectDto>>.Ok(projectDtoList);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return Result.InternalError("Could not get projects");
        }
    }
}
