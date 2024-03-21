using AutoMapper;
using HotelManager.Model;
using HotelManager.Model.Common;
using HotelManager.Service.Common;
using HotelManager.WebApi.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace HotelManager.WebApi.Controllers
{
    [RoutePrefix("api/User")]
    public class UserController : ApiController
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public UserController(IUserService userService, IMapper mapper) {
            _userService = userService;
            _mapper = mapper;
        }

        // GET api/Prfile/5
        [Authorize(Roles = "Admin, User")]
        [HttpGet]
        [Route("")]
        public async Task<HttpResponseMessage> GetUserAsync()
        {
            try
            {
                IUser profile = await _userService.GetUserAsync();

                if (profile == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
                var profileView = _mapper.Map<UserView>(profile);
                profileView.Role = await _userService.GetRoleTypeByRoleIdAsync(profile.RoleId);
                return Request.CreateResponse(HttpStatusCode.OK, profileView);
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,ex);
            }
        }

        // POST api/Profile
        [HttpPost]
        [Route("")]
        public async Task<HttpResponseMessage> CreateUserAsync(UserRegistered newProfile)
        {
            if(newProfile == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            IUser profile = _mapper.Map<User>(newProfile);
            try
            {
                bool created = await _userService.CreateUserAsync(profile);
                if (created) return Request.CreateResponse(HttpStatusCode.OK);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError,ex);
            }
        }

        
        //PUT api/Profile/5
        [Authorize(Roles = "Admin, User")]
        [HttpPut]
        [Route("")]
        public async Task<HttpResponseMessage> UpdateUserAsync(UserUpdated updatedProfile)
        {
            if (updatedProfile == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            IUser profileInBase = await _userService.GetUserAsync();
            if (profileInBase == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            IUser user = _mapper.Map<User>(updatedProfile);
            try
            {
                bool updated = await _userService.UpdateUserAsync(user);
                if (updated) return Request.CreateResponse(HttpStatusCode.OK);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
        
        [Authorize(Roles = "User,Admin")]
        [HttpPut]
        [Route("updatePassword")]
        public async Task<HttpResponseMessage> UpdatePasswordAsync([FromBody] PasswordUpdateModel passwordUpdateModel)
        {
            if (passwordUpdateModel == null || string.IsNullOrEmpty(passwordUpdateModel.PasswordNew))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            IUser profileInBase = await _userService.GetUserAsync();
            if (profileInBase == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            try
            {
                bool updated = await _userService.UpdatePasswordAsync(passwordUpdateModel.PasswordNew, passwordUpdateModel.PasswordOld);
                if (updated) return Request.CreateResponse(HttpStatusCode.OK, "Password updated");
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        public class PasswordUpdateModel
        {
            public string PasswordOld { get; set; }
            public string PasswordNew { get; set; }
        }

    }
}
