using Business.Dtos;
using Data.Entities;

namespace Business.Factories;

internal class UserFactory
{
    public static UserRegistrationForm CreateRegistrationForm(string firstname, string lastname, string email)
    {
        return new UserRegistrationForm { Firstname = firstname, Lastname = lastname, Email = email };
    }
    public static UserDto CreateDto(UserEntity entity)
    {
        return new UserDto { Id = entity.Id, Firstname = entity.Firstname, Lastname = entity.Lastname, Email = entity.Email};
    }
    public static UserEntity CreateEntity(UserRegistrationForm form)
    {
        return new UserEntity { Firstname = form.Firstname, Lastname = form.Lastname, Email = form.Email };
    }
    public static UserEntity CreateEntity(int id, UserRegistrationForm form)
    {
        return new UserEntity { Id = id, Firstname = form.Firstname, Lastname = form.Lastname, Email = form.Email };
    }
}
