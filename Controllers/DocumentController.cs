using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Ultimate_POS_Api.Data;
using Ultimate_POS_Api.DTOS;
using Ultimate_POS_Api.Models;
using Ultimate_POS_Api.Repository;
using Ultimate_POS_Api.Helper;


namespace Ultimate_POS_Api.Controllers
{
   [ApiController]
   [Route("api/[controller]")]
   public class DocumentController : ControllerBase
   {
      private readonly UltimateDBContext _dbContext;
      private readonly IDocumentRepository _documentsRepository;
      private readonly DocumentService _documentService;

      public DocumentController(UltimateDBContext ultimateDB, IDocumentRepository documentsRepository, DocumentService documentService)
      {

         _dbContext = ultimateDB;
         _documentsRepository = documentsRepository;
         _documentService = documentService;

      }
      [HttpPost("AddDocument")] 
      [Consumes("multipart/form-data")]
      // public async Task<IActionResult> UploadDocument(UploadedFileDto file) //(IFormFile file)
      public async Task<IActionResult> AddDocument([FromForm] IFormFile file, [FromForm] string fileName)
      {
         try
         {



            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            var document = new UploadedFileDto
            {
               FileName = fileName,
               ContentType = file.ContentType,
               FileData = memoryStream.ToArray()
            };

            var result = await _documentsRepository.UploadAsync(document);
            return Ok(new { fileId = result });
         }
         catch (Exception ex)
         {
            return BadRequest(ex.Message);
         }

      }




      // [HttpGet("preview/{id}")]
      // public async Task<IActionResult> PreviewPdf(int id)
      // {
      //    var file = await _documentsRepository.GetFileByIdAsync(id);
      //    if (file == null)
      //       return NotFound();

      //    if (file.ContentType != "application/pdf")
      //       return BadRequest("Only PDF previews supported in this route.");

      //    return File(file.FileData, "application/pdf");
      // } 




      [HttpGet("DownloadDocument/{id}")]
      public async Task<IActionResult> DownloadDocument(Guid id)
      {
         var result = await _documentsRepository.DownloadFileAsync(id);
         if (result == null)
            return NotFound();

            var fileData = result.Value.FileData;
            var contentType = result.Value.ContentType;
            var fileName = result.Value.FileName;

         return File(fileData, contentType, fileName);
      }

   

      [HttpPost("GetDocuments")]
      [Authorize]
      public async Task<ActionResult> GetDocuments()
      {
         try
         {
            var response = await _documentsRepository.GetDocuments();
            return Ok(response);
         }
         catch (Exception ex)
         {
            return BadRequest(ex.Message);
         }

      } 

         [HttpGet("preview/{id}")]
      public async Task<IActionResult> Preview(Guid id)
      {

         var previewResult  = await _documentService.GetFilePreviewAsync(id);
         if (previewResult == null)
            return NotFound();

             return File(previewResult.Value.fileData, previewResult.Value.contentType); 
      }


   }
}