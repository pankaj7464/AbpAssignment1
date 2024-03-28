using EMS.Permissions;
using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;

namespace EMS.OpenIddict
{
    public class RoleDataSeeder : IDataSeedContributor, ITransientDependency
    {
        private readonly IdentityRoleManager _identityRoleManager;
        private readonly IPermissionManager _permissionManager;
        public RoleDataSeeder(IdentityRoleManager identityRoleManager, IPermissionManager permissionGrantManager)
        {
            _identityRoleManager = identityRoleManager;
            _permissionManager = permissionGrantManager;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            var hrRole = await _identityRoleManager.FindByNameAsync("HR");
            if (hrRole == null)
            {
                await _identityRoleManager.CreateAsync(new IdentityRole(Guid.NewGuid(), "HR"));
            }

            await _permissionManager.SetForRoleAsync(hrRole.Name, EMSPermissions.Employee.Create, true);
            await _permissionManager.SetForRoleAsync(hrRole.Name, EMSPermissions.Employee.Edit, true);
            await _permissionManager.SetForRoleAsync(hrRole.Name, EMSPermissions.Employee.Delete, true);
            await _permissionManager.SetForRoleAsync(hrRole.Name, EMSPermissions.Employee.View, true);

        }
    }
}
