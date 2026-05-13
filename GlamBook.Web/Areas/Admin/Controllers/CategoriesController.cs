using GlamBook.Application.DTOs;
using GlamBook.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GlamBook.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categories;

        public CategoriesController(ICategoryService categories)
        {
            _categories = categories;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _categories.GetAllAsync();
            return View(list);
        }

        public IActionResult Create() => View(new CategoryDto());

        [HttpPost]
        public async Task<IActionResult> Create(CategoryDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _categories.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var dto = (await _categories.GetAllAsync()).FirstOrDefault(x => x.Id == id);
            if (dto == null) return NotFound();
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryDto model)
        {
            await _categories.UpdateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _categories.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
