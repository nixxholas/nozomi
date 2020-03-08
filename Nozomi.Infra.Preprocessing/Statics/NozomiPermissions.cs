using Nozomi.Base.Auth.Models;

namespace Nozomi.Preprocessing.Statics
{
    public static class NozomiPermissions
    {
        public const string AllowHigherStaffRoles = "Administrator, Owner";
        
        public const string AllowAllStaffRoles = "Staff, Administrator, Owner";

        public static readonly RoleEnum[] AllStaffRoles = new RoleEnum[]
        {
            RoleEnum.Administrator, RoleEnum.Owner, RoleEnum.Staff
        };
    }
}