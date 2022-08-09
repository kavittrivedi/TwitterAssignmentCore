using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TwitterAssignmentApi.Dtos;
using TwitterAssignmentApi.Models;

namespace TwitterAssignmentApi.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private IMapper _mapper;

        public AuthRepository(DataContext context, IConfiguration configuration, IMapper mapper)
        {
            this._context = context;
            this._configuration = configuration;
            this._mapper = mapper;
        }

        public async Task<ServiceResponse<string>> Login(string username, string password)
        {
            var response = new ServiceResponse<string>();
            var user = await _context.Users.Where(c => c.LoginId.ToLower() == username.ToLower() || c.Email.ToLower() == username.ToLower()).FirstOrDefaultAsync();
            if (user == null)
            {
                response.Success = false;
                response.Message = "User not found.";
            }
            else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Wrong password";
            }
            else
            {
                response.Data = CreateToken(user);
            }

            return response;

        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            ServiceResponse<int> response = new ServiceResponse<int>();
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            if (await UserExists(user.LoginId, user.Email))
            {
                response.Success = false;
                response.Message = "User already exists.";
                return response;
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            response.Data = user.Id;
            response.Message = "User registered successfully.";
            return response;
        }

        public async Task<bool> UserExists(string loginId, string email)
        {
            if (await _context.Users.AnyAsync(u => u.LoginId.ToLower() == loginId.ToLower() || u.Email.ToLower() == email.ToLower()))
            {
                return true;
            }
            return false;
        }

        public async Task<ServiceResponse<bool>> ResetUserPassword(ResetUserPasswordDto resetUserPasswordDto)
        {
            var response = new ServiceResponse<bool>();
            var user = await _context.Users.Where(x => x.LoginId.ToLower() == resetUserPasswordDto.UserName.ToLower() || x.Email == resetUserPasswordDto.UserName.ToLower()).FirstOrDefaultAsync();
            if (user == null)
            {
                response.Data = false;
                response.Success = false;
                response.Message = "User not found";
            }
            else
            {
                CreatePasswordHash(resetUserPasswordDto.Password, out byte[] passwordHash, out byte[] passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                response.Data = true;
            }

            return response;
        }

        public async Task<ServiceResponse<List<GetUserDto>>> GetUsers()
        {
            var response = new ServiceResponse<List<GetUserDto>>()
            {
                Data = _mapper.Map<List<GetUserDto>>(_context.Users.ToList())
            };

            return response;
        }

        public async Task<ServiceResponse<List<GetUserDto>>> SearchUser(string username)
        {
            var response = new ServiceResponse<List<GetUserDto>>();
            var users = _mapper.Map<List<GetUserDto>>(_context.Users.Where(x => x.Email.ToLower().Equals(username.ToLower()) || x.LoginId.ToLower().Equals(username.ToLower())));

            if (users.Any())
            {
                response.Data = users;
            }
            else
            {
                response.Success = false;
                response.Message = "No user found";
            }

            return response;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.LoginId),
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            SigningCredentials signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = signingCredentials,

            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
