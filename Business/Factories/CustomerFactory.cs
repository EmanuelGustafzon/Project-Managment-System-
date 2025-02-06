using Business.Dtos;
using Data.Entities;

namespace Business.Factories;

public static class CustomerFactory
{
    public static CustomerDto CreateDto(CustomerEntity entity)
    {
        return new CustomerDto { Id = entity.Id , Name = entity.Name, OrganisationNumber = entity.OrgansisationNumber};
    }
    public static CustomerEntity CreateEntity(CustomerRegistrationForm form)
    {
        return new CustomerEntity { Name = form.Name, OrgansisationNumber = form.OrganisationNumber };
    }
    public static CustomerEntity CreateEntity(int id, CustomerRegistrationForm form)
    {
        return new CustomerEntity { Id = id, Name = form.Name, OrgansisationNumber = form.OrganisationNumber};
    }
}
