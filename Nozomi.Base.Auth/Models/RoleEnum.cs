using System.ComponentModel;

namespace Nozomi.Base.Auth.Models
{
    public enum RoleEnum
    {
        [Description("User")]
        User,
        [Description("CorporateUser")]
        CorporateUser,
        [Description("Staff")]
        Staff,
        [Description("Administrator")]
        Administrator,
        [Description("Owner")]
        Owner
    }
}