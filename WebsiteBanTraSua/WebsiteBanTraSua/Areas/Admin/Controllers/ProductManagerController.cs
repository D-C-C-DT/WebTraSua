using Microsoft.AspNetCore.Mvc;
using WebsiteBanTraSua.Repositories;

namespace WebsiteBanTraSua.Areas.Admin.Controllers
{
    [Area("Admim")]
    public class ProductManageController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductManageController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }
        public async Task<IActionResult> Index()
        {


            var products = await _productRepository.GetAllAsync();
            return View(products);

        }
    }
}
