using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityDemo.Models
{
    public class FileUploadViewModel
    {
        public string Name { get; set; }
        public IFormFile File { get; set; }
    }
}
