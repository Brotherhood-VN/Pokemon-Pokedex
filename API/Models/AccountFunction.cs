using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("AccountFunction")]
    public partial class AccountFunction
    {
        [Key]
        public long Id { get; set; }
        public long AccountId { get; set; }
        public long FunctionId { get; set; }
        [ForeignKey("FunctionId")]
        [InverseProperty("AccountFunctions")]
        public virtual Function Function { get; set; }
        [ForeignKey("AccountId")]
        [InverseProperty("AccountFunctions")]
        public virtual Account Account { get; set; }
    }
}
