using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityDemo.Models
{
    public class FileUpload
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string File { get; set; }
    }
}
