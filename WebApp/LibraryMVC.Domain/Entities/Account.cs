using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LibraryMVC.Domain.Models
{
    public class Account
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Wprowadź nickname dla swojego konta")]
        [StringLength(30, MinimumLength = 3)]
        public string NickName { get; set; }
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
