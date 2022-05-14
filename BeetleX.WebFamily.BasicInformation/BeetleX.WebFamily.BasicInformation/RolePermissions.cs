using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BeetleX.WebFamily.BasicInformation
{
    [Table("RolePermissions")]
    public class RolePermissions
    {
        public string RoleID { get; set; }

        public string PermissionsID { get; set; }

        public int Value { get; set; }
    }
}
