
using Business.Dtos;
using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Business.Services;

public class CustomerService(ICutomerRepository customerRepository) : ICustomerService
{
    private readonly ICutomerRepository _customerRepository = customerRepository;

    public async Task<IResponseResult> GetAllCustomersAsync()
    {
        try
        {
            IEnumerable<CustomerEntity> customerEntity = await _customerRepository.GetAllAsync();
            if (!customerEntity.Any()) Result<IEnumerable<CustomerEntity>>.Ok([]);

            IEnumerable<CustomerDto> customerDtoList = customerEntity.Select(customerEntity => CustomerFactory.CreateDto(customerEntity));

            return Result<IEnumerable<CustomerDto>>.Ok(customerDtoList);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            return Result.InternalError("Failed to get the customers");
        }
    }
    public async Task<IResponseResult> GetCustomerAsync(int id)
    {
        try
        {
            CustomerEntity customerEntity = await _customerRepository.GetAsync(x => x.Id == id);
            if (customerEntity == null) return Result.NotFound("The customer was not found");

            CustomerDto customerDto = CustomerFactory.CreateDto(customerEntity);

            return Result<CustomerDto>.Ok(customerDto);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return Result.InternalError("Failed to get the customer");
        }
    }
    public async Task<IResponseResult> CreateCustomerAsync(CustomerRegistrationForm customerForm)
    {
        List<ValidationResult> errors = ValidateRegistrationFormService.Validate<CustomerRegistrationForm>(customerForm);
        if (errors?.Count != 0 && errors != null)
        {
            return Result<List<ValidationResult>>.BadRequest(errors);
        }
        try
        {
            bool alreadyExist = await _customerRepository.EntityExistsAsync(x => x.Name == customerForm.Name);
            if (alreadyExist) return Result.AlreadyExists("Customer already exists");


            CustomerEntity currencyEntity = CustomerFactory.CreateEntity(customerForm);

            var resultEntity = await _customerRepository.CreateAsync(currencyEntity);

            return Result<CustomerDto>.Created(CustomerFactory.CreateDto(resultEntity));
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return Result.InternalError("Failed to create Customer");
        }
    }
    public async Task<IResponseResult> UpdateCustomerAsync(int id, CustomerRegistrationForm updatedCustomerForm)
    {
        List<ValidationResult> errors = ValidateRegistrationFormService.Validate<CustomerRegistrationForm>(updatedCustomerForm);
        if (errors?.Count != 0 && errors != null)
        {
            return Result<List<ValidationResult>>.BadRequest(errors);
        }

        try
        {
            bool customerExists = await _customerRepository.EntityExistsAsync(x => x.Id == id);
            if (customerExists == false) return Result.NotFound($"Customer not found with the id: {id}");

            var updatedEntity = await _customerRepository.UpdateAsync(x => x.Id == id, CustomerFactory.CreateEntity(id, updatedCustomerForm));
            if (updatedEntity == null) return Result.InternalError("Failed to update the Customer");

            CustomerDto customerDto = CustomerFactory.CreateDto(updatedEntity);
            return Result<CustomerDto>.Ok(customerDto);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return Result.InternalError("Failed to update the Customer");
        }
    }

    public async Task<IResponseResult> DeleteCustomerAsync(int id)
    {
        try
        {
            bool customerExist = await _customerRepository.EntityExistsAsync(x => x.Id == id);
            if (customerExist == false) return Result.NotFound($"Customer not found with the id: {id}");

            bool result = await _customerRepository.DeleteAsync(x => x.Id == id);
            if (result == false) return Result.InternalError("Failed to delete the Customer");

            return Result.NoContent();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return Result.InternalError("failed to delete Customer");
        }
    }
}
