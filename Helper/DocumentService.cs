using Ultimate_POS_Api.Repository;
using Ultimate_POS_Api.Repository.Authentication;
using System.Security.Claims;
using Ultimate_POS_Api.Data;
using Ultimate_POS_Api.DTOS;
using Ultimate_POS_Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Ultimate_POS_Api.Helper;
using System.IO;
using System.Drawing;
using DocumentFormat.OpenXml.Packaging;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging; 


namespace Ultimate_POS_Api.Helper
{
   public  class DocumentService
   {
      private readonly UltimateDBContext _dbContext;
      private readonly IDocumentRepository _documentsRepository;
        
      public DocumentService(UltimateDBContext ultimateDB, IDocumentRepository documentsRepository)
      {
           _dbContext = ultimateDB;
           _documentsRepository = documentsRepository;
      }

      public async Task<(byte[] fileData, string contentType)?> GetFilePreviewAsync(Guid id)
      {
         var file = await _documentsRepository.GetFileByIdAsync(id);
         if (file == null) return null;

         if (file.ContentType == "application/pdf")
            return (file.FileData, "application/pdf");

         byte[] pdfBytes = file.ContentType switch
         {
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document" =>
                ConvertWordToPdf(new MemoryStream(file.FileData)),
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" =>
                ConvertExcelToPdf(new MemoryStream(file.FileData)),
            "image/jpeg" or "image/png" =>
                ConvertImageToPdf(new MemoryStream(file.FileData)),
            _ => throw new NotSupportedException("Unsupported file type for preview.")
         };

         return (pdfBytes, "application/pdf");
      } 



//convert word to pdf
private byte[] ConvertWordToPdf(Stream wordStream)
{
    using var mem = new MemoryStream();
    using var wordDoc = WordprocessingDocument.Open(wordStream, false);
    var body = wordDoc.MainDocumentPart?.Document?.Body;

    var text = body?.InnerText ?? "[Empty document]";

    var pdf = QuestPDF.Fluent.Document.Create(container =>
    {
        container.Page(page =>
        {
            page.Margin(40);
            page.Content().Text(text).FontSize(14);
        });
    });

    return pdf.GeneratePdf();
}


//convert image to pdf
private byte[] ConvertImageToPdf(Stream imageStream)
{
    using var image = System.Drawing.Image.FromStream(imageStream);

    // Convert System.Drawing.Image to byte array
    byte[] imageBytes;
    using (var ms = new MemoryStream())
    {
        // Explicitly specify System.Drawing.Imaging.ImageFormat
        image.Save(ms, System.Drawing.Imaging.ImageFormat.Png); // Or System.Drawing.Imaging.ImageFormat.Jpeg, etc.
        imageBytes = ms.ToArray();
    }

    var pdf = QuestPDF.Fluent.Document.Create(container =>
    {
        container.Page(page =>
        {
            page.Margin(20);
            page.Content().Image(imageBytes);
        });
    });

    return pdf.GeneratePdf();
}


//convert excel to pdf
private byte[] ConvertExcelToPdf(Stream excelStream)
{
    using var package = new OfficeOpenXml.ExcelPackage(excelStream);
    var worksheet = package.Workbook.Worksheets.FirstOrDefault();
    if (worksheet == null)
        throw new InvalidOperationException("Excel file has no worksheets");

    return QuestPDF.Fluent.Document.Create(container =>
    {
        container.Page(page =>
        {
            page.Size(PageSizes.A4);
            page.Margin(30);
            page.Content().Table(table =>
            {
                int rows = worksheet.Dimension.End.Row;
                int cols = worksheet.Dimension.End.Column;

                // Define columns
                table.ColumnsDefinition(columns =>
                {
                    for (int i = 0; i < cols; i++)
                        columns.RelativeColumn();
                });

                // Optional: Add header row (assuming first row is header)
                if (rows > 0)
                {
                    table.Header(header =>
                    {
                        for (int col = 1; col <= cols; col++)
                        {
                            string headerText = worksheet.Cells[1, col].Text ?? "";
                            header.Cell().Text(headerText).Bold().AlignCenter();
                        }
                    });
                }

                // Add all rows and cells (starting from row 2 if you used row 1 as header)
                // If no header, start from row 1
                int startRow = (rows > 0) ? 2 : 1; // Start from row 2 if header was added, else from row 1

                for (int row = startRow; row <= rows; row++)
                {
                    // For each row, add its cells
                    for (int col = 1; col <= cols; col++)
                    {
                        string value = worksheet.Cells[row, col].Text ?? "";
                        table.Cell().Text(value); // Simply add the cell content
                    }
                }
            });
        });
    }).GeneratePdf();
}



   }
}