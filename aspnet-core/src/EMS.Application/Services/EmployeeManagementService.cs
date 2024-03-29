using EMS.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace EMS.Services
{
    public class EmployeeManagementService : IdentityUserAppService, ITransientDependency
    {
        private readonly ICurrentUser _currentUser;
        private readonly IdentityUserManager _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        IIdentityRoleRepository _roleRepository;

        public EmployeeManagementService(
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
        [Authorize(Roles ="admin")]
        public async  Task<IdentityUserDto> humanResource(ApplicationUserDto input)
        {
            // Determine the current user's role (e.g., "admin" or "hr").

            string currentUserRole = _currentUser.Roles.FirstOrDefault();
            // You need to implement this method.

            // Create the user based on the current user's role.
            switch (currentUserRole.ToLower())
            {
                case "admin":
                    // Create user with "hr" role.
                    return await CreateUserWithRoleAsync(input, "HR");
                default:
                    // Handle unknown roles.
                    throw new AbpException("Unknown role");
            }

        }

        [Authorize(Roles = "HR")]
        public async Task<IdentityUserDto> CreateEmployee(ApplicationUserDto input)
        {
            // Create the user with the specified role.
            var user = new IdentityUser(Guid.NewGuid(), input.UserName, input.Email, input.DepartmentId);

            var role = await _roleManager.FindByNameAsync("employee");
            user.SetProperty("DepartmentId", input.DepartmentId);
            user.AddRole(role.Id);

            var result = await _userManager.CreateAsync(user, input.Password);

            if (!result.Succeeded)
            {
                throw new AbpException(result.Errors.First().Description);
            }

            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
        }


        [Authorize(Roles = "HR")]
        public async Task<IdentityUserDto> UpdateEmployee(Guid id, ApplicationUserDto input)
        {

            var user = new IdentityUser(id, input.UserName, input.Email, input.DepartmentId);
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                throw new AbpException(result.Errors.First().Description);
            }

            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
        }

        [Authorize(Roles = "HR")]
        public async Task<string> DeleteEmployee(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            var result = await _userManager.DeleteAsync(user);

            return "Deleted successfully";
        }

        [Authorize(Roles = "HR")]
        public async Task<IdentityUserDto> GetEmployeeById(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            return ObjectMapper.Map<IdentityUser, IdentityUserDto>(user);
        }

        [Authorize(Roles = "HR")]
        public async Task<ListResultDto<IdentityUserDto>> GetEmployees()
        {
            var user = _currentUser;

            var users = await _userManager.GetUsersInRoleAsync("employee");
            var userDtos = ObjectMapper.Map<List<IdentityUser>, List<IdentityUserDto>>((List<IdentityUser>)users);
            return new ListResultDto<IdentityUserDto>(userDtos);
        }



        [Authorize(Roles = "HR")]
        public async Task<ListResultDto<IdentityUserDto>> SearchEmployeesByName(string name)
        {
           
            var users = await _userManager.GetUsersInRoleAsync("HR");
            var filteredUsers = users.Where(u => u.UserName.Contains(name)).ToList();
            var userDtos = ObjectMapper.Map<List<IdentityUser>, List<IdentityUserDto>>(filteredUsers);
            return new ListResultDto<IdentityUserDto>(userDtos);
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


    }
}
