﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HPlusSport.API.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Sku { get; set; } = string.Empty;
        [Required]
        public string Name { get; set; } = string.Empty;    //or public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price{ get; set; }
        public bool IsAvailable {  get; set; }

        [Required]
        public int CategoryId{ get; set; }
        
        [JsonIgnore]
        public virtual Category? Category { get; set; }     //nullable

    }
}
