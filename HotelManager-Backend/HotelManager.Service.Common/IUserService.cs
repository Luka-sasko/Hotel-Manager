using HotelManager.Model.Common;
using System;
using System.Threading.Tasks;

namespace HotelManager.Service.Common
{
    public interface IUserService
    {
        Task<IUser> GetUserAsync();
        Task<bool> CreateUserAsync(IUser profile);
        Task<bool> UpdateUserAsync(IUser profile);
        Task<bool> DeleteUserAsync();
        Task<IUser> ValidateUserAsync(string username, string password);
        Task<string> GetRoleTypeByRoleIdAsync(Guid id);
        Task<string> GetUserEmailByIdAsync(Guid id);
        Task<bool> UpdatePasswordAsync(string passwordNew, string passwordOld);
    }
}
