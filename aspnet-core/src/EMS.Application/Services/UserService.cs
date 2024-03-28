using EMS.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace EMS.Services
{
    public class UserService : IdentityUserAppService, ITransientDependency
    {
        private readonly ICurrentUser _currentUser;
        private readonly IdentityUserManager _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        IIdentityRoleRepository _roleRepository;

        public UserService(
            IdentityUserManager userManager,
            IIdentityUserRepository userRepository,
            IIdentityRoleRepository roleRepository,
            IOptions<IdentityOptions> identityOptions,
            RoleManager<IdentityRole> roleManager,
            ICurrentUser currentUser)
            : base(userManager, userRepository, roleRepository, identityOptions)
        {
            _userManager = userManager;
            _currentUser = currentUser;
            _roleRepository = roleRepository;
            _roleManager = roleManager;
        }


        public async  Task<IdentityUserDto> addUser(ApplicationUserDto input)
        {
            // Determine the current user's role (e.g., "admin" or "hr").
            string currentUserRole = await GetCurrentRoleAsync(); // You need to implement this method.

            // Create the user based on the current user's role.
            switch (currentUserRole.ToLower())
            {
                case "admin":
                    // Create user with "hr" role.
                    return await CreateUserWithRoleAsync(input, "HR");
                case "hr":
                    // Create user with "employee" role.
                    return await CreateUserWithRoleAsync(input, "employee");
                default:
                    // Handle unknown roles.
                    throw new AbpException("Unknown role");
            }

        }

       
        private async Task<IdentityUserDto> CreateUserWithRoleAsync(ApplicationUserDto input, string roleName)
        {

            // Create the user with the specified role.
            var user = new IdentityUser(Guid.NewGuid(),input.UserName,input.Email, input.DepartmentId );

            var role = await _roleManager.FindByNameAsync(roleName);
            user.SetProperty("DepartmentId", input.DepartmentId);
            user.AddRole(role.Id); 

            var result = await _userManager.CreateAsync(user, input.Password);

            if (!result.Succeeded)
            {
                throw new AbpException(result.Errors.First().Description);
            }

            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
        }

        public async Task<string> GetCurrentRoleAsync()
        {
            var userId = _currentUser.Id;

            // Get the user entity from the UserManager
            var user = await _userManager.FindByIdAsync(userId.ToString());

            // Get the roles associated with the user
            var roles = await _userManager.GetRolesAsync(user);

            // For simplicity, assuming a user has only one role, return the first role
            return roles.FirstOrDefault();
        }
    }
}
