using System.ComponentModel;

namespace Nozomi.Base.Identity.Models.Identity
{
    public enum RoleEnum
    {
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