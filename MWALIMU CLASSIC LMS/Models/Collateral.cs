using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MWALIMU_CLASSIC_LMS.Models
{
    // Collateral Entity
    public class Collateral
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Type { get; set; } // Property, Vehicle, Investment, etc.

        [Required]
        [Range(0, double.MaxValue)]
        [Precision(18, 2)]
        public decimal Value { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        // Foreign key for Loan
        public int LoanId { get; set; }

        [ForeignKey("LoanId")]
        public virtual Loan? Loan { get; set; }
    }
}
