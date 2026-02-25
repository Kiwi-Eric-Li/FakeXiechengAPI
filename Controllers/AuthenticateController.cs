using FakeXiechengAPI.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using FakeXiechengAPI.Services;
using AutoMapper;
using FakeXiechengAPI.Models;
using System.Collections.Generic;

namespace FakeXiechengAPI.Controllers
{
    
    [Route("auth")]
    [ApiController]
    public class AuthenticateController: ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ITouristRouteRepository _touristRouteRepository;

        public AuthenticateController(IMapper mapper, IConfiguration configuration, ITouristRouteRepository touristRouteRepository)
        {
            _mapper = mapper;
            _configuration = configuration; // 注册配置服务
            _touristRouteRepository = touristRouteRepository;
        }

        [AllowAnonymous]    // 允许任何人可访问
        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody] LoginDto loginDto)
        {
            // 验证用户名和密码
            var user = await _touristRouteRepository.ValidateLoginUserAsync(loginDto.Email, loginDto.Password);
            if (user == null)
            {
                return Unauthorized(new
                {
                    code = -1,
                    message = "用户名或密码错误"
                });
            }

            // 创建jwt
            var signingAlgorithm = SecurityAlgorithms.HmacSha256;
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                //new Claim(ClaimTypes.Role, "Admin")
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
            return Ok(new {
                code = 0,
                message = "登录成功",
                token = tokenStr
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> register([FromBody] RegisterDto registerDto)
        {
            // 先将 registerDto 映射为 User
            var userModel = _mapper.Map<User>(registerDto);
            var roleModel = await _touristRouteRepository.GetRoleByRoleName("user");
            userModel.RoleId = roleModel.Id;
            userModel.Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
            // 调用实现类AddUser
            _touristRouteRepository.AddUser(userModel);
            
            return Ok(new
            {
                code = 0,
                message = "注册成功"
            });
        }

    }
}
