using GlamBook.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlamBook.Application.Interfaces
{
    public interface IBlogService
    {
        Task<IEnumerable<BlogPostDto>> GetPublishedAsync();
        Task<int> CreateAsync(BlogPostDto dto);
        Task UpdateAsync(BlogPostDto dto);
        Task DeleteAsync(int id);
    }
}
