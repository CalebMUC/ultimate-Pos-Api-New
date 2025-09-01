using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Ultimate_POS_Api.Data;
using Ultimate_POS_Api.DTOS;
using Ultimate_POS_Api.Models;

namespace Ultimate_POS_Api.Repository
{
   public class DocumentsRepository : IDocumentRepository
   {  

       private readonly UltimateDBContext _dbContext;

      public DocumentsRepository(UltimateDBContext ultimateDB)
      {
         _dbContext = ultimateDB;

      }



   public async Task<ResponseStatus> UploadAsync(UploadedFileDto file)  {   

         var uploadedFile = new UploadedFiles  {
            
            FileName = file.FileName,
            ContentType = file.ContentType,
            FileData = file.FileData,  
            CreatedOn =  DateTime.UtcNow
         };

         _dbContext.Documents.Add(uploadedFile);
         await _dbContext.SaveChangesAsync();

          return new ResponseStatus
            {
               Status = 200,
               StatusMessage = "Document Added Successfully"
            };
      }



   public async Task<UploadedFiles?> GetFileByIdAsync(Guid id) {

         return await _dbContext.Documents.FindAsync(id);
      }


   public async Task<(byte[] FileData, string ContentType, string FileName)?> DownloadFileAsync(Guid id) {

         var file = await _dbContext.Documents.FindAsync(id);
         if (file == null)
            return null;
         return (file.FileData, file.ContentType, file.FileName);
      }




   public async Task<IEnumerable<UploadedFiles>> GetDocuments() {

         var response = await _dbContext.Documents.ToListAsync();
         return response;
      } 
      


   }
}