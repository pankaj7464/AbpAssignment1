using EMS.interfaces;
using EMS.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace EMS.Services
{

    public class EmployeeService : ApplicationService { 
     
        public EmployeeService()
        {
            
        }

        [Authorize(Roles=EMSPermissions.Employee.Create)]
        public async Task<string> CreateAsync(EmployeeDto input)
        {
            // Implementation to create an employee
            return "Employee Created";
        }
    }





}
