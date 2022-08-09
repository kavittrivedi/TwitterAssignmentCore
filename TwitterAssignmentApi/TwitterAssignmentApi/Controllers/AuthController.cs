using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TwitterAssignmentApi.Data;
using TwitterAssignmentApi.Dtos;
using TwitterAssignmentApi.Models;

namespace TwitterAssignmentApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            this._authRepository = authRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDto registerUserDto)
        {
            var response = await _authRepository.Register(
                new User
                {
                    FirstName = registerUserDto.FirstName,
                    LastName = registerUserDto.LastName,
                    Email = registerUserDto.Email,
                    ContactNumber = registerUserDto.ContactNumber,
                    LoginId = registerUserDto.LoginId,
                }, registerUserDto.Password
                );

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDto loginUserDto)
        {
            var response = await _authRepository.Login(loginUserDto.LoginId, loginUserDto.Password);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("ResetPassword")]
        [Authorize]
        public async Task<IActionResult> ResetUserPassword(ResetUserPasswordDto resetUserPasswordDto)
        {
            var response = await _authRepository.ResetUserPassword(resetUserPasswordDto);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            else
            {
                return Ok(response);
            }
        }

        [HttpGet("All")]
        [Authorize]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await _authRepository.GetUsers();

            return Ok(response);
        }

        [HttpGet("Search/{username}")]
        [Authorize]
        public async Task<IActionResult> Search(string username)
        {
            var response = await _authRepository.SearchUser(username);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
