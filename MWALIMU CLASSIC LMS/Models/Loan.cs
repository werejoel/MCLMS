using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MWALIMU_CLASSIC_LMS.Models
{
    // Loan Entity
    public class Loan
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        [Precision(18, 2)]
        public decimal LoanAmount { get; set; }

        [Required]
        [Range(0, 100)]
        [Precision(5, 2)]
        public decimal InterestRate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

    
        [StringLength(20)]
        [DefaultValue("Pending")]
        public string? Status { get; set; } = "Pending";


        // Foreign key for Customer
        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }

        // Foreign key for LoanOfficer
        public int? LoanOfficerId { get; set; }

        [ForeignKey("LoanOfficerId")]
        public virtual LoanOfficer? LoanOfficer { get; set; }

        // Navigation properties
        public virtual List<Payment> Payments { get; set; } = new List<Payment>();
        public virtual Collateral? Collateral { get; set; }
    }

}
