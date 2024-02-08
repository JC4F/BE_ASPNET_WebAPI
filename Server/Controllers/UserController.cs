using AutoMapper;
using BusinessObject.Commons;
using BusinessObject.Dtos;
using BusinessObject.Models;
using DataAccess.Interfaces;
using EmailService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Server.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;


        public UserController(IUserRepository userRepository, IConfiguration configuration, IMapper mapper, IEmailSender emailSender)
        {
            this.userRepository = userRepository;
            _configuration = configuration;
            _mapper = mapper;
            _emailSender = emailSender;
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSetting:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSetting:ValidIssuer"],
                audience: _configuration["JwtSetting:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        [HttpGet("all")]
        public async Task<ActionResult<APIResponse<IEnumerable<UserDto>>>> GetAll()
        {
            var users = await userRepository.GetAllUser();
            return new APIResponse<IEnumerable<UserDto>>
            {
                Data = _mapper.Map<IEnumerable<UserDto>>(users),
                Message = "Get all user success",
                IsSuccess = true
            };
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<Object>> GetMe()
        {
            var currentUser = HttpContext.User;
            var userEmail = currentUser.FindFirst(ClaimTypes.Name).Value;
            return new APIResponse<Object>
            {
                Data = userEmail,
                Message = "Get me data",
                IsSuccess = true
            };
        }

        [HttpPost("register")]
        public async Task<ActionResult<Object>> Register([FromBody] RegisterRequestDto registerDto)
        {
            if (!registerDto.Password.Equals(registerDto.RePassword))
            {
                return new BadRequestObjectResult(new APIResponse<Object>
                {
                    Message = "Password not match",
                    IsSuccess = false
                });
            }

            var user = await userRepository.FindUserByEmail(registerDto.Email);

            if (user != null)
            {
                return new BadRequestObjectResult(new APIResponse<Object>
                {
                    Message = "User existed",
                    IsSuccess = false
                });
            }

            var result = await userRepository.CreateUser(registerDto);

            return new APIResponse<int>
            {
                Message = "Register success",
                Data = result,
                IsSuccess = true
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<Object>> Login([FromBody] LoginRequestDto loginDto)
        {
            var user = await userRepository.FindUserByEmailAndPw(loginDto.Email, loginDto.Password);

            if (user == null)
            {
                return new BadRequestObjectResult(new APIResponse<Object>
                {
                    Message = "User not found",
                    IsSuccess = false
                });
            }

            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim("UserID", user.ID.ToString())
                };

            var token = GetToken(authClaims);
            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            var responseData = new LoginResponseDto
            {
                ID = user.ID,
                Email = loginDto.Email,
                Role = user.Role,
                Avatar = user.Avatar,
                DateOfBirth = user.DateOfBirth,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                AccessToken = accessToken
            };

            return new APIResponse<Object>
            {
                Message = "Login success",
                Data = responseData,
                IsSuccess = true
            };
        }

        [HttpPut("change-password")]
        public async Task<ActionResult<Object>> ChangePassword([FromBody] ChangePasswordRequestDto data)
        {
            try
            {
                var result = await userRepository.UpdatePassword(data);
                return new APIResponse<int>
                {
                    Message = "Change password success",
                    Data = result,
                    IsSuccess = true
                };
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(new APIResponse<Object>
                {
                    Message = e.Message,
                    IsSuccess = true
                });
            }
        }

        [HttpPost("token")]
        public async Task<ActionResult<APIResponse<Object>>> ForgotPassword([FromBody] TokenRequestDto data)
        {
            try
            {
                var token = await userRepository.GenareateTokenForgotPassword(data.Email);

                var message = new Message(new string[] { "quanlmhe163084@fpt.edu.vn" }, "Forgot Password", "This is the content from our email: " + token, null);
                await _emailSender.SendEmailAsync(message);

                return new APIResponse<Object>
                {
                    Message = "Send forgot password success",
                    IsSuccess = true
                };
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(new APIResponse<Object>
                {
                    Message = e.Message,
                    IsSuccess = true
                });
            }
        }

        [HttpPost("verify-token")]
        public async Task<ActionResult<APIResponse<Object>>> VerifyForgotPassword([FromBody] VerifyForgotPasswordDto data)
        {
            try
            {
                bool isVerified = await userRepository.VerifiedTokenForgotPasswordWithUpdate(data);

                if (!isVerified)
                {
                    return new BadRequestObjectResult(new APIResponse<Object>
                    {
                        Message = "Verify token fail!",
                        IsSuccess = false
                    });
                }

                return new APIResponse<Object>
                {
                    Message = "Update password success",
                    IsSuccess = true
                };
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(new APIResponse<Object>
                {
                    Message = e.Message,
                    IsSuccess = true
                });
            }
        }
    }
}