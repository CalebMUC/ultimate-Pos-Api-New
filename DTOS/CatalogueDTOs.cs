using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ultimate_POS_Api.DTOS
{
    public class CatalogueDTOs
    {

        public Guid ProductId { get; set; } 
        
        public bool Availability { get; set; }



    }

    public class CatalogueListDTO
    {
       [Required]
       public IList<CatalogueDTOs> Cataloguee { get; set; } = new List<CatalogueDTOs>();
    }
}
