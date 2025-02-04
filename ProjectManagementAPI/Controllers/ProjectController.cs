using Business.Dtos;
using Business.Interfaces;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ProjectManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController(IProjectService projectService) : ControllerBase
    {

        private readonly IProjectService _projectService = projectService;

        [HttpPost]
        public async Task<ObjectResult> CreateProject([FromBody] ProjectRegistrationForm form)
        {
            try
            {
                var result = await _projectService.CreateProjectAsync(form);
                return StatusCode(result.StatusCode, result);
            } catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return StatusCode(500, "An internal error occurred.");
            }
        }

        [HttpGet]
        public async Task<ObjectResult> GetProject()
        {
            try
            {
                var result = await _projectService.GetAllProjectsAsync();
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return StatusCode(500, "An internal error occurred.");
            }
        }
    }
}
