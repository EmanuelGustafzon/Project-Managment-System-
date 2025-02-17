using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Business.Services;
using Data.Contexts;
using Data.Interfaces;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ProjectManagerBackend_Tests;

public class ProjectService_Tests
{
    private readonly ServiceProvider _serviceProvider;
    public ProjectService_Tests()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase($"{Guid.NewGuid().ToString()}")
            .ConfigureWarnings(warnings => warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
        var services = new ServiceCollection();

        services.AddScoped<DataContext>(provider => new DataContext(options));
        services.AddMemoryCache();

        // repositories
        services.AddScoped<ICurrencyRepository, CurrencyRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IServiceRespository, ServiceRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();

        // services
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<ICurrencyService, CurrencyService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IServiceService, ServiceService>();
        services.AddScoped<ICustomerService, CustomerService>();

        _serviceProvider = services.BuildServiceProvider();
    }
    [Fact]
    public async Task CreateProjectAsync_WithFormsOfForeigKeys_ShouldCreateProject_AndReturnIResponse_WithDataIncludingProjectDto()
    {
        var user = UserFactory.CreateRegistrationForm("emanuel", "gustafzon", "e.g@live.se");
        var customer = CustomerFactory.CreateRegistrationForm("Ikea", "111");
        var service = ServiceFactory.CreateRegistrationForm("Web Design", "Awsome designs", 800, "sek", "Hour");
        var project = ProjectFactory.CreateRegistrationForm(
            "awsome project", 
            20000, 
            new DateTime(2025, 02, 15), 
            new DateTime(2025, 03, 15), 
            "OnGoing", 
            userForm: user,
            customerForm: customer,
            serviceForm: service
            );

        var projectService = _serviceProvider.GetRequiredService<IProjectService>();
        IResponseResult createdProject = await projectService.CreateProjectAsync( project );

        Assert.NotNull( createdProject );
        Assert.IsType<Result<ProjectDto>>(createdProject);
    }
    [Fact]
    public async Task CreateProjectAsync_WithProvidedForeignKeysIds_ShouldCreateProject_AndReturnIResponse_WithDataIncludingProjectDto()
    {
        await SeedDatabase();

        var project = ProjectFactory.CreateRegistrationForm(
            "awsome project",
            20000,
            new DateTime(2025, 02, 15),
            new DateTime(2025, 03, 15),
            "OnGoing",
            projectManagerId: 1,
            customerId: 1,
            serviceId: 1
            );

        var projectService = _serviceProvider.GetRequiredService<IProjectService>();
        IResponseResult createdProject = await projectService.CreateProjectAsync(project);

        Assert.NotNull(createdProject);
        Assert.IsType<Result<ProjectDto>>(createdProject);
    }
    [Fact]
    public async Task GetAllProjectsAsync_ShouldReturn_IResultResponse_IncludingDataListOfProjectDto()
    {
        await SeedDatabase();
        var projectService = _serviceProvider.GetRequiredService<IProjectService>();
        IResponseResult allProjects = await projectService.GetAllProjectsAsync();
        Assert.NotNull(allProjects);
        Assert.IsType<Result<IEnumerable<ProjectDto>>>(allProjects);
        if (allProjects is Result<IEnumerable<ProjectDto>> result)
        {
            Assert.NotNull(result.Data);
            Assert.NotEmpty(result.Data);
            Assert.Single(result.Data);
        }
    }
    [Fact]
    public async Task GeProjectAsync_ShouldReturn_IResultResponse_IncludingDataProjectDto()
    {
        await SeedDatabase();
        var projectService = _serviceProvider.GetRequiredService<IProjectService>();
        IResponseResult foundProject = await projectService.GetProjectAsync(1);
        Assert.NotNull(foundProject);
        Assert.True(foundProject.Success);
        Assert.IsType<Result<ProjectDto>>(foundProject);

    }
    [Fact]
    public async Task UpdateProjectAsyncShould_UpdateProjectAndreturn_IResponseResultWith_DataIncludingProjectDto()
    {
        await SeedDatabase();
        var project = ProjectFactory.CreateRegistrationForm(
            "new project",
            10000,
            new DateTime(2025, 03, 15),
            new DateTime(2025, 04, 15),
            "Finished",
            projectManagerId: 1,
            customerId: 1,
            serviceId: 1
        );
        var projectService = _serviceProvider.GetRequiredService<IProjectService>();
        IResponseResult updatedProject = await projectService.UpdateProjectAsync(1, project);

        Assert.NotNull(updatedProject);
        Assert.True(updatedProject.Success);
        Assert.IsType<Result<ProjectDto>>(updatedProject);
        if (updatedProject is Result<ProjectDto> result)
        {
            Assert.NotNull(result.Data);
            Assert.Equal("new project", result.Data.Name);
            Assert.Equal(10000, result.Data.TotalPrice);
        }
    }
    [Fact]
    public async Task DeleteProjectAsyncShould_DeleteProjectAnd_ReturnNoContent204()
    {
        await SeedDatabase();
        var projectService = _serviceProvider.GetRequiredService<IProjectService>();
        IResponseResult deleteProject = await projectService.DeleteProjectAsync(1);

        Assert.Equal(204, deleteProject.StatusCode);
    }

    private async Task SeedDatabase()
    {
        var user = UserFactory.CreateRegistrationForm("seed", "seed", "seed@live.com");
        var customer = CustomerFactory.CreateRegistrationForm("seed", "111");
        var service = ServiceFactory.CreateRegistrationForm("seed", "seed", 800, "seed", "Hour");
        var project = ProjectFactory.CreateRegistrationForm(
            "seedProject",
            20000,
            new DateTime(2025, 02, 15),
            new DateTime(2025, 03, 15),
            "OnGoing",
            projectManagerId: 1,
            customerId: 1,
            serviceId: 1
        );

        var userService = _serviceProvider.GetRequiredService<IUserService>();
        var customerService = _serviceProvider.GetRequiredService<ICustomerService>();
        var serviceService = _serviceProvider.GetRequiredService<IServiceService>();
        var projectService = _serviceProvider.GetRequiredService<IProjectService>();

        Assert.NotNull(userService);
        Assert.NotNull(customerService);
        Assert.NotNull(serviceService);
        Assert.NotNull(projectService);

        IResponseResult createdUser = await userService.CreateUserAsync(user);
        Assert.True(createdUser.Success);

        IResponseResult createdCustomer = await customerService.CreateCustomerAsync(customer);
        Assert.True(createdCustomer.Success);

        IResponseResult createdService = await serviceService.CreateServicesAsync(service);
        Assert.True(createdService.Success);

        IResponseResult createdProject = await projectService.CreateProjectAsync(project);
        Assert.True(createdProject.Success);
    }

}
