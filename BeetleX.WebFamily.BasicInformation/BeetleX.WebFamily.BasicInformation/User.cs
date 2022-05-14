using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BeetleX.WebFamily.BasicInformation
{
    [Table("users")]
    public class User
    {
        [Key]
        public string ID { get; set; }

        public string Name { get; set; }

        public string Sex { get; set; }

        public string WorkNumber { get; set; }

        public string JobPostion { get; set; }

        public string DepartmentID { get; set; }

        public string EMail { get; set; }

        public string Note { get; set; }

        public string TelePhone { get; set; }

        public string Address { get; set; }

        public string MobilePhone { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime BirthDay { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime EntryDay { get; set; }

        public string SuperiorID { get; set; }

        public string LoginPassword { get; set; }

        //int 0,1
        public bool Enabled { get; set; }

        public string Icon { get; set; }
        //int 0,1
        public bool SystemData { get; set; }

        public string WeiXinID { get; set; }

        public string Roles { get; set; }

    }
}
