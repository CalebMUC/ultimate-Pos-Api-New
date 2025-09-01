using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;

namespace Ultimate_POS_Api.DTOS
{
   public class UploadedFileDto
   {
        public string FileName { get; set; } 
        public string ContentType { get; set; } 
        public byte[] FileData { get; set; } 
   }
}    