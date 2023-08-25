using System.Security.Claims;

namespace TestCore_Api.Controllers.Services.UserService
{
    public class UserService : IUserServices
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public UserService(IHttpContextAccessor httpContextAccessor) 
        {
           this._contextAccessor = httpContextAccessor;
        }
        public string GetMyName()
        {
            var results = string.Empty;
            if(_contextAccessor.HttpContext != null)
            {
                results = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            }

            return results;
        }
    }
}
