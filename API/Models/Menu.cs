using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("Menu")]
    public partial class Menu
    {
        [Key]
        public long Id { get; set; }
        [StringLength(250)]
        public string Controller { get; set; }
        [StringLength(100)]
        public string Label { get; set; }
        [StringLength(100)]
        public string Icon { get; set; }
        [StringLength(500)]
        public string Url { get; set; }
        [StringLength(500)]
        public string RouterLink { get; set; }
        public bool Visible { get; set; }
        public bool Separator { get; set; }
        [StringLength(50)]
        public string Target { get; set; }
        [StringLength(100)]
        public string Badge { get; set; }
        [StringLength(250)]
        public string Title { get; set; }
        [StringLength(250)]
        public string Class { get; set; }
        public int Seq { get; set; }
        public long? ParentId { get; set; }
    }
}
