﻿using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Business.Services;

public class UserService(IUserRepository userRepository, IUserContactRepository userContactRepository) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUserContactRepository _userContactRepository = userContactRepository;

    public async Task<IResponseResult> GetAllUsersAsync()
    {
        try
        {
            IEnumerable<UserEntity> userEntities = await _userRepository.GetAllAsync();
            if (!userEntities.Any()) Result<IEnumerable<UserEntity>>.Ok([]);

            IEnumerable<UserDto> userDtoList = userEntities.Select(userEntity => UserFactory.CreateDto(userEntity));

            return Result<IEnumerable<UserDto>>.Ok(userDtoList);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            return Result.InternalError("Failed to get the users");
        }
    }
    public async Task<IResponseResult> GetUserAsync(int id)
    {
        try
        {
            var userEntity = await _userRepository.GetAsync(x => x.Id == id);
            if (userEntity == null) return Result.NotFound("The user was not found");

            UserDto userDto = UserFactory.CreateDto(userEntity);

            return Result<UserDto>.Ok(userDto);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return Result.InternalError("Failed to get the user");
        }
    }
    public async Task<IResponseResult> CreateUserAsync(UserRegistrationForm userForm)
    {
        List<ValidationResult> errors = ValidateRegistrationFormService.Validate<UserRegistrationForm>(userForm);
        if (errors?.Count != 0 && errors != null)
        {
            return Result<List<ValidationResult>>.BadRequest(errors);
        }
        try
        {
            UserEntity userEntity = UserFactory.CreateEntity(userForm);

            var resultEntity = await _userRepository.CreateAsync(userEntity);

            return Result<UserDto>.Created(UserFactory.CreateDto(resultEntity));
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return Result.InternalError("Failed to create user");
        }
    }
    public async Task<IResponseResult> UpdateUserAsync(int id, UserRegistrationForm updatedUserForm)
    {
        if (updatedUserForm == null) return Result.BadRequest("No user form provided");

        List<ValidationResult> errors = ValidateRegistrationFormService.Validate<UserRegistrationForm>(updatedUserForm);
        if (errors?.Count != 0 && errors != null)
        {
            return Result<List<ValidationResult>>.BadRequest(errors);
        }

        try
        {
            UserEntity user = await _userRepository.GetAsync(x => x.Id == id);
            if (user == null) return Result.NotFound("User does not exist");

            if (user.ContactInformation != null && updatedUserForm.ContactInformation == null) await RemoveContactInformation(id);

            var updatedEntity = updatedUserForm != null ? await _userRepository.UpdateAsync(x => x.Id == id, UserFactory.CreateEntity(id, updatedUserForm)) : null;
            if (updatedEntity == null) return Result.InternalError("Failed to update the User");

            UserDto userDto = UserFactory.CreateDto(updatedEntity);
            return Result<UserDto>.Ok(userDto);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return Result.InternalError("Failed to update the user");
        }
    }

    public async Task<IResponseResult> DeleteUserAsync(int id)
    {
        try
        {
            bool userExists = await _userRepository.EntityExistsAsync(x => x.Id == id);
            if (userExists == false) return Result.NotFound($"User not found with the id: {id}");

            bool result = await _userRepository.DeleteAsync(x => x.Id == id);
            if (result == false) return Result.InternalError("Failed to delete the user");

            return Result.NoContent();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return Result.InternalError("failed to delete user");
        }
    }

    public async Task<IResponseResult> RemoveContactInformation(int userId)
    {
        try
        {
            UserEntity user = await _userRepository.GetAsync(x => x.Id == userId);
            if (user == null) return Result.NotFound("User does not exist");

            if (user.ContactInformation != null) Result.NotFound("The user does not have contact information");

            bool contactInfoDeleted = user.ContactInformation != null ? await _userContactRepository.DeleteAsync(x => x.Id == user.ContactInformation.Id) : false;
            if(contactInfoDeleted == false) return Result.InternalError("Failed to remove contact informaton");

            return Result.NoContent();

        } catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return Result.InternalError("Failed to remove contact informaton");
        }
    }
}
