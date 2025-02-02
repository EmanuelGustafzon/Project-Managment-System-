using Business.Dtos;

namespace Business.Interfaces;

public interface IUserService
{
    public Task<IResponseResult> GetAllUsersAsync();
    public Task<IResponseResult> GetUserAsync(int id);
    public Task<IResponseResult> CreateUserAsync(UserRegistrationForm userform);
    public Task<IResponseResult> UpdateUserAsync(int id, UserRegistrationForm updatedUserForm);
    public Task<IResponseResult> DeleteUserAsync(int id);
}
