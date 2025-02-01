using Business.Dtos;
using Business.Interfaces;
using Business.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ProjectManagementAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ServiceController(IServiceService serviceService) : Controller
{
    private readonly IServiceService _serviceService = serviceService;

    [HttpPost]
    public async Task<ObjectResult> AddService([FromBody] ServiceRegistrationForm serviceForm)
    {
        try
        {
            var result = await _serviceService.CreateServicesAsync(serviceForm);
            return StatusCode(result.StatusCode, result.ErrorMessage);

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return StatusCode(500, "An internal error occurred.");
        }
    }

    [HttpGet]
    public async Task<ObjectResult> GetAllServices()
    {
        try
        {
            var result = await _serviceService.GetAllServicesAsync();
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return StatusCode(500, "An internal error occurred.");
        }
    }
    [HttpGet("{id}")]
    public async Task<ObjectResult> GetServiceById(int id)
    {
        try
        {
            var result = await _serviceService.GetServiceAsync(id);
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return StatusCode(500, "An internal error occurred.");
        }
    }
}
