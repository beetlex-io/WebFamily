using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BeetleX.WebFamily.BasicInformation
{
    [Table("Permissions")]
    public class Permissions
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public string Note { get; set; }

        public string Code { get; set; }

        public bool SystemData { get; set; }
    }
}
