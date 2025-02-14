using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Contexts;
using Data.Entities;
using Data.Enums;
using Data.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Business.Services;

public class ProjectService(IProjectRepository projectRepository, ICustomerService customerService, IServiceService serviceService, IUserService userService) : IProjectService
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IUserService _userService = userService;
    private readonly ICustomerService _customerService = customerService;
    private readonly IServiceService _serviceService = serviceService;

    public async Task<IResponseResult> CreateProjectAsync(ProjectRegistrationForm projectForm)
    {
        if (projectForm.CustomerForm == null && projectForm.CustomerId == 0)
            return Result.BadRequest("Customer form or existing customer must be provided");
        if (projectForm.ServiceForm == null && projectForm.ServiceId == 0)
            return Result.BadRequest("Service form or existing service must be provided");
        if (projectForm.UserForm == null && projectForm.ProjectManagerId == 0)
            return Result.BadRequest("User form or project manager id of user must be proviced");

        List<ValidationResult> errors = ValidateRegistrationFormService.Validate<ProjectRegistrationForm>(projectForm);
        if (errors?.Count != 0 && errors != null)
        {
            return Result<List<ValidationResult>>.BadRequest(errors);
        }
        await _projectRepository.BeginTransactionAsync();
        try
        {
            var projectExist = await _projectRepository.GetAsync(x => x.Name == projectForm.Name) != null; 
            if(projectExist == true) return Result.AlreadyExists("project name already exist");

            if (projectForm.UserForm != null && projectForm.ProjectManagerId > 0)
            {
                (int userId, string? errorMessage) = await CreateUserAndGetId(projectForm.UserForm);
                if (userId != 0) projectForm.ProjectManagerId = userId;
                else return Result.Error(errorMessage!);
            }

            if (projectForm.CustomerForm != null && projectForm.CustomerId > 0)
            {
                (int customerId, string? errorMessage) = await CreateCustomerAndGetId(projectForm.CustomerForm);
                if (customerId != 0) projectForm.CustomerId = customerId;
                else
                {
                    await _projectRepository.RollbackTransactionAsync();
                    return Result.Error(errorMessage!);
                }
            }
            if (projectForm.ServiceForm != null && projectForm.ServiceId > 0)
            {
                (int serviceId, string? errorMessage) = await CreateServiceAndGetId(projectForm.ServiceForm);
                if (serviceId != 0) projectForm.ServiceId = serviceId;
                else
                {
                    Debug.WriteLine("hi");
                    await _projectRepository.RollbackTransactionAsync();
                    return Result.Error(errorMessage!);
                }
            }

            if(projectForm.TotalPrice == 0 || projectForm.TotalPrice == null)
            {
                var totalPrice = await GetCalculatedPrice(projectForm);
                projectForm.TotalPrice = totalPrice;
            }

            var entity = ProjectFactory.CreateEntity(projectForm);
            ProjectEntity createdProject = await _projectRepository.CreateAsync(entity);
            if (createdProject == null)
            {
                await _projectRepository.RollbackTransactionAsync();
                return Result.Error("Could not create project");
            }

            await _projectRepository.CommitTransactionAsync();

            ProjectEntity fullProject = await _projectRepository.GetAsync(x => x.Id == createdProject.Id);
            return Result<ProjectDto>.Created(ProjectFactory.CreateDto(fullProject));
        }
        catch (Exception ex)
        {
            await _projectRepository.RollbackTransactionAsync();
            Debug.WriteLine(ex.Message);
            return Result.Error("Could not create project");
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
            return Result.Error("Could not get projects");
        }
    }
    public async Task<IResponseResult> GetProjectAsync(int id)
    {
        try
        {
            ProjectEntity project =  await _projectRepository.GetAsync(x => x.Id == id);
            if(project == null) return Result.Error("Could not  get project");
            return Result<ProjectDto>.Ok(ProjectFactory.CreateDto(project));
        }
        catch (Exception ex) 
        {
            Debug.WriteLine(ex.Message);
            return Result.Error("Could not  get project");
        }
    }
    public async Task<IResponseResult> UpdateProjectAsync(int id, ProjectRegistrationForm projectForm)
    {
        if (projectForm.CustomerForm == null && projectForm.CustomerId == 0)
            return Result.BadRequest("Customer form or existing customer must be provided");
        if (projectForm.ServiceForm == null && projectForm.ServiceId == 0)
            return Result.BadRequest("Service form or existing service must be provided");
        if (projectForm.UserForm == null && projectForm.ProjectManagerId == 0)
            return Result.BadRequest("User form or project manager id of user must be proviced");

        List<ValidationResult> errors = ValidateRegistrationFormService.Validate<ProjectRegistrationForm>(projectForm);
        if (errors?.Count != 0 && errors != null)
        {
            return Result<List<ValidationResult>>.BadRequest(errors);
        }

        await _projectRepository.BeginTransactionAsync();

        try
        {
            var projectExist = await _projectRepository.EntityExistsAsync(x => x.Id == id);
            if (projectExist == false) return Result.NotFound("project does not exist");

            if (projectForm.UserForm != null && projectForm.ProjectManagerId > 0)
            {
                (int userId, string? errorMessage) = await CreateUserAndGetId(projectForm.UserForm);
                if (userId != 0) projectForm.ProjectManagerId = userId;
                else return Result.Error(errorMessage!);
            }

            if (projectForm.CustomerForm != null && projectForm.CustomerId > 0)
            {
                (int customerId, string? errorMessage) = await CreateCustomerAndGetId(projectForm.CustomerForm);
                if (customerId != 0) projectForm.CustomerId = customerId;
                else
                {
                    await _projectRepository.RollbackTransactionAsync();
                    return Result.Error(errorMessage!);
                }
            }
            if (projectForm.ServiceForm != null && projectForm.ServiceId > 0)
            {
                (int serviceId, string? errorMessage) = await CreateServiceAndGetId(projectForm.ServiceForm);
                if (serviceId != 0) projectForm.ServiceId = serviceId;
                else
                {
                    await _projectRepository.RollbackTransactionAsync();
                    return Result.Error(errorMessage!);
                }
            }

            if (projectForm.TotalPrice == 0 || projectForm.TotalPrice == null)
            {
                var totalPrice = await GetCalculatedPrice(projectForm);
                projectForm.TotalPrice = totalPrice;
            }

            var entity = ProjectFactory.CreateEntity(id, projectForm);
            ProjectEntity updatedProject = await _projectRepository.UpdateAsync(x => x.Id == id, entity);
            if (updatedProject == null)
            {
                await _projectRepository.RollbackTransactionAsync();
                return Result.Error("Could not update project");
            }
            await _projectRepository.CommitTransactionAsync();
            return Result.Ok();
        }
        catch (Exception ex)
        {
            await _projectRepository.RollbackTransactionAsync();
            Debug.WriteLine(ex.Message);
            return Result.Error("Could not update project");
        }
    }
    public async Task<IResponseResult> DeleteProjectAsync(int id)
    {
        try
        {
            var alreadyExist = await _projectRepository.EntityExistsAsync(x => x.Id == id);
            if (alreadyExist == false) return Result.NotFound("project does not exist");

            var result = await _projectRepository.DeleteAsync(x => x.Id == id);
            if(result == false) return Result.Error("Could not delete project");
            return Result.NoContent();
        }
        catch (Exception ex) 
        {
            Debug.WriteLine(ex.Message);
            return Result.Error("Could not delete project");
        }
    }

    public IResponseResult GetAllStatuses()
    {
        try
        {
            List<string> statusesAsString = [];

            StatusStates[] units = Enum.GetValues<StatusStates>();
            foreach (var item in units)
            {
                statusesAsString.Add(item.ToString());
            }
            return Result<List<string>>.Ok(statusesAsString);

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return Result.Error("Error retrieving units");
        }
    }

    // help methods
    private async Task<(int id, string? errorMessage)> CreateCustomerAndGetId(CustomerRegistrationForm form)
    {
        var result = await _customerService.CreateCustomerAsync(form);
        Result<CustomerDto>? castResult = result as Result<CustomerDto>;
        if (castResult != null && castResult.Data?.Id != null)
        {
            var customerId = castResult.Data.Id;
            return (customerId, null);
        }
        return (0, result.ErrorMessage);
    }
    private async Task<(int id, string? errorMessage)> CreateServiceAndGetId(ServiceRegistrationForm form)
    {
        var result = await _serviceService.CreateServicesAsync(form);
        Result<ServiceDto>? castResult = result as Result<ServiceDto>;
        if (castResult != null && castResult.Data?.Id != null)
        {
            var serviceId = castResult.Data.Id;
            return (serviceId, null);

        }
        return (0, result.ErrorMessage);
    }
    private async Task<(int id, string? errorMessage)> CreateUserAndGetId(UserRegistrationForm form)
    {
        var result = await _userService.CreateUserAsync(form);
        Result<UserDto>? castResult = result as Result<UserDto>;
        if (castResult != null && castResult.Data?.Id != null)
        {
            var userId = castResult.Data.Id;
            return (userId, null);

        }
        return (0, result.ErrorMessage);
    }

    private async Task<decimal> GetCalculatedPrice(ProjectRegistrationForm form)
    {
        try
        {
            var result = await _serviceService.GetServiceAsync((int)form.ServiceId!);
            var castedResult = result as Result<ServiceDto>;
            if (castedResult?.Data == null) return 0;

            decimal price = castedResult.Data.Price;
            Enum.TryParse<Units>(castedResult.Data.Unit, true, out var unit);
            
            decimal totalPrice = CalculateTotalProjectPriceService.CalculatePrice(form.StartTime, form.EndTime, unit, price);

            return totalPrice;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return 0;
        }
    } 
}
