using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebAPISuite.Models
{
    public class Client
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool GoogleAdwordsEnabled { get; set; }
        public string Email { get; set; }
    }

    public class ClientContext : DbContext
    {
        public ClientContext() : base("DefaultConnection")
        {
        }

        public DbSet<Client> Clients { get; set; }
    }
}