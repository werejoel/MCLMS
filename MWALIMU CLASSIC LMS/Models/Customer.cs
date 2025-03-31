using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MWALIMU_CLASSIC_LMS.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(20)]
        [Phone]
        public string ContactNo { get; set; }

       
        [StringLength(100)]
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        [StringLength(200)]
        public string WorkPlace { get; set; }

        // Foreign key for ApplicationUser
        public string ApplicationUserId { get; set; }

        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser? ApplicationUser { get; set; }

        // Navigation properties
        public virtual List<Account> Accounts { get; set; } = new List<Account>();
        public virtual List<Loan> Loans { get; set; } = new List<Loan>();
        public virtual CreditScore? CreditScore { get; set; }
    }
}
