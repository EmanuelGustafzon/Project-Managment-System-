using Business.Dtos;
using Data.Entities;

namespace Business.Factories;

public static class UserFactory
{
    public static UserRegistrationForm CreateRegistrationForm(string firstname, string lastname, string email, string phoneNumber)
    {
        var user = new UserRegistrationForm { Firstname = firstname, Lastname = lastname};
        if(user.ContactInformation != null)
        {
            user.ContactInformation.Email = email;
            user.ContactInformation.PhoneNumber = phoneNumber;
        }
        return user;
    }
    public static UserDto CreateDto(UserEntity entity)
    {
        var user = new UserDto { Id = entity.Id, FirstName = entity.Firstname, LastName = entity.Lastname };
        
        if (entity.ContactInformation != null)
        {
            user.ContactInformation = new();
            user.ContactInformation.Id = entity.ContactInformation.Id;
            user.ContactInformation.Email = entity.ContactInformation.Email;
            user.ContactInformation.PhoneNumber = entity.ContactInformation.PhoneNumber;
        }
        return user;
    }
    public static UserEntity CreateEntity(UserRegistrationForm form)
    {
        var user = new UserEntity { Firstname = form.Firstname, Lastname = form.Lastname };
        if (form.ContactInformation != null)
        {
            var contact = new UserContactEntity { Email = form.ContactInformation.Email, PhoneNumber = form.ContactInformation.PhoneNumber };
            user.ContactInformation = contact;
        }
        return user;
    }
    public static UserEntity CreateEntity(int id, UserRegistrationForm form)
    {
        var user = new UserEntity { Id = id, Firstname = form.Firstname, Lastname = form.Lastname};
        if (form.ContactInformation != null)
        {
            var contact = new UserContactEntity { Email = form.ContactInformation.Email, PhoneNumber = form.ContactInformation.PhoneNumber };
            user.ContactInformation = contact;
        }
        return user;
    }
}
