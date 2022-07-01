using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StarboardNFT.Models
{
    public class Collection: AuditFields
    {
        public string DisplayName { get; set; }
        public string Symbol { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
        public string FileType { get; set; }

        [Required]
        [ForeignKey("UserProfile")]
        public Guid UserProfileId { get; set; }
        public virtual UserProfile UserProfile { get; set; }
    }
}
