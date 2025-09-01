using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ultimate_POS_Api.Models {

    public class UploadedFiles
    {
        [Key]
        public Guid DocumentId { get; set; }
        [Required]
        [Column(TypeName = "VARCHAR(255)")]
        public string FileName { get; set; } = string.Empty;
        [Required]
        [Column(TypeName = "VARCHAR(255)")]
        public string ContentType { get; set; }= string.Empty;
        public byte[] FileData { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
         [Required]
        [Column(TypeName = "VARCHAR(255)")]
        public string CreatedBy { get; set; } = string.Empty;
    }
}



        // public string FileName { get; set; } 
        // public string ContentType { get; set; } 
        // public byte[] FileData { get; set; } 