using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ECommerceApp.Auth.Api.Domain.Model;

namespace ECommerceApp.Auth.Api.Domain.Models
{
    public class Login
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [StringLength(200)]
        public string Password { get; set; }

        [Required]
        [Range(18,60,ErrorMessage ="Age must be greater than 18")]
        public int Age { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public int Pincode { get; set; }
       
        [ForeignKey("Roles")]
        public int RoleId { get; set; }
        public virtual Roles Roles { get; set; }
    }
}
