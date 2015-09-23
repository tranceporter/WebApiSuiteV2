using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPISuite.Models
{
    public class UploadedFile
    {
        public string FileName { get; set; }
        public byte[] FileBytes { get; set; }
        public string ContentType { get; set; }
    }
}