using EMS.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace EMS.Permissions;

public class EMSPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        //Define your own permissions here. Example:
        //myGroup.AddPermission(EMSPermissions.MyPermission1, L("Permission:MyPermission1"));

        var EmsPermissions = context.AddGroup(EMSPermissions.GroupName, L("Permission:EMS"));



        var usersPermission = EmsPermissions.AddPermission(EMSPermissions.Hr.Default, L("Permission:Hr"));
        usersPermission.AddChild(EMSPermissions.Hr.Create, L("Permission:Hr.Create"));
        usersPermission.AddChild(EMSPermissions.Hr.Edit, L("Permission:Hr.Edit"));
        usersPermission.AddChild(EMSPermissions.Hr.Delete, L("Permission:Hr.Delete"));

        var employeePermission = EmsPermissions.AddPermission(EMSPermissions.Employee.Default, L("Permission:Employee"));
        employeePermission.AddChild(EMSPermissions.Employee.Create, L("Permission:Employee.Create"));
        employeePermission.AddChild(EMSPermissions.Employee.Edit, L("Permission:Employee.Edit"));
        employeePermission.AddChild(EMSPermissions.Employee.Delete, L("Permission:Employee.Delete"));


    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<EMSResource>(name);
    }
}
