using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BeetleX.WebFamily.BasicInformation
{

    [Table("Roles")]
    public class Role
    {
        [Key]
        public string ID { get; set; }

        public string Note { get; set; }

        public string Name { get; set; }

        public bool SystemData { get; set; }

    }
}
