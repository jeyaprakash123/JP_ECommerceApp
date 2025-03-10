﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ECommerceApp.Product.Api.Domain.Model
{
    public class Catagory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        
        public string CatagoryName { get; set; }

        [JsonIgnore]
        public ICollection<Products> Products { get; set; }

    }
}
