using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Contexts;
using Pronia.Helpers;
using Pronia.Models;
using Pronia.ViewModels.ProductViewModels;

namespace Pronia.Areas.Admin.Controllers;
    [Area("Admin")]

    public class ProductController(AppDBContext _context,IWebHostEnvironment _environment) : Controller
    {
        public IActionResult Index()
        {
            var products = _context.Products.Include(x => x.Category).ToList();
            return View(products);
        }

        [HttpGet]
        public IActionResult Create()
        {
            SendCategoriesWithViewBag();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductCreateVM productVM)
        {
            if (!ModelState.IsValid)
            {
            SendCategoriesWithViewBag();
            return View(productVM);
            }

        if (!productVM.MainImage.CheckType("image"))
        {
            SendCategoriesWithViewBag();
            ModelState.AddModelError("MainImage", "Please add only image!");
            return View(productVM);
        }

        if (!productVM.MainImage.CheckSize(2))
        {
            SendCategoriesWithViewBag();
            ModelState.AddModelError("MainImage", "Maximum 2 mb!");
            return View(productVM);
        }

        if (!productVM.HoverImage.CheckType("image"))
        {
            SendCategoriesWithViewBag();
            ModelState.AddModelError("HoverImage", "Please add only image!");
            return View(productVM);
        }

        if (!productVM.HoverImage.CheckSize(2))
        {
            SendCategoriesWithViewBag();
            ModelState.AddModelError("HoverImage", "Maximum 2 mb!");
            return View(productVM);
        }


        var isExistCategory = _context.Categories.Any(x => x.Id == productVM.CategoryId);

            if (!isExistCategory)
            {
                SendCategoriesWithViewBag();
                ModelState.AddModelError("", "Category is not found!");
                return View(productVM);
            }
        string folderPath = Path.Combine(_environment.WebRootPath, "assets", "images", "website-images");

        string mainImageName = productVM.MainImage.SaveFile(folderPath);
        string hoverImageName = productVM.HoverImage.SaveFile(folderPath);

        Product product = new()
        {
            Name = productVM.Name,
            Description = productVM.Description,
            Price = productVM.Price,
            SKU = productVM.SKU,
            MainImageUrl = mainImageName,
            HoverImageUrl = hoverImageName,
            CategoryId = productVM.CategoryId
        };
            _context.Products.Add(product);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int id)
        {
        var product = _context.Products.Find(id);

        if (product == null)
        {
            return NotFound();
        }
        SendCategoriesWithViewBag();
        ProductUpdateVM vm = new ProductUpdateVM()
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            CategoryId = product.CategoryId,
            Price = product.Price,
            SKU = product.SKU,
        };
        return View(vm);
    }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(ProductUpdateVM vm)
        {
        if (!ModelState.IsValid)
        {
            SendCategoriesWithViewBag();
            return View(vm);
        }

        if (!vm.MainImage?.CheckType("image") ?? false)
        {
            SendCategoriesWithViewBag();
            ModelState.AddModelError("MainImage", "Please add only image!");
            return View(vm);
        }

        if (vm.MainImage?.CheckSize(2) ?? false)
        {
            SendCategoriesWithViewBag();
            ModelState.AddModelError("MainImage", "Maximum 2 mb!");
            return View(vm);
        }

        if (!vm.HoverImage?.CheckType("image") ?? false)
        {
            SendCategoriesWithViewBag();
            ModelState.AddModelError("HoverImage", "Please add only image!");
            return View(vm);
        }

        if (vm.HoverImage?.CheckSize(2) ?? false)
        {
            SendCategoriesWithViewBag();
            ModelState.AddModelError("HoverImage", "Maximum 2 mb!");
            return View(vm);
        }

        var existProduct = _context.Products.Find(vm.Id);

        if (existProduct is null)
        {
            return NotFound();
        }

        var isExistCategory = _context.Categories.Any(c => c.Id == vm.CategoryId);

        if (!isExistCategory)
        {
            SendCategoriesWithViewBag();
            ModelState.AddModelError("CategoryId", "Category not found!");
            return View(vm);
        }
        existProduct.Name = vm.Name;
        existProduct.Description = vm.Description;
        existProduct.Price = vm.Price;
        existProduct.SKU = vm.SKU;
        existProduct.CategoryId = vm.CategoryId;

        string folderPath = Path.Combine(_environment.WebRootPath, "assets", "images", "website-images");

        if (vm.MainImage != null)
        {
            string newMainImageName = vm.MainImage.SaveFile(folderPath);
            if (System.IO.File.Exists(Path.Combine(folderPath, existProduct.MainImageUrl)))
            {
                System.IO.File.Delete(Path.Combine(folderPath, existProduct.MainImageUrl));
            }
            existProduct.MainImageUrl = newMainImageName;
        }

        if (vm.HoverImage != null)
        {
            string newHoverImageName = vm.HoverImage.SaveFile(folderPath);
            if (System.IO.File.Exists(Path.Combine(folderPath, existProduct.HoverImageUrl)))
            {
                System.IO.File.Delete(Path.Combine(folderPath, existProduct.HoverImageUrl));
            }
            existProduct.HoverImageUrl = newHoverImageName;
        }

        _context.Update(existProduct);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);

            if (product is null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        private void SendCategoriesWithViewBag()
        {
            var categories = _context.Categories.ToList();
            ViewBag.Categories = categories;
        }
    }

