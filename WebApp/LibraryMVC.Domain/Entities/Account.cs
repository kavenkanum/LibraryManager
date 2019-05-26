using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LibraryMVC.Domain.Models
{
    public class Account
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Wprowadź nickname dla swojego konta")]
        [StringLength(30, MinimumLength = 3)]
        public string NickName { get; set; }

        [Required(ErrorMessage = "Wprowadź hasło")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Powtórz hasło")]
        [NotMapped]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
