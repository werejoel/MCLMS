using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MWALIMU_CLASSIC_LMS.Models
{
    
    // LoanOfficer Entity
    public class LoanOfficer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        // Foreign key for ApplicationUser
        public string ApplicationUserId { get; set; }

        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser? ApplicationUser { get; set; }

        // Navigation property
        public virtual List<Loan> ReviewedLoans { get; set; } = new List<Loan>();
    }

}
