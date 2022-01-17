using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PUS_lab3.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PUS_lab3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly DataMemory _context;

        public TokenController(IConfiguration configuration, DataMemory context)
        {
            _configuration = configuration;
            _context = context;

            context.Database.EnsureCreated();
        }

        [HttpPost]
        [Authorize]
        [AllowAnonymous]
        public async Task<IActionResult> Post(UserInfo userInfo)
        {
            if(userInfo != null && userInfo.UserName != null && userInfo.Password != null)
            {
                var user = await GetUser(userInfo.UserName, userInfo.Password);

                if(user != null)
                {
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                        new Claim(ClaimTypes.Role, user.UserRole)
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.Now.AddMinutes(15),
                        signingCredentials: signIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        //only if public [HttpGet]
        private async Task<UserInfo> GetUser(string userName, string passw)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName && u.Password == passw);
        }
    }
}
