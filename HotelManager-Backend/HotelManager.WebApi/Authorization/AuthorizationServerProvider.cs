using HotelManager.Repository;
using HotelManager.Service;
using HotelManager.Service.Common;
using Microsoft.Owin.Security.OAuth;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HotelManager.WebApi.Authorization
{
    public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private readonly IUserService _userService;
        public AuthorizationServerProvider()
        {
            _userService = new UserService(new UserRepository(), new RoleTypeRepository());
        }
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var user = await _userService.ValidateUserAsync(context.UserName, context.Password);
            if (user == null)
            {
                context.SetError("invalid_grant", "Provided email and password is incorrect");
                return;
            }
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            var role = await _userService.GetRoleTypeByRoleIdAsync(user.RoleId);
            identity.AddClaim(new Claim(ClaimTypes.Role, role));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            context.Validated(identity);
            
        }
    }
}