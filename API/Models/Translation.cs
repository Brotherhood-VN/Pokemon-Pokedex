using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("Translation")]
    public partial class Translation
    {
        [Key]
        public long Id { get; set; }
        [Required]
        [StringLength(250)]
        public string FromTable { get; set; }
        [Required]
        [StringLength(10)]
        public string Language { get; set; }
        [Required]
        [StringLength(250)]
        public string Key { get; set; }
        [Required]
        public string Value { get; set; }
        public bool? Status { get; set; }
        public bool? IsDelete { get; set; }
        public long CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; }
        public long? UpdateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdateTime { get; set; }
    }
}
