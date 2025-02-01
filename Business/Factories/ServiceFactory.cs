using Business.Dtos;
using Data.Entities;
using Data.Enums;

namespace Business.Factories;

public static class ServiceFactory
{
    public static ServiceRegistrationForm CreateRegistrationForm()
    {
        return new ServiceRegistrationForm();
    }
    public static ServiceRegistrationForm CreateRegistrationForm(string name, string? description, decimal price, int currencyId, string unit)
    {
        return new ServiceRegistrationForm
        {
            Name = name,
            Description = description,
            Price = price,
            CurrencyId = currencyId,
            Unit = unit
        };
    }
    public static ServiceDto CreateDto()
    {
        return new ServiceDto();
    }
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
    public static ServiceEntity CreateServiceEntity(ServiceRegistrationForm serviceDto) 
    {
        return new ServiceEntity
        {
            Name = serviceDto.Name,
            Description = serviceDto.Description,
            Price = serviceDto.Price,
            CurrencyId = serviceDto.CurrencyId,
            Unit = Enum.TryParse<Units>(serviceDto.Unit, true, out var unit) ? unit : default
        };
    }
    public static ServiceEntity CreateServiceEntity(int id, ServiceRegistrationForm serviceDto)
    {
        return new ServiceEntity
        {
            Id = id,
            Name = serviceDto.Name,
            Description = serviceDto.Description,
            Price = serviceDto.Price,
            CurrencyId = serviceDto.CurrencyId,
            Unit = Enum.TryParse<Units>(serviceDto.Unit, true, out var unit) ? unit : default
        };
    }
}
