using CommunityEngagementAPI.Models;
using CommunityEngagementAPI.Models.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CommunityEngagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SummaryPhotosController : ControllerBase
    {

        private readonly CEAppContext _db;
        public SummaryPhotosController(CEAppContext db)
        {
            _db = db;
        }


        [HttpGet("getSummaryPhotos/{Summary_Id}")]
        public async Task<IActionResult> GetSummaryPhotos(int Summary_Id)
        {
            var photos = await _db.Photo.Where(p => p.Summary_Id == Summary_Id).ToListAsync();

            if (photos.IsNullOrEmpty())
            {
                return  NotFound(new { message = "The summary photos not found" });
            }
            else
            {
                List<SendPhotoDto> Docs = new List<SendPhotoDto>();

                foreach (var doc in photos)
                {
                    var photo = new SendPhotoDto
                    {
                        Photo_Id = doc.Photo_Id,
                        Type = doc.Type,
                    
                    };
                    Docs.Add(photo);
                }
                return Ok(Docs);
            }
        }

        [HttpGet("photo/{photo_id}")]
        public async Task<IActionResult> GetPhoto(int photo_id)
        {
            var photo = await _db.Photo.Where(d => d.Photo_Id == photo_id).FirstOrDefaultAsync();

            if (photo == null)
            {
                return NotFound(new { message = " Photo not found in this summary." });
            }

            return File(photo.Data, photo.Type);
        }
    }
}
