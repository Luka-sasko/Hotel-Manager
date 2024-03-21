using HotelManager.Repository.Common;
using HotelManager.Service.Common;
using Microsoft.AspNet.Identity;
using System;
using System.Threading.Tasks;
using System.Web;

namespace HotelManager.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleTypeRepository _roleTypeRepository;
        

        public UserService(IUserRepository userRepository, IRoleTypeRepository roleTypeRepository)
        {
            _userRepository = userRepository;
            _roleTypeRepository = roleTypeRepository;
        }

        public async Task<Model.Common.IUser> GetUserAsync()
        {
            var userId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            try
            {
                return await _userRepository.GetByIdAsync(userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> CreateUserAsync(Model.Common.IUser profile)
        {
            var id = Guid.NewGuid();
            profile.CreatedBy = id;
            profile.UpdatedBy = id;
            profile.Id = id;
            profile.IsActive = true;
            try
            {
                return await _userRepository.CreateAsync(profile);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdateUserAsync (Model.Common.IUser profile)
        {
            var id = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            try
            {
                return await _userRepository.UpdateAsync(id, profile);
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> UpdatePasswordAsync(string passwordNew, string passwordOld)
        {
            var id = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            try
            {
                return await _userRepository.UpdatePasswordAsync(id,passwordNew,passwordOld);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> DeleteUserAsync()
        {
            var id = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            try
            {
                return await _userRepository.DeleteAsync(id);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Model.Common.IUser> ValidateUserAsync(string email, string password)
        {
            try
            {
                return await _userRepository.ValidateUserAsync(email, password);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> GetRoleTypeByRoleIdAsync(Guid id)
        {
            try
            {
                return await _roleTypeRepository.GetByIdAsync(id);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> GetUserEmailByIdAsync(Guid id)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                return user.Email;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
