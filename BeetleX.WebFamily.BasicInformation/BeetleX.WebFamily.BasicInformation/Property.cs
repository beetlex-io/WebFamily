using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BeetleX.WebFamily.BasicInformation
{
    [Table("properties")]
    public class Property
    {
        public string Category { get; set; }

        [Key]
        public string ID { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }

        public int OrderValue { get; set; }

        public bool SystemData { get; set; }
    }
}
