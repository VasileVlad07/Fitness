using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fitness.Entities
{
    public partial class ClientInformation
    {
        public int ClientId { get; set; }
        [Required(ErrorMessage = "Nu ati introdus nici un nume!")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "Nu ati introdus nici un prenume!")]
        public string? LastName { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Adresa de email nu este corecta!")]
        public string? Email { get; set; }
        [Required]
        [StringLength(10), MinLength(10)]
        [Phone(ErrorMessage ="Numarul de telefon introdus nu este corect!")] 
        public string? PhoneNumber { get; set; }
        public int Hours { get; set; }
        public int InstructorID { get; set; }
        public string? FullName
        {
            get { return LastName + " " + FirstName; }
        }


        [Required]
        public string CategoryString
        {
            get { return this.Categories.ToString(); }
            set { Categories = (Categories)Enum.Parse(typeof(Categories), value, true); } 
        }

        public Categories Categories { get; set; }

    }

    public enum Categories
    {
        Adult = 0,
        Elev = 1,
        Student = 2
    }

}
