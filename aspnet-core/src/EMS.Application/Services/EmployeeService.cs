using EMS.interfaces;
using EMS.Permissions;
using EMS.Services.Employee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace EMS.Services
{

    public class EmployeeService : ApplicationService, IEmployeeService
    {
        private readonly IdentityUserManager _userManager;
        private readonly IAuthorizationService _authorizationService;
        private readonly ICurrentUser _currentUser;
        private readonly IIdentityRoleRepository _roleRepository;

        public EmployeeService(
            IdentityUserManager userManager,
        IAuthorizationService authorizationService,
        IIdentityRoleRepository identityRoleRepository,
        ICurrentUser currentUser
            )
        {
            _userManager = userManager;
            _authorizationService = authorizationService;
            _currentUser = currentUser;
            _roleRepository = identityRoleRepository;

        }

        [Authorize(Roles = "admin")]
        public async Task<string> AddHr()
        {
            return "Hr added";
        }

        [Authorize(Roles = EMSPermissions.Hr.Default)]
        public async Task<string> AddEmployeeAsync(EmployeeDto employeeDto)
        {
            // Check if the current user is an HR
            return "Employee added";

        }


    }





}
