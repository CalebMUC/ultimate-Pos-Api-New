using System;
using System.Collections.Generic; // Import for IList 
using System.ComponentModel.DataAnnotations;

namespace Ultimate_POS_Api.DTOS
{
    public class CategoryDTOs
    {
        [Required]
        public string CategoryName { get; set; } = string.Empty;

        [Required]
        public string CategoryCode { get; set; } = string.Empty;

        [Required]
        public int NoOfItems { get; set; } 

        [Required]
        public string CategoryDescription { get; set; } = string.Empty;

        [Required]
        public string Status { get; set; } = string.Empty;

       
    }

    public class EditCategoryDTO
    {
        public Guid CategoryID { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public string CategoryCode { get; set; } = string.Empty;

        public int NoOfItems { get; set; }

        public string CategoryDescription { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;


    }



    //public class CategoryListDto
    //{
    //    [Required]
    //    public IList<CategoryDTOs> Categ { get; set; } = new List<CategoryDTOs>();
    //}
}
