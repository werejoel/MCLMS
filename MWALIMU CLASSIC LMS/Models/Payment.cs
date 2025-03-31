using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MWALIMU_CLASSIC_LMS.Models
{

    // Payment Entity
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime PaymentDate { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        [Precision(18, 2)]
        public decimal Amount { get; set; }

        [StringLength(100)]
        public string PaymentMethod { get; set; }

        [StringLength(100)]
        public string TransactionId { get; set; }

        // Foreign key for Loan
        public int LoanId { get; set; }

        [ForeignKey("LoanId")]
        public virtual Loan? Loan { get; set; }
    }

}
