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
        public async Task<ObjectResult> GetAllProjects()
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
        [HttpGet("{id}")]
        public async Task<ObjectResult> GetProject(int id)
        {
            try
            {
                var result = await _projectService.GetProjectAsync(id);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return StatusCode(500, "An internal error occurred.");
            }
        }
        [HttpPut("{id}")]
        public async Task<ObjectResult> UpdateProject(int id, ProjectRegistrationForm form)
        {
            try
            {
                var result = await _projectService.UpdateProjectAsync(id, form);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return StatusCode(500, "An internal error occurred.");
            }
        }
        [HttpDelete("{id}")]
        public async Task<ObjectResult> DeleteProject(int id)
        {
            try
            {
                var result = await _projectService.DeleteProjectAsync(id);
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
