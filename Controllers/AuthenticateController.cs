using FakeXiechengAPI.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;

namespace FakeXiechengAPI.Controllers
{
    
    [Route("auth")]
    [ApiController]
    public class AuthenticateController: ControllerBase
    {

        private readonly IConfiguration _configuration;

        public AuthenticateController(IConfiguration configuration)
        {
            _configuration = configuration; // 注册配置服务
        }

        [AllowAnonymous]    // 允许任何人可访问
        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody] LoginDto loginDto)
        {
            // 验证用户名和密码
            Console.WriteLine("loginDto="+loginDto.Email);
            Console.WriteLine("loginDto="+loginDto.Password);

            // 创建jwt
            var signingAlgorithm = SecurityAlgorithms.HmacSha256;
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, "fake_user_id"),
                new Claim(ClaimTypes.Role, "Admin")
            };
            var secretByte = Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]);
            var signingKey = new SymmetricSecurityKey(secretByte);
            var signingCredentials = new SigningCredentials(signingKey, signingAlgorithm);

            var token = new JwtSecurityToken(
                issuer: _configuration["Authentication:Isssuer"],
                audience: _configuration["Authentication:Audience"],
                claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials
            );

            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
            // return 200 + jwt
            return Ok(tokenStr);
        }

        //[AllowAnonymous]
        //[HttpPost("register")]
        //public IActionResult

    }
}
