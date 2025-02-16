using Business.Dtos;
using Data.Entities;
using Data.Enums;
using System.Diagnostics;

namespace Business.Factories;

public static class ProjectFactory
{
    public static ProjectDto CreateDto(ProjectEntity entity)
    {

        var projectDto = new ProjectDto
        {
            Id = entity.Id,
            Name = entity.Name,
            TotalPrice = entity.TotalPrice,
            StartTime = entity.StartTime,
            EndTime = entity.EndTime,
            Status = entity.Status.ToString(),
        };
        projectDto.ProjectManager = new()
        {
            Id = entity.ProjectManager.Id,
            FirstName = entity.ProjectManager.Firstname,
            LastName = entity.ProjectManager.Lastname,
            Email = entity.ProjectManager.Email
        };

        projectDto.Customer = new()
        {
            Id = entity.Customer.Id,
            Name = entity.Customer.Name,
            OrganisationNumber = entity.Customer.OrgansisationNumber
        };

        projectDto.Service = new()
        {
            Id = entity.Service.Id,
            Name = entity.Service.Name,
            Price = entity.Service.Price,
            Unit = entity.Service.Unit.ToString(),
            Currency = entity.Service.Currency.Currency
        };

        return projectDto;
    }

    public static ProjectEntity CreateEntity(ProjectRegistrationForm form)
    {

        var projectEntity = new ProjectEntity
        {
            Name = form.Name,
            TotalPrice = (decimal)form.TotalPrice!, 
            StartTime = form.StartTime,
            EndTime = form.EndTime,
            Status = Enum.TryParse<StatusStates>(form.Status, true, out var unit) ? unit : default,
            ProjectManagerId = (int)form.ProjectManagerId!,
            ServiceId = (int)form.ServiceId!,
            CustomerId = (int)form.CustomerId!
        };

        return projectEntity; 
    }
    public static ProjectEntity CreateEntity(int id, ProjectRegistrationForm form)
    {

        return new ProjectEntity
        {
            Id = id,
            Name = form.Name,
            StartTime = form.StartTime,
            EndTime = form.EndTime,
            TotalPrice = (decimal)form.TotalPrice!,
            Status = Enum.TryParse<StatusStates>(form.Status, true, out var unit) ? unit : default,
            ProjectManagerId = (int)form.ProjectManagerId!,
            ServiceId = (int)form.ServiceId!,
            CustomerId = (int)form.CustomerId!
        };
    }
    public static ProjectRegistrationForm CreateRegistrationForm(
    string name,
    decimal totalPrice,
    DateTime start,
    DateTime end,
    string status,
    int projectManagerId = 0,
    int customerId = 0,
    int serviceId = 0,
    UserRegistrationForm? userForm = null,
    CustomerRegistrationForm? customerForm = null,
    ServiceRegistrationForm? serviceForm = null)
    {
        return new ProjectRegistrationForm
        {
            Name = name,
            TotalPrice = totalPrice,
            StartTime = start,
            EndTime = end,
            Status = status,
            ProjectManagerId = projectManagerId,
            CustomerId = customerId,
            ServiceId = serviceId,
            UserForm = userForm,
            CustomerForm = customerForm,
            ServiceForm = serviceForm
        };
    }
}
