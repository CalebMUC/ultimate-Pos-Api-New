using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ultimate_POS_Api.Models
{
  public class Catalogue
  {
    [Key]
    public Guid SKU { get; set; }

    [Required]
    public Guid ProductId { get; set; }

    [ForeignKey(nameof(ProductId))]
    public Products Products { get; set; }

    [Required]
    public bool Availability { get; set; }
  }
}



