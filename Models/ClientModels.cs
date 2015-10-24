using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAPISuite.Models
{
    public class Client
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Town { get; set; }
        public string County { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }

        public virtual IEnumerable<ClientSetting> ClientSettings { get; set; }

        public Client()
        {
            this.ClientSettings = new List<ClientSetting>();
        }
    }

    public class ClientSetting
    {
        [ForeignKey("Client")]
        public Guid ClientId {get; set;}

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public string Name { get; set; }

        [Required]
        public string SubjectLine { get; set; }

        [DefaultValue(true)]
        public bool AutoReplyToCustomer { get; set; }

        public string AutoReplySubjectLine { get; set; }

        [AllowHtml]
        public string AutoReplyBody { get; set; }

        [DefaultValue(false)]
        public bool EnableFileUpload { get; set; }

        [DefaultValue(false)]
        public bool GoogleAdwordsEnabled { get; set; }
        public Int64? GoogleConversionId { get; set; }
        public string GoogleConversionLanguage { get; set; }
        public string GoogleConversionFormat { get; set; }
        public string GoogleConversionColour { get; set; }
        public string GoogleConversionLabel { get; set; }
        public string GoogleConversionValue { get; set; }
        public string GoogleConversionCurrency { get; set; }

        [DefaultValue(false)]
        public bool GoogleRemarketingOnly { get; set; }

        public virtual Client Client { get; set; }
    }

    public class ClientContext : DbContext
    {
        public ClientContext() : base("DefaultConnection")
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<ClientSetting> ClientSettings { get; set; }
    }
}