using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Business.Services;

public class ProjectService(IProjectRepository projectRepository, ICustomerService customerService, IServiceService serviceService) : IProjectService
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly ICustomerService _customerService = customerService;
    private readonly IServiceService _serviceService = serviceService;

    public async Task<IResponseResult> CreateProjectAsync(ProjectRegistrationForm projectForm)
    {
        // manuel validation 
        if (projectForm.CustomerForm == null && projectForm.CustomerId == 0)
            return Result.BadRequest("Customer form or existing customer must be provided");
        if (projectForm.ServiceForm == null && projectForm.ServiceId == 0)
            return Result.BadRequest("Service form or existing service must be provided");

        List<ValidationResult> errors = ValidateRegistrationFormService.Validate<ProjectRegistrationForm>(projectForm);
        if (errors?.Count != 0 && errors != null)
        {
            return Result<List<ValidationResult>>.BadRequest(errors);
        }

        var dataContext = _projectRepository.GetContext();
        using var transaction = await dataContext.Database.BeginTransactionAsync();

        try
        {                                                                                                                                                                                                                                                                      
            if(projectForm.CustomerForm != null)
            {
                var result = await _customerService.CreateCustomerAsync(projectForm.CustomerForm);
                Result<CustomerDto>? castResult = result as Result<CustomerDto>;
                if (castResult != null && castResult.Data?.Id != null)
                {
                    var customerId = castResult.Data.Id;
                    projectForm.CustomerId = customerId;
                }
                else
                {
                    return Result.InternalError(result.ErrorMessage ?? "Could not create customer");
                }
            } else
            {
                // check if entity exist
            }
            if (projectForm.ServiceForm != null)
            {
                var result = await _serviceService.CreateServicesAsync(projectForm.ServiceForm);
                Result<ServiceDto>? castResult = result as Result<ServiceDto>;
                if (castResult != null && castResult.Data?.Id != null)
                {
                    var serviceId = castResult.Data.Id;
                    projectForm.ServiceId = serviceId;
                }
                else
                {
                    return Result.InternalError(result.ErrorMessage ?? "Could not create service");
                }
            } else
            {
                // check if entity exist
            }
            var entity = ProjectFactory.CreateEntity(projectForm);
            ProjectEntity createdProject = await _projectRepository.CreateAsync(entity);
            if (createdProject == null)
            {
                await transaction.RollbackAsync();
                return Result.InternalError("Could not create project");
            }
            await transaction.CommitAsync();
            ProjectEntity fullProject = await _projectRepository.GetAsync(x => x.Id == createdProject.Id);
            return Result<ProjectDto>.Created(ProjectFactory.CreateDto(fullProject));
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
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
    public async Task<IResponseResult> GetProjectAsync(int id)
    {
        try
        {
            ProjectEntity project =  await _projectRepository.GetAsync(x => x.Id == id);
            if(project == null) return Result.InternalError("Could not  get project");
            return Result<ProjectDto>.Ok(ProjectFactory.CreateDto(project));
        }
        catch (Exception ex) 
        {
            Debug.WriteLine(ex.Message);
            return Result.InternalError("Could not  get project");
        }
    }
    public async Task<IResponseResult> UpdateProjectAsync(int id, ProjectRegistrationForm projectForm)
    {
        var dataContext = _projectRepository.GetContext();
        using var transaction = await dataContext.Database.BeginTransactionAsync();

        try
        {
            var projectExist = await _projectRepository.EntityExistsAsync(x => x.Id == id);
            if (projectExist == false) return Result.NotFound("project does not exist");

            if (projectForm.CustomerForm != null)
            {
                var result = (Result<CustomerDto>)await _customerService.CreateCustomerAsync(projectForm.CustomerForm);
                if (result.Success == true && result.Data?.Id != null)
                {
                    var customerId = result.Data.Id;
                    projectForm.CustomerId = customerId;

                }
                return Result.InternalError(result.ErrorMessage ?? "Could not create customer");
            }
            if (projectForm.ServiceForm != null)
            {
                var result = (Result<ServiceDto>)await _serviceService.CreateServicesAsync(projectForm.ServiceForm);
                if (result.Success == true && result.Data?.Id != null)
                {
                    var serviceId = result.Data.Id;
                    projectForm.ServiceId = serviceId;

                }
                await transaction.RollbackAsync();
                return Result.InternalError(result.ErrorMessage ?? "Could not create service");
            }
            await transaction.CommitAsync();
            return Result.Ok();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            Debug.WriteLine(ex.Message);
            return Result.InternalError("Could not update project");
        }
    }
    public async Task<IResponseResult> DeleteProjectAsync(int id)
    {
        try
        {
            var alreadyExist = await _projectRepository.EntityExistsAsync(x => x.Id == id);
            if (alreadyExist == false) return Result.NotFound("project does not exist");

            var result = await _projectRepository.DeleteAsync(x => x.Id == id);
            if(result == false) return Result.InternalError("Could not delete project");
            return Result.NoContent();
        }
        catch (Exception ex) 
        {
            Debug.WriteLine(ex.Message);
            return Result.InternalError("Could not delete project");
        }
    }
}
