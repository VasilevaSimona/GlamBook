using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlamBook.Application.DTOs
{
    public class BlogPostDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string Content { get; set; } = default!;
        public string? CoverUrl { get; set; }
        public DateTime PublishedDate { get; set; }
        public bool isPublished { get; set; }

    }

}
