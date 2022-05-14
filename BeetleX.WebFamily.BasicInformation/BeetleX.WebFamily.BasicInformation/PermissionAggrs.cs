using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.WebFamily.BasicInformation
{
    public class UserPermissionAggre
    {
        public string Category { get; set; }
        public List<AggrsItem> Items { get; set; } = new List<AggrsItem>();

        public class AggrsItem
        {
            public string Name { get; set; }

            public string Code { get; set; }

            public string PermissionsID { get; set; }

            public bool Value { get; set; }
        }
    }
}
