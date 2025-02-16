using Business.Dtos;
using Data.Entities;

namespace Business.Factories;

public static class UserFactory
{
    public static UserDto CreateDto(UserEntity entity)
    {
        return new UserDto { Id = entity.Id, FirstName = entity.Firstname, LastName = entity.Lastname, Email = entity.Email};
    }
    public static UserEntity CreateEntity(UserRegistrationForm form)
    {
        return new UserEntity { Firstname = form.Firstname, Lastname = form.Lastname, Email = form.Email };
    }
    public static UserEntity CreateEntity(int id, UserRegistrationForm form)
    {
        return new UserEntity { Id = id, Firstname = form.Firstname, Lastname = form.Lastname, Email = form.Email };
    }

    public static UserRegistrationForm CreateRegistrationForm(string firstname, string lastname, string email)
    {
        return new UserRegistrationForm { Firstname = firstname, Lastname = lastname, Email = email };
    }
}
