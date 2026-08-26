using CommunityEngagementAPI.Models;
using CommunityEngagementAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.OutputCaching;

namespace CommunityEngagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentsController : ControllerBase
    {
        private readonly CEAppContext _db;

        public DocumentsController(CEAppContext db)
        {
            _db = db;
        }
        [HttpGet("getApplicationDocs/{Application_Id}")]
        [OutputCache(Duration = 900)]
        public async Task<IActionResult> getAppicationDocs(int Application_Id)
        {
            var documents = await _db.Document.Where(p => p.Entity_Id == Application_Id ).ToListAsync();

            if (documents.IsNullOrEmpty())
            {
                return NotFound(new { message = "The application documents are not found" });
            }
            else
            {
                List<SendDocumentDto> Docs = new List<SendDocumentDto>();
                foreach (var doc in documents)
                {
                    var document = new SendDocumentDto
                    {
                        Document_Id = doc.Document_Id,
                        Name = doc.Name,
                        Type= doc.Type,
                       // Data = Convert.ToBase64String(doc.Data),
                    };
                    Docs.Add(document);
                }
                return Ok(Docs);
            }
        }

        [HttpGet("document/{id}")]
        [OutputCache(Duration = 900)]
        public async Task<IActionResult> GetDocument(int id)
        {
            var doc = await _db.Document.Where(d => d.Document_Id == id).FirstOrDefaultAsync();

            if (doc == null)
            {
                return NotFound();
            }

            return File(doc.Data, doc.Type, doc.Name);
        }
		
		
		[HttpGet("get-last-4-docs")]
		public async Task<IActionResult> GetLast4Docs()
		{
			var documents = await _db.Document
				.OrderByDescending(doc => doc.Document_Id)
				.Take(4)
				.Select(doc => new SendDocumentDto
				{
					Document_Id = doc.Document_Id,
					Name = doc.Name,
					Type = doc.Type,
					Data = Convert.ToBase64String(doc.Data)
				})
				.ToListAsync();

			if (documents.IsNullOrEmpty())
			{
				return NotFound(new { message = "The application documents are not found" });
			}

			return Ok(documents);
		}
    }
}
