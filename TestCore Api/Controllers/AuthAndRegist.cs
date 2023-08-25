using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using TestCore_Api.Context;
using TestCore_Api.Model;

namespace TestCore_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class AuthAndRegist : ControllerBase
    {
        //public static User user= new User();
        private readonly IConfiguration config;
        private readonly IUserServices _userServices;

        public AuthAndRegist(IConfiguration configuration, IUserServices userServices)
        {
            this.config = configuration;
            this._userServices = userServices;
        }

        [HttpGet, Authorize]
        public ActionResult<object> GetMe()
        {
            var userName = _userServices.GetMyName();
            return Ok(userName);
            /*var userName = User?.Identity?.Name;
            var userName2 = User.FindFirstValue(ClaimTypes.Name);
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            return Ok(new { userName,userName2,userRole});*/
        }


        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDTO requset)
        {
            CreatePasswordHash(requset.Password, out byte[] passHash, out byte[] passSalt);
            /*user.UserName = requset.UserName;
            user.passwordHash = passHash;
            user.passwordSalt = passSalt;*/

            using (ApplicContext appContext = new ApplicContext())
            {

                User temp = new User();
                try
                {
                    temp.UserName = requset.UserName;
                    temp.passwordHash = passHash;
                    temp.passwordSalt = passSalt;

                    appContext.Add(temp);

                    appContext.SaveChanges();

                    return Ok(temp);
                }
                catch (Exception ex)
                {
                    return Content(ex.Message);
                }


            }


        }

        private void CreatePasswordHash(string password, out byte[] passHash, out byte[] passSalt)
        {
            using (var hmc = new HMACSHA512())
            {
                passSalt = hmc.Key;
                passHash = hmc.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDTO request)
        {

            using (ApplicContext db = new ApplicContext())
            {
                try
                {
                    User tempUser = db.Users.Where(c => c.UserName == request.UserName).FirstOrDefault();

                    if (tempUser.UserName != request.UserName)
                    {
                        return BadRequest("User not found");
                    }

                    if (!VerifyPasswordHash(request.Password, tempUser.passwordHash, tempUser.passwordSalt))
                    {
                        return BadRequest("Wrong password");
                    }

                    string token = CreateToken(tempUser);
                    var refreshToken = GenerateRefreshToken();
                    SetRefreshToken(refreshToken, tempUser);
                    return Ok(token);

                }
                catch (Exception ex)
                {
                    return Content(ex.Message);
                }

            }

        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<string>> RefreshToken( string NameUser)
        {
            var refreshToken = Request.Cookies["refToken"];
            using (var db = new ApplicContext())
            {
                User tempUser = db.Users.Where(c => c.UserName == NameUser).FirstOrDefault();

                if (!tempUser.RefreshToken.Equals(refreshToken))
                {
                    return Unauthorized("Invalid Refresh Token");
                }else if( tempUser.TokenExpires < DateTime.Now)
                {
                    return Unauthorized("Token expired");
                }

                string token = CreateToken(tempUser);
                var newRefreshToken = GenerateRefreshToken();
                SetRefreshToken(newRefreshToken, tempUser);
                return Ok(token);
            }
                
            
        }

        private RefreshToken GenerateRefreshToken()
        {
            var refToken = new RefreshToken()
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now
            };
            return refToken;
        }
        private void SetRefreshToken(RefreshToken newRefreshToken,User user)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };
            Response.Cookies.Append("refToken", newRefreshToken.Token, cookieOptions);

            using (var db = new ApplicContext()) {
                User tempUser = db.Users.Where(c => c.UserId == user.UserId).FirstOrDefault();
                tempUser.RefreshToken = newRefreshToken.Token;
                tempUser.TokenCreated = newRefreshToken.Created;
                tempUser.TokenExpires = newRefreshToken.Expires;

                
                db.SaveChanges();
            }

        }


        private bool VerifyPasswordHash(string password, byte[] passHash, byte[] passSalt)
        {
            using (var hmac = new HMACSHA512(passSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passHash);
            }
        }


        private string CreateToken(User user)
        {
            string nameRole = null;
            using (var db = new ApplicContext()) 
            {
                var roleTx = db.Roles.Where(c => c.IdRole == user.roleId).FirstOrDefault();
                nameRole = roleTx.NameRole;
            }
                List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Role,nameRole)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(config.GetSection("AppSet:Token").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
