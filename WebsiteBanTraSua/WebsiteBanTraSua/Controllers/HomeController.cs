using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebsiteBanTraSua.Models;
using WebsiteBanTraSua.Repositories;

namespace WebsiteBanTraSua.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepository _productRepository;

        public HomeController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllAsync(); // L?y danh sách s?n ph?m t? Repository
            return View(products); // Truy?n danh sách s?n ph?m vào View
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
