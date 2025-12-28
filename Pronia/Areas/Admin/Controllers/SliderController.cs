using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Contexts;

namespace Pronia.Areas.Admin.Controllers;
    [Area("Admin")]

    public class SliderController(AppDBContext _context) : Controller
    {
        public IActionResult Index()
        {
            var serviceItems = _context.Services.ToList();
            return View(serviceItems);
        }
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Service service)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
    [HttpPost]

    public IActionResult Delete(int id)
    {
        var slider = _context.Sliders.Find(id);
        if (slider is null)
        {
            return NotFound();
        }

        _context.Sliders.Remove(slider);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }


}

