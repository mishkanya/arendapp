using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArendApp.Api.Services;
using ArendApp.Models;
using System.IO;
using Microsoft.Extensions.Hosting;
using ArendApp.Api.Extensions;

namespace ArendApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private IWebHostEnvironment _environment;
        public const string ImageFolderName = "Images";

        private string RequestUrl { get => $"{this.GetServerDomen()}{ImageFolderName}/"; }
        private IEnumerable<string> _allowedExtension = new string[] { ".jpg", ".jpeg", ".png" };

        public ImagesController(IWebHostEnvironment environment)
        {
            this._environment = environment;
        }

        // GET: api/Images
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> GetImages()
        {
            string wwwPath = _environment.WebRootPath;
            string contentPath = this._environment.ContentRootPath;

            string path = Path.Combine(_environment.WebRootPath, ImageFolderName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var images = Directory.EnumerateFiles(path);

            if (images.Any())
            {
                var requestUrl = RequestUrl;
                var response = images.Select(t => new FileInfo(t).Name).Select(t => requestUrl + t);
                return Ok(response);
            }
            else
                return NotFound();
        }

        // POST: api/Images
        [HttpPost]
        [HeaderValidator(true)]
        public async Task<string> PostImage(IFormFileCollection images)
        {
            string wwwPath = _environment.WebRootPath;
            string contentPath = _environment.ContentRootPath;

            string path = Path.Combine(_environment.WebRootPath, ImageFolderName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            List<string> uploadedFiles = new List<string>();
            foreach (var image in images)
            {
                var fileExtension = Path.GetExtension(image.FileName);
                if (_allowedExtension.Contains(fileExtension) == false)
                    continue;

                string fileName = Path.GetFileName(image.FileName);
                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    image.CopyTo(stream);
                    uploadedFiles.Add(fileName);
                }

            }
            return string.Join(", ", uploadedFiles);

        }
        // DELETE: api/Images/id : string
        [HttpDelete("{id}")]
        [HeaderValidator(true)]
        public async Task<IActionResult> DeleteImage(string id)
        {
            var findName = id.Replace("%2F", "/");
            string wwwPath = _environment.WebRootPath;
            string contentPath = _environment.ContentRootPath;

            string path = Path.Combine(_environment.WebRootPath, ImageFolderName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var files = Directory.EnumerateFiles(path);
            var images = files.FirstOrDefault(t => t.Contains(findName));

            System.IO.File.Delete(images);

            if (images != null)
                return Ok();
            else
                return NotFound();
        }
    }
}
