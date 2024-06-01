using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("Menu")]
    public partial class Menu
    {
        [Key]
        public long Id { get; set; }
        public string Controller { get; set; }
        public string Label { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public string RouterLink { get; set; }
        public bool Visible { get; set; }
        public bool Separator { get; set; }
        public string Target { get; set; }
        public string Badge { get; set; }
        public string Title { get; set; }
        public string Class { get; set; }
        public int Seq { get; set; }
        public long? ParentId { get; set; }
    }
}
