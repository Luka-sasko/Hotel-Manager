using HotelManager.Model.Common;
using System;
using System.Threading.Tasks;

namespace HotelManager.Repository.Common
{
    public interface IUserRepository
    {
        Task<IUser> GetByIdAsync(Guid Id);
        Task<bool> CreateAsync(IUser Profile);
        Task<bool> UpdateAsync(Guid Id, IUser Profile);
        Task<bool> DeleteAsync(Guid Id);
        Task<IUser> ValidateUserAsync(string username, string password);
        Task<bool> UpdatePasswordAsync(Guid id, string passwordNew, string passwordOld);
    }
}
