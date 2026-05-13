using GlamBook.Domain.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlamBook.Domain.Entities
{
    public class BlogPost : BaseEntity
    {
        public string Title { get; set; } = default!;
        public string Content { get; set; } = default!;
        public string? CoverUrl { get; set; }
        public DateTime PublishedAt { get; set; }
        public bool IsPublished { get; set; } = true;
    }
}
