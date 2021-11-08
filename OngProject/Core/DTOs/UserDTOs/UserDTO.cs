using Microsoft.AspNetCore.Http;
using OngProject.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Core.DTOs
{
    public class UserDTO
    {
        [Required(ErrorMessage = "Escriba su nombre")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Escriba su apellido")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Escriba su email")]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Escriba su pasword")]
        [MinLength(5,ErrorMessage = "su paswor debe tener mas de 5 caracteres")]
        public string Password { get; set; }
        public string Photo { get; set; }

        [ForeignKey("Role")]
        public int RoleId { get; set; }
    }
}
