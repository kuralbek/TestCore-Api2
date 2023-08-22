﻿using Microsoft.AspNetCore.Cors;
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

        public AuthAndRegist(IConfiguration configuration)
        {
            this.config = configuration;
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

                    string token = CreateToke(tempUser);
                    return Ok(token);

                }
                catch (Exception ex)
                {
                    return Content(ex.Message);
                }
                
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


        private string CreateToke(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(ClaimTypes.Role,"Admin")
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