using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LibraryMVC.Domain.Entities
{
    public class UserRole 
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int RoleID { get; set; }
    }
}
