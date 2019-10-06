using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryMVC.Domain.Entities
{
    public class Account 
    {
        [Key]
        public int ID { get; set; }
        [Required(ErrorMessage = "Wprowadź nazwę konta")]
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }

        [Required(ErrorMessage = "Wprowadź hasło")]
        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "Powtórz hasło")]
        [NotMapped]
        [Compare("PasswordHash")]
        public string ConfirmPassword { get; set; }

        public string Email { get; set; }
        public string Roles { get; set; }
    }
}
