using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ArendApp.Api.Services;
using ArendApp.Models;
using ArendApp.Api.Extensions;

namespace ArendApp.Api.Controllers
{
    public class ImagesController : Controller
    {
        private IWebHostEnvironment _environment;
        public const string ImageFolderName = "Images";

        private string RequestUrl { get => $"{this.GetServerDomen()}{ImageFolderName}/"; }
        private IEnumerable<string> _allowedExtension = new string[] { ".jpg", ".jpeg", ".png" };

        public ImagesController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        // GET: Images
        public async Task<IActionResult> Index()
        {
            string wwwPath = _environment.WebRootPath;
            string contentPath = _environment.ContentRootPath;

            string path = Path.Combine(_environment.WebRootPath, ImageFolderName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var images = Directory.EnumerateFiles(path);

            if (images.Any())
            {
                var response = images.Select(t => new FileInfo(t).Name).Select(t => $"/{ImageFolderName}/{t}");
                return View(response);
            }
            else
                return NotFound();
            //return View(await _context.Images.ToListAsync());
        }

        //GET: Images/Details/5
        //[Route("[controller]/Details/{id}")]
        public async Task<IActionResult> Details(string id)
        {
            id = id.Replace("%2F", "/");
            var findName = id.Split("/").Last();
            string wwwPath = _environment.WebRootPath;
            string contentPath = _environment.ContentRootPath;

            string path = Path.Combine(_environment.WebRootPath, "Images");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var files = Directory.EnumerateFiles(path);
            var images = files.FirstOrDefault(t => t.Contains(findName));
            if (images != null)
                return View(model: id);
            else return NotFound();
        }

        // GET: Images/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Images/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IEnumerable<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                string wwwPath = _environment.WebRootPath;
                string contentPath = this._environment.ContentRootPath;

                string path = Path.Combine(this._environment.WebRootPath, "Images");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                List<string> uploadedFiles = new List<string>();
                foreach (IFormFile postedFile in files)
                {
                    string fileName = Path.GetFileName(postedFile.FileName);
                    using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                    {
                        postedFile.CopyTo(stream);
                        uploadedFiles.Add(fileName);
                        ViewBag.Message += string.Format("<b>{0}</b> uploaded.<br />", fileName);
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(files.Select(t => t.Name));
        }

        // GET: Images/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            id = id.Replace("%2F", "/");
            var findName = id.Split("/").Last();
            string wwwPath = _environment.WebRootPath;
            string contentPath = _environment.ContentRootPath;

            string path = Path.Combine(_environment.WebRootPath, "Images");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var files = Directory.EnumerateFiles(path);
            var images = files.FirstOrDefault(t => t.Contains(findName));
            if (images != null)
                return View(model: id);
            else return NotFound();
        }

        // POST: Images/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {

            id = id.Replace("%2F", "/");
            var findName = id.Split("/").Last();
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

            return RedirectToAction(nameof(Index));
        }

        //private bool ImageExists(int id)
        //{
        //    return _context.Images.Any(e => e.Id == id);
        //}
    }
}
