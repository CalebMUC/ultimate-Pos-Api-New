using Microsoft.AspNetCore.Mvc;
using Ultimate_POS_Api.DTOS;
using Ultimate_POS_Api.Models;

namespace Ultimate_POS_Api.Repository
{
   public interface IDocumentRepository
   {
      public Task<ResponseStatus> UploadAsync(UploadedFileDto file);

      public Task<UploadedFiles?> GetFileByIdAsync(Guid id);

      public Task<(byte[] FileData, string ContentType, string FileName)?> DownloadFileAsync(Guid id);
      
      public Task<IEnumerable<UploadedFiles>> GetDocuments();

   }
}





 