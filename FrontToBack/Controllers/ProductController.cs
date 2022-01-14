using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FrontToBack.Models;

namespace FrontToBack.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly int _productsCount;

        public ProductController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _productsCount = _dbContext.Products.Count();
        }

        public IActionResult Index()
        {
            ViewBag.Products = _productsCount;

            var products = _dbContext.Products.Include(x => x.Category)
                .Where(x => x.IsDeleted == false && x.Category.IsDeleted == false).OrderByDescending(x => x.Id).Take(8).ToList();
            return View(products);
        }

        public IActionResult Load(int skip)
        {
            if (skip >= _productsCount)
                return BadRequest();

            var products = _dbContext.Products.Include(x => x.Category).OrderByDescending(x => x.Id).Skip(skip).Take(8).ToList();

            return PartialView("_ProductPartial", products);
        }
    }
}
