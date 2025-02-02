using Business.Dtos;

namespace Business.Interfaces;

public interface ICustomerService
{
    public Task<IResponseResult> GetAllCustomersAsync();
    public Task<IResponseResult> GetCustomerAsync(int id);
    public Task<IResponseResult> CreateCustomerAsync(CustomerRegistrationForm customerform);
    public Task<IResponseResult> UpdateCustomerAsync(int id, CustomerRegistrationForm updatedCustomerForm);
    public Task<IResponseResult> DeleteCustomerAsync(int id);
}
