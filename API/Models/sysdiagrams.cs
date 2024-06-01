using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("sysdiagrams")]
    public partial class sysdiagrams
    {
        [Required]
        public string name { get; set; }
        public int principal_id { get; set; }
        [Key]
        public int diagram_id { get; set; }
        public int? version { get; set; }
        public byte[] definition { get; set; }
    }
}
