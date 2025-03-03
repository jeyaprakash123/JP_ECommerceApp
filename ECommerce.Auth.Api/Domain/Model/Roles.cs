using System.ComponentModel.DataAnnotations;
using ECommerceApp.Auth.Api.Domain.Models;

namespace ECommerceApp.Auth.Api.Domain.Model
{
    public class Roles
    {
        [Key]
       
        public int RoleId { get; set; }

        [Required]
        public string RoleName { get; set; }
    }
}
