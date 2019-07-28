using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace IdentityDemo.Data
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [Display(Name ="Full Name")]
        public string FullName { get; set; }
    }
}
