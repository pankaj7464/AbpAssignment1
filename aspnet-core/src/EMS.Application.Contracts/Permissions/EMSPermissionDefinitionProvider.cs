using EMS.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace EMS.Permissions;

public class EMSPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var employeeManagementGroup = context.AddGroup("EmployeeManagement");
        var hrPermission = employeeManagementGroup.AddPermission("HR", L("Permission:HR"));
        var adminPermission = employeeManagementGroup.AddPermission("Admin", L("Permission:Admin"));

    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<EMSResource>(name);
    }
}
