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
        if (projectForm.StartTime > projectForm.EndTime)
            return Result.BadRequest("The project ending time must be later then the starting time");

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

            if (projectForm.UserForm != null && projectForm.ProjectManagerId == 0)
            {
                var result = await _userService.CreateUserAsync(projectForm.UserForm);
                var user = ResultResponseCastingService.CastResultAndGetData<UserDto>(result);
                if (user is null)
                {
                    await _projectRepository.RollbackTransactionAsync();
                    return Result.Error($"{result.ErrorMessage}");
                }
                projectForm.ProjectManagerId = user.Id;
            }
            if (projectForm.CustomerForm != null && projectForm.CustomerId == 0)
            {
                var result = await _customerService.CreateCustomerAsync(projectForm.CustomerForm);
                var customer = ResultResponseCastingService.CastResultAndGetData<CustomerDto>(result);
                if (customer is null)
                {
                    await _projectRepository.RollbackTransactionAsync();
                    return Result.Error($"{result.ErrorMessage}");
                }
                projectForm.CustomerId = customer.Id;
            }
            if (projectForm.ServiceForm != null && projectForm.ServiceId == 0)
            {
                var result = await _serviceService.CreateServicesAsync(projectForm.ServiceForm);
                var service = ResultResponseCastingService.CastResultAndGetData<ServiceDto>(result);
                if (service is null)
                {
                    await _projectRepository.RollbackTransactionAsync();
                    return Result.Error($"{result.ErrorMessage}");
                }
                projectForm.ServiceId = service.Id;
            }

            if (projectForm.TotalPrice == 0 || projectForm.TotalPrice == null)
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
            ProjectDto fullProjectDto = ProjectFactory.CreateDto(fullProject);
            return Result<ProjectDto>.Created(fullProjectDto);
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
        if (projectForm.StartTime > projectForm.EndTime)
            return Result.BadRequest("The project end time bust be later then the start time");

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

            if (projectForm.UserForm != null && projectForm.ProjectManagerId == 0)
            {
                var result = await _userService.CreateUserAsync(projectForm.UserForm);
                var user = ResultResponseCastingService.CastResultAndGetData<UserDto>(result);
                if (user is null)
                {
                    await _projectRepository.RollbackTransactionAsync();
                    return Result.Error($"{result.ErrorMessage}");
                }
                projectForm.ProjectManagerId = user.Id;
            }
            if (projectForm.CustomerForm != null && projectForm.CustomerId == 0)
            {
                var result = await _customerService.CreateCustomerAsync(projectForm.CustomerForm);
                var customer = ResultResponseCastingService.CastResultAndGetData<CustomerDto>(result);
                if (customer is null)
                {
                    await _projectRepository.RollbackTransactionAsync();
                    return Result.Error($"{result.ErrorMessage}");
                }
                projectForm.CustomerId = customer.Id;
            }
            if (projectForm.ServiceForm != null && projectForm.ServiceId == 0)
            {
                var result = await _serviceService.CreateServicesAsync(projectForm.ServiceForm);
                var service = ResultResponseCastingService.CastResultAndGetData<ServiceDto>(result);
                if (service is  null)
                {
                    await _projectRepository.RollbackTransactionAsync();
                    return Result.Error($"{result.ErrorMessage}");
                }     
                projectForm.ServiceId = service.Id;
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

            ProjectEntity fullProject = await _projectRepository.GetAsync(x => x.Id == updatedProject.Id);
            ProjectDto fullProjectDto = ProjectFactory.CreateDto(fullProject);
            return Result<ProjectDto>.Created(fullProjectDto);
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

    private async Task<decimal> GetCalculatedPrice(ProjectRegistrationForm form)
    {
        try
        {
            var result = await _serviceService.GetServiceAsync((int)form.ServiceId!);
            var service = ResultResponseCastingService.CastResultAndGetData<ServiceDto>(result);
            if (service is null) return 0;

            decimal price = service.Price;
            Enum.TryParse<Units>(service.Unit, true, out var unit);
            
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
