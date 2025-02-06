﻿using Business.Dtos;
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
            StartTime = entity.StartTime,
            EndTime = entity.EndTime,
            Status = entity.Status.ToString(),
        };
        projectDto.ProjectManager = new();
        projectDto.ProjectManager.FirstName = entity.ProjectManager.Firstname;
        projectDto.ProjectManager.LastName = entity.ProjectManager.Lastname;
        projectDto.ProjectManager.Email = entity.ProjectManager.Email;

        projectDto.Customer = new();
        projectDto.Customer.Name = entity.Customer.Name;

        projectDto.Service = new();
        projectDto.Service.Name = entity.Service.Name;
        projectDto.Service.Price = entity.Service.Price;
        projectDto.Service.Unit = entity.Service.Unit.ToString();
        projectDto.Service.Currency = entity.Service.Currency.Currency;

        return projectDto;
    }

    public static ProjectEntity CreateEntity(ProjectRegistrationForm form)
    {

        var projectEntity = new ProjectEntity
        {
            Name = form.Name,
            StartTime = form.StartTime,
            EndTime = form.EndTime,
            Status = Enum.TryParse<StatusStates>(form.Status, true, out var unit) ? unit : default,
            ProjectManagerId = form.ProjectManagerId,
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
            Status = Enum.TryParse<StatusStates>(form.Status, true, out var unit) ? unit : default,
            ProjectManagerId = form.ProjectManagerId,
            ServiceId = (int)form.ServiceId!,
            CustomerId = (int)form.CustomerId!
        };
    }
}
