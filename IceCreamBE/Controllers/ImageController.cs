using IceCreamBE.Repository.Irepository;
using Microsoft.AspNetCore.Mvc;
using System;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IceCreamBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IRepositoryFileService _IRepositoryFileService; 
        private IWebHostEnvironment environment;

        public ImageController(IRepositoryFileService IRepositoryFileService, IWebHostEnvironment _environment)
        {
            _IRepositoryFileService = IRepositoryFileService;
            environment = _environment;
        }
        [HttpPost]
        public IActionResult Post([FromForm] IFormFile img)
        {
            var avatarURL = _IRepositoryFileService.SaveImage(img, "Images");
            return Ok(avatarURL.Item3);
        }

        [HttpGet("{imgname}")]
        public IActionResult Get(string? imgname)
        {
            var contentPath = this.environment.ContentRootPath;
            return PhysicalFile($@"{contentPath}\Assets\Images\{imgname}", "image/jpeg");
        }
    }
}
