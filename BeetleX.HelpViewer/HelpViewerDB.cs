using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeetleX.HelpViewer
{
    public class HelpViewerDB : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(@"Filename=helpviewer.db");
            }
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Folder> Folders { get; set; }

        public DbSet<Document> Documents { get; set; }


    }
    [Table("Documents")]
    public class Document
    {
        [Key]
        public string ID { get; set; }

        public string Title { get; set; }

        public string Summary { get; set; }

        public string Content { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime CreateTime { get; set; }

        public string Tag { get; set; }

        public string Folder { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool Enabled { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int OrderValue { get; set; }


    }

    [Table("Folders")]
    public class Folder
    {
        [Key]
        public string ID { get; set; }

        public string Name { get; set; }

        public string Note { get; set; }
    }
    [Table("Users")]
    public class User
    {
        [Key]
        public string ID { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Password { get; set; }

        public bool Enabled { get; set; }

        public bool IsAdmin { get; set; }
    }
}
