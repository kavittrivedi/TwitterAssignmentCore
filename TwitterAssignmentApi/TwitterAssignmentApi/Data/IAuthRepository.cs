using TwitterAssignmentApi.Dtos;
using TwitterAssignmentApi.Models;

namespace TwitterAssignmentApi.Data
{
    public interface IAuthRepository
    {
        Task<ServiceResponse<int>> Register(User user, string password);

        Task<ServiceResponse<string>> Login(string username, string password);

        Task<bool> UserExists(string loginId, string email);

        Task<ServiceResponse<bool>> ResetUserPassword(ResetUserPasswordDto resetUserPasswordDto);

        Task<ServiceResponse<List<GetUserDto>>> GetUsers();

        Task<ServiceResponse<List<GetUserDto>>> SearchUser(string username);
    }
}
