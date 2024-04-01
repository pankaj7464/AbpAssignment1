namespace EMS.Permissions;

public static class EMSPermissions
{
    // This is a provider name.
    public const string GroupName = "EMS";

    //Add your own permission names. Example:
    //public const string MyPermission1 = GroupName + ".MyPermission1";

    //public const string HrPermission = GroupName + ".HR";


    public static class Admin
    {
        public const string Default = GroupName + ".Admin";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
        public const string View = Default + ".View";
    }
    public static class Hr
    {
        public const string Default = GroupName + ".HR";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
        public const string View = Default + ".View";
    }

    public static class Employee
    {
        public const string Default = GroupName + ".Employee";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
        public const string View = Default + ".View";
    }

}
