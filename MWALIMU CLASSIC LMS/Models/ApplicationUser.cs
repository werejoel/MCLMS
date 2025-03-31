
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    


namespace MWALIMU_CLASSIC_LMS.Models
{

    // ApplicationUser
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string District { get; set; }

        // Navigation properties
        public virtual Customer Customer { get; set; }
        public virtual LoanOfficer LoanOfficer { get; set; }
    }
}
