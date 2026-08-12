using CommunityEngagementAPI.Models;
using CommunityEngagementAPI.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CommunityEngagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserProfileController : ControllerBase
    {
        private readonly CEAppContext _db;

        public UserProfileController(CEAppContext db)
        {
            _db = db;
        }

        [HttpPost("profile-picture")]
        public async Task<IActionResult> UploadPic(ProfilePictureDto pictureDto)
        {
            if(pictureDto.ProfilePicture == null || pictureDto.ProfilePicture.Length == 0)
            {
                return BadRequest(new { message = "There is no picture to upload to the database" });
            }
            var fileStream = new MemoryStream();
            await pictureDto.ProfilePicture.CopyToAsync(fileStream);

            var profilePic = new Picture
            {
                Data = fileStream.ToArray(),
                Type = pictureDto.ProfilePicture.ContentType,
                User_Id = pictureDto.User_Id,
            };
            _db.Picture.Add(profilePic);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Uploaded the profile picture to the database" });
        }


        [HttpGet("profile-picture/{User_Id}")]
        public async Task<IActionResult> GetProfilePic (int User_Id)
        {
            var profilePic = await _db.Picture.Where(p => p.User_Id == User_Id).FirstOrDefaultAsync();
           
            if(profilePic == null)
            {
                return NotFound(new {message = "The user have not uploaded the profile picture yet"});
            }
            else
            {
                return File(profilePic.Data,profilePic.Type);
            }
        }

        [HttpPost("change-pic")]
        public async Task<IActionResult> ChangePic([FromForm]ProfilePictureDto pictureDto)
        {
            var profilePic = await _db.Picture.Where(p => p.User_Id == pictureDto.User_Id).FirstOrDefaultAsync();

            if (profilePic == null || pictureDto.ProfilePicture == null || pictureDto.ProfilePicture.Length == 0)
            {
                return BadRequest(new { message = "There is no picture to upload to the database" });
            }

            //push the pic datainto the stream
            var fileStream = new MemoryStream();
            await pictureDto.ProfilePicture.CopyToAsync(fileStream);
            //save new pic details in the db
            profilePic.Data = fileStream.ToArray();
            profilePic.Type = pictureDto.ProfilePicture.ContentType;
            await _db.SaveChangesAsync();

            return Ok(new { message = "Profile picture updated" });
        }
    }
}
