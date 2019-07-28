using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IdentityDemo.Data;
using IdentityDemo.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityDemo.Controllers
{
    public class FileUploadController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IHostingEnvironment _hostingEnvironment;
        public FileUploadController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var model = await _context.FileUploads.ToListAsync();
            return View(model);
        }

        public IActionResult Create()
        {
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(FileUploadViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                // If the Photo property on the incoming model object is not null, then the user
                // has selected an image to upload.
                if (model.File != null)
                {
                    // The image must be uploaded to the images folder in wwwroot
                    // To get the path of the wwwroot folder we are using the inject
                    // HostingEnvironment service provided by ASP.NET Core
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                    // To make sure the file name is unique we are appending a new
                    // GUID value and and an underscore to the file name
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.File.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    // Use CopyTo() method provided by IFormFile interface to
                    // copy the file to wwwroot/images folder
                    model.File.CopyTo(new FileStream(filePath, FileMode.Create));
                }
                FileUpload newFileUpload = new FileUpload
                {
                    Name = model.Name,
                    File = uniqueFileName
                };
                _context.FileUploads.Add(newFileUpload);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
       
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();    
            }
            var file = await _context.FileUploads.Where(f => f.Id == id).FirstOrDefaultAsync();

            return View(file);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var file = await _context.FileUploads.Where(f => f.Id == id).FirstOrDefaultAsync();

            return View(file);
        }

        [HttpPost]
        public async Task<IActionResult> Edit( FileUploadViewModel model, int? id, string oldFile)
        {
            if(id == null)
            {
                return NotFound();
            }
            var updateColumn = await _context.FileUploads.Where(f => f.Id == id).SingleOrDefaultAsync();
           //update with old file
            if (updateColumn != null) {
                if (model.File == null)
                {
                    updateColumn.Name = model.Name;
                    _context.Update(updateColumn);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                //update with new file
                string uniqueFileName = null;
                if (model.File != null)
                {
                    //remove old file
                    string webRootPath = _hostingEnvironment.WebRootPath;
                    var oldFileName = oldFile;
                    var oldPath = webRootPath + "/images/" + oldFileName;

                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);

                    }

                    //update new file

                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.File.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    model.File.CopyTo(new FileStream(filePath, FileMode.Create));

                    updateColumn.Name = model.Name;
                    updateColumn.File = uniqueFileName;
                    _context.FileUploads.Update(updateColumn);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }              
            }
            return RedirectToAction(nameof(Index));
        }




        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var result = await _context.FileUploads.FirstOrDefaultAsync(f => f.Id == id);
            var fileName = result.File;
            if (result == null)
            {
                return NotFound();
            }
            if (result != null)
            {
                var fullPath = _hostingEnvironment.WebRootPath + "/images/" + fileName;
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
                _context.FileUploads.Remove(result);
               await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }





    }
}