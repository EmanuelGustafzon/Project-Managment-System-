using Business.Dtos;
using Business.Interfaces;
using Business.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ProjectManagementAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(IUserService userService) : Controller
{
    private readonly IUserService _userService = userService;

    [HttpPost]
    public async Task<ObjectResult> AddUser([FromBody] UserRegistrationForm userForm)
    {
        try
        {
            var result = await _userService.CreateUserAsync(userForm);
            return StatusCode(result.StatusCode, result);

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return StatusCode(500, "An internal error occurred.");
        }
    }

    [HttpGet]
    public async Task<ObjectResult> GetAllUsers()
    {
        try
        {
            var result = await _userService.GetAllUsersAsync();
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return StatusCode(500, "An internal error occurred.");
        }
    }

    [HttpGet("{id}")]
    public async Task<ObjectResult> GetUserById(int id)
    {
        try
        {
            var result = await _userService.GetUserAsync(id);
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return StatusCode(500, "An internal error occurred.");
        }
    }
    [HttpPut("{id}")]
    public async Task<ObjectResult> UpdateUser(int id, [FromBody] UserRegistrationForm updatedUser)
    {
        try
        {
            var result = await _userService.UpdateUserAsync(id, updatedUser);
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return StatusCode(500, "An internal error occurred.");
        }
    }
    [HttpDelete("{id}")]
    public async Task<ObjectResult> DeleteUser(int id)
    {
        try
        {
            var result = await _userService.DeleteUserAsync(id);
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return StatusCode(500, "An internal error occurred.");
        }
    }
}
