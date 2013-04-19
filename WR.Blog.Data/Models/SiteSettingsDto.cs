using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WR.Blog.Data.Models
{
    public class SiteSettingsDto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(100)]
        [DisplayName("Site Title")]
        public string SiteTitle { get; set; }

        [StringLength(500)]
        [DisplayName("Tagline")]
        public string Tagline { get; set; }

        [StringLength(500)]
        [DisplayName("Alternate Tagline")]
        public string AltTagline { get; set; }

        [StringLength(1000)]
        [DisplayName("Menu Links")]
        public string MenuLinks { get; set; }

        [DefaultValue(10)]
        [DisplayName("Blog Posts Per Page")]
        public int PostsPerPage { get; set; }

        [DefaultValue(true)]
        [DisplayName("Allow Comments")]
        public bool AllowComments { get; set; }

        [StringLength(20)]
        [DisplayName("Google Analytics Account Number")]
        public string GoogleAccount { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayName("Date Last Updated")]
        public DateTime LastModifiedDate { get; set; }

        [DisplayName("Last Modified By")]
        public UserProfileDto LastModifiedBy { get; set; }
    }
}