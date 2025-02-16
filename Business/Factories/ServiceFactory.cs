using Business.Dtos;
using Data.Entities;
using Data.Enums;
using System.Diagnostics;

namespace Business.Factories;

public static class ServiceFactory
{
    public static ServiceDto CreateDto(ServiceEntity serviceEntity)
    {
        return new ServiceDto
        {
            Id = serviceEntity.Id,
            Name = serviceEntity.Name,
            Description = serviceEntity.Description ?? "",
            Price = serviceEntity.Price,
            Currency = serviceEntity.Currency.Currency,
            Unit = serviceEntity.Unit.ToString()
        };
    }
    public static ServiceEntity CreateEntity(ServiceRegistrationForm form, int currencyId) 
    {
        return new ServiceEntity
        {
            Name = form.Name,
            Description = form.Description,
            Price = form.Price,
            CurrencyId = currencyId,
            Unit = Enum.TryParse<Units>(form.Unit, true, out var unit) ? unit : default
        };
    }
    public static ServiceEntity CreateEntity(int id, int currencyId, ServiceRegistrationForm form)
    {
        return new ServiceEntity
        {
            Id = id,
            Name = form.Name,
            Description = form.Description,
            Price = form.Price,
            CurrencyId = currencyId,
            Unit = Enum.TryParse<Units>(form.Unit, true, out var unit) ? unit : default
        };
    }
    public static ServiceRegistrationForm CreateRegistrationForm(string name, string description, decimal price, string currency, string unit)
    {
        return new ServiceRegistrationForm { Name = name, Description = description, Price = price, Currency = currency, Unit = unit};
    }
}
