using Microsoft.AspNetCore.Mvc;
using Pronia.Contexts;
using Pronia.Models;

namespace Pronia.Areas.Admin.Controllers;
[Area("Admin")]

public class ServiceController : Controller
{
    private readonly AppDBContext _context;
    private readonly IWebHostEnvironment _environment;



    public ServiceController(AppDBContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

    public IActionResult Index()
    {
        var shippingItems = _context.Services.ToList();
        return View(shippingItems);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Create(Models.Service service)
    {
        if (!ModelState.IsValid)
        {
            return View(service);
        }

        if(service.Image.ContentType.Contains("image") == false)
        {
            ModelState.AddModelError("Image", "Please select image file");
            return View(service);
        }

        if (service.Image.Length > 2 * 1024 * 1024)
        { 
            ModelState.AddModelError("Image", "Image size must be less than 2MB");
            return View(service);
        }

        string uniqueFileName = Guid.NewGuid().ToString() + service.Image.FileName;
        string folderPath = Path.Combine(_environment.WebRootPath, "assets", "images", "website-images", uniqueFileName);
        using FileStream stream = new FileStream(folderPath, FileMode.Create);
        service.Image.CopyTo(stream);
        service.ImageUrl = uniqueFileName;
        _context.Services.Add(service);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }


    public IActionResult Delete(int id)
    {
        var item = _context.Services.Find(id);
        if (item == null)
        {
            return NotFound();
        }
        string filePath = Path.Combine(_environment.WebRootPath, "assets", "images", "website-images", item.ImageUrl);
        if (System.IO.File.Exists(filePath))
        {
            System.IO.File.Delete(filePath);
        }

        _context.Services.Remove(item);
        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }


    public IActionResult Update(int id)
    {
        var service = _context.Services.Find(id);
        if (service is null)
        {
            return NotFound();
        }
        return View(service);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Update(Service service)
    {
        if (!ModelState.IsValid)
        {
            return View(service);
        }

        var existService = _context.Services.Find(service.Id);
        if (existService == null)
        {
            return NotFound();
        }

        existService.Title = service.Title;
        existService.Description = service.Description;
        existService.ImageUrl = service.ImageUrl;
        _context.Services.Update(existService);
        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

}

