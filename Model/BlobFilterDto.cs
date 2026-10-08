using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class BlobFilterDto
    {
        public string FileName { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public long? FileSize { get; set; } 
        public DateTimeOffset? LastModified { get; set; } 
    }
}
