
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
using Volo.Abp.Application.Services;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Users;

namespace EMS.Services
{
    public class EmployeeManagementService : IApplicationService, ITransientDependency
    {
        private readonly ICurrentUser _currentUser;
        private readonly IdentityUserManager _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IObjectMapper _objectMapper;
        private readonly UserManager<IdentityUser> _userRepository;
        IIdentityRoleRepository _roleRepository;

        public EmployeeManagementService(
            IdentityUserManager userManager,
            IIdentityRoleRepository roleRepository,
            RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> userRepository,
            ICurrentUser currentUser)
        
        {
            _userManager = userManager;
            _currentUser = currentUser;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _roleManager = roleManager;
        }
        [Authorize(Roles ="admin")]
        public async  Task<string> humanResource(ApplicationUserDto input)
        {
            // Create the user with the specified role.
            var user = new IdentityUser(Guid.NewGuid(), input.UserName, input.Email, input.DepartmentId);
            var role = await _roleManager.FindByNameAsync("HR");


            user.SetProperty("DepartmentId", input.DepartmentId);
            user.AddRole(role.Id);

            var result = await _userManager.CreateAsync(user, input.Password);

            if (!result.Succeeded)
            {
                throw new AbpException(result.Errors.First().Description);
            }

            return "HR Created successully";

        }

        [Authorize(Roles= "admin")]
        public async Task<string> getAllhr()
        {
            // Create the user with the specified role.
       
            var users = await _userRepository.FindByEmailAsync("user@gmail.com");

            return "Fetch hr successfull";

        }
        [Authorize(Roles = "HR")]
        public async Task<IdentityUserDto> CreateEmployee(ApplicationUserDto input)
        {
            //Ensure only add employee in own department


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

            return _objectMapper.Map<IdentityUser,IdentityUserDto>(user);
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

            return _objectMapper.Map<IdentityUser, IdentityUserDto>(user);
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

            return _objectMapper.Map<IdentityUser, IdentityUserDto>(user);
        }

        [Authorize(Roles = "HR")]
        public async Task<ListResultDto<IdentityUserDto>> GetEmployees()
        {
            var user = _currentUser;

            var users = await _userManager.GetUsersInRoleAsync("employee");
            var userDtos = _objectMapper.Map<List<IdentityUser>, List<IdentityUserDto>>((List<IdentityUser>)users);
            return new ListResultDto<IdentityUserDto>(userDtos);
        }



        [Authorize(Roles = "HR")]
        public async Task<ListResultDto<IdentityUserDto>> SearchEmployeesByName(string name)
        {
           
            var users = await _userManager.GetUsersInRoleAsync("HR");
            var filteredUsers = users.Where(u => u.UserName.Contains(name)).ToList();
            var userDtos = _objectMapper.Map<List<IdentityUser>, List<IdentityUserDto>>(filteredUsers);
            return new ListResultDto<IdentityUserDto>(userDtos);
        }
        

    }
}
