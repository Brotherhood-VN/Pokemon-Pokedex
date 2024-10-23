using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("RefreshToken")]
    public partial class RefreshToken
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string Token { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? Expires { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedTime { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? RevokedTime { get; set; }
        public string ReplacedByToken { get; set; }
        public string ReasonRevoked { get; set; }
        public long AccountId { get; set; }

        public bool IsExpired => DateTime.Now >= Expires;
        [NotMapped]
        public bool IsRevoked => RevokedTime != null;
        [NotMapped]
        public bool IsActive => !IsRevoked && !IsExpired;
    }
}
