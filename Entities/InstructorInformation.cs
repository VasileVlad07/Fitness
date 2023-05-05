using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fitness.Entities
{
    public partial class InstructorInformation
    {
        public int UserId { get; set; }    
        [Required]
        [StringLength(10, ErrorMessage = "Prenumele nu are minim 2 litere sau este mai lung de 10 caractere!", MinimumLength = 2)]
        public string? LastName { get; set; }
        [Required]
        [StringLength(10, ErrorMessage = "Numele nu are minim 2 litere sau este mai lung de 10 caractere!", MinimumLength = 2)]
        public string? FirstName { get; set; }
        public DateTime? DateCreated { get; set; }
        public int? InstructorId { get; set; }
        public int Hours { get; set; }
    }
}
