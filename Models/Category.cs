using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ultimate_POS_Api.Models
{
    public class Categories
    {
            [Key]
            public Guid CategoryID { get; set; } 

            [Required]
            [Column(TypeName = "VARCHAR(255)")] 
            public required string CategoryName { get; set; }

            [Required]
            [Column(TypeName = "VARCHAR(255)")] 
            public required string CategoryCode { get; set; }

            [Required]
            [Column(TypeName = "INTEGER")] 
            public int NoOfItems { get; set; }

            [Required]
            [Column(TypeName = "VARCHAR(255)")] 
            public required string CategoryDescription { get; set; }

            [Required]
            [Column(TypeName = "VARCHAR(255)")] 
            public required string Status { get; set; }

            public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
            public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;

            [Column(TypeName = "VARCHAR(255)")] 
            public required string CreatedBy { get; set; }


            [Column(TypeName = "VARCHAR(255)")]
            public string? UpdatedBy { get; set; }

        public  ICollection<Products>? Products { get; set; }
    }

    }

