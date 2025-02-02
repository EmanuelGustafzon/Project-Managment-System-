using Business.Dtos;
using Data.Entities;

namespace Business.Factories;

internal class CustomerFactory
{
    public static CustomerRegistrationForm CreateRegistrationForm(string name)
    {
        return new CustomerRegistrationForm { Name = name };
    }
    public static CustomerDto CreateDto(CustomerEntity entity)
    {
        return new CustomerDto { Id = entity.Id, Name = entity.Name };
    }
    public static CustomerEntity CreateEntity(CustomerRegistrationForm form)
    {
        return new CustomerEntity { Name = form.Name };
    }
    public static CustomerEntity CreateEntity(int id, CustomerRegistrationForm form)
    {
        return new CustomerEntity { Id = id, Name = form.Name};
    }
}
