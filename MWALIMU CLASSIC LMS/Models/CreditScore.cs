using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MWALIMU_CLASSIC_LMS.Models
{
    // CreditScore Entity
    public class CreditScore
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(300, 850)]
        public int? Score { get; set; }

        [Required]
        [StringLength(20)]
        public string? Rating { get; set; } // Poor, Fair, Good, Very Good, Excellent

        [DataType(DataType.Date)]
        public DateTime LastUpdated { get; set; } = DateTime.Now;

        // Foreign key for Customer
        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }
    }

}
