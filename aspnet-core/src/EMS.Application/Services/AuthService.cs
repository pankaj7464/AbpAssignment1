
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Security.Claims;
using Volo.Abp.Users;
using EMS.Permissions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using EMS.DTOs;
using EMS.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EMS.Services
{
    public class AuthService:IAuthService,IScopedDependency
    {

        protected IdentityUserManager UserManager;
        private readonly ICurrentUser _currentUser;
        private readonly IConfiguration _config;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IRepository<IdentityUser> _userRepository;
        private readonly EMSDbContext _context;
        public string PasswordlessLoginUrl { get; set; }
        public AuthService(
            UserManager<IdentityUser> userManager,
            IRepository<IdentityUser> userRepository,
            EMSDbContext eMSDbContext,
        IRepository<Volo.Abp.Identity.IdentityUser, Guid> userRepositoy,
            IConfiguration config,
         
            ICurrentUser currentUser)
        {

            _context = eMSDbContext;
            _userRepository = userRepository;
            _currentUser = currentUser;
            _userManager = userManager;
            _currentUser = currentUser;
            _config = config;
           

        }

        /// <summary>
        /// Logs in a user using a token and user ID for passwordless authentication.
        /// </summary>
        /// <param name="token">Token for authentication.</param>
        /// <param name="userId">ID of the user.</param>
        /// <returns>
        /// Returns an HTTP status code indicating success or failure along with a JWT token and user details upon successful login.
        /// </returns>
        public async Task<LoginResponse> Login(LoginModel model)
        {
           //var user =await _userRepository.FirstOrDefaultAsync(user=>user.Email == model.Email);
            var user =await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            var users = await _context.Users.ToListAsync();
            var isValid = await _userManager.CheckPasswordAsync(user, model.Password);
            if (user != null && isValid)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var claims = CreateClaims(user, roles);
                var token = await GenerateJwtTokenAsync(user, claims);
                return new LoginResponse { Data = user , Message = "You have login successfully",token = token,StatusCode =200};
            }
            return new LoginResponse {  Message = "Invalid Credentials" ,StatusCode=401};
        }


        /// <summary>
        /// Generates a JWT token for the specified user with the provided claims.
        /// </summary>
        /// <param name="user">The user for whom the token is generated.</param>
        /// <param name="claims">The claims to include in the token.</param>
        /// <returns>
        /// Returns JWT token as a string.
        /// </returns>
        private async Task<string> GenerateJwtTokenAsync(IUser user, IEnumerable<Claim> claims)
        {

            // Create symmetric security key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["jwt:key"]));

            // Create signing credentials
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create token descriptor
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1), // Token expiry time
                SigningCredentials = creds
            };

            // Create token handler
            var tokenHandler = new JwtSecurityTokenHandler();

            // Generate token
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Return token as string
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Creates claims for a user, including user ID, email, username, email verification status, and roles.
        /// </summary>
        /// <param name="user">The user for whom claims are created.</param>
        /// <param name="roles">Roles assigned to the user.</param>
        /// <returns>
        /// Returns a collection of claims containing user information and roles.
        /// </returns>
        private static IEnumerable<Claim> CreateClaims(IUser user, IEnumerable<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(AbpClaimTypes.UserId, user.Id.ToString()),
                new Claim(AbpClaimTypes.Email, user.Email),
                new Claim(AbpClaimTypes.UserName, user.UserName),
                new Claim(AbpClaimTypes.EmailVerified, user.EmailConfirmed.ToString().ToLower()),
            };

            if (!string.IsNullOrWhiteSpace(user.PhoneNumber))
            {
                claims.Add(new Claim(AbpClaimTypes.PhoneNumber, user.PhoneNumber));
            }

            foreach (var role in roles)
            {
                claims.Add(new Claim(AbpClaimTypes.Role, role));
            }

            return claims;
        }
    }
}
