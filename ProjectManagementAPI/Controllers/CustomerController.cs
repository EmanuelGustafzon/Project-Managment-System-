using Business.Dtos;
using Business.Interfaces;
using Business.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ProjectManagementAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomerController(ICustomerService customerService) : Controller
{
    private readonly ICustomerService _customerService = customerService;

    [HttpPost]
    public async Task<ObjectResult> AddCustomer([FromBody] CustomerRegistrationForm customerForm)
    {
        try
        {
            var result = await _customerService.CreateCustomerAsync(customerForm);
            return StatusCode(result.StatusCode, result.ErrorMessage);

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return StatusCode(500, "An internal error occurred.");
        }
    }

    [HttpGet]
    public async Task<ObjectResult> GetAllCustomers()
    {
        try
        {
            var result = await _customerService.GetAllCustomersAsync();
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return StatusCode(500, "An internal error occurred.");
        }
    }

    [HttpGet("{id}")]
    public async Task<ObjectResult> GetCustomerById(int id)
    {
        try
        {
            var result = await _customerService.GetCustomerAsync(id);
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return StatusCode(500, "An internal error occurred.");
        }
    }
    [HttpPut("{id}")]
    public async Task<ObjectResult> UpdateCustomer(int id, [FromBody] CustomerRegistrationForm updatedCustomer)
    {
        try
        {
            var result = await _customerService.UpdateCustomerAsync(id, updatedCustomer);
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return StatusCode(500, "An internal error occurred.");
        }
    }
    [HttpDelete("{id}")]
    public async Task<ObjectResult> DeleteCustomer(int id)
    {
        try
        {
            var result = await _customerService.DeleteCustomerAsync(id);
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return StatusCode(500, "An internal error occurred.");
        }
    }
}
