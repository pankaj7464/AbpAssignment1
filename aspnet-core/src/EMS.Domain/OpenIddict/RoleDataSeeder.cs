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

            //If Admin not exist Seed Admin role and permission
            var adminRole = await _identityRoleManager.FindByNameAsync("Admin");
            if (adminRole == null)
            {
                await _identityRoleManager.CreateAsync(new IdentityRole(Guid.NewGuid(), "Admin"));
            }
            adminRole = await _identityRoleManager.FindByNameAsync("Admin");

            await _permissionManager.SetForRoleAsync(adminRole.Name, EMSPermissions.Admin.Create, true);
            await _permissionManager.SetForRoleAsync(adminRole.Name, EMSPermissions.Admin.Edit, true);
            await _permissionManager.SetForRoleAsync(adminRole.Name, EMSPermissions.Admin.Delete, true);
            await _permissionManager.SetForRoleAsync(adminRole.Name, EMSPermissions.Admin.View, true);


            //If HR not exist Seed Hr role and permission
            var hrRole = await _identityRoleManager.FindByNameAsync("HR");
            if (hrRole == null)
            {
                await _identityRoleManager.CreateAsync(new IdentityRole(Guid.NewGuid(), "HR"));
            }

            hrRole = await _identityRoleManager.FindByNameAsync("HR");

            await _permissionManager.SetForRoleAsync(hrRole.Name, EMSPermissions.Hr.Create, true);
            await _permissionManager.SetForRoleAsync(hrRole.Name, EMSPermissions.Hr.Edit, true);
            await _permissionManager.SetForRoleAsync(hrRole.Name, EMSPermissions.Hr.Delete, true);
            await _permissionManager.SetForRoleAsync(hrRole.Name, EMSPermissions.Hr.View, true);
        }
    }
}
