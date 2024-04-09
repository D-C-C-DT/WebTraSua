using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebsiteBanTraSua.Models;
using WebsiteBanTraSua.Repositories;

namespace WebsiteBanTraSua.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        // Hiển thị danh sách sản phẩm
        public async Task<IActionResult> Index()
        {


            var products = await _productRepository.GetAllAsync();
            return View(products);

        }


        // Hiển thị form thêm sản phẩm mới
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }

        // Xử lý thêm sản phẩm mới
        [HttpPost]
        public async Task<IActionResult> Create(Product product, IFormFile ImageUrl)
        {
            if (ModelState.IsValid)
            {
                if (ImageUrl != null)
                {
                    // Lưu hình ảnh đại diện
                    product.ImageUrl = await SaveImage(ImageUrl);
                }
                await _productRepository.AddAsync(product);
                return RedirectToAction(nameof(Index));
            }
            // Nếu ModelState không hợp lệ, hiển thị form với dữ liệu đã nhập
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(product);
        }

        private async Task<string> SaveImage(IFormFile image)
        {
            var savePath = Path.Combine("wwwroot/images", image.FileName); // Thay đổi đường dẫn theo cấu hình của bạn
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return "/images/" + image.FileName; // Trả về đường dẫn tương đối
        }

        // Hiển thị thông tin chi tiết sản phẩm
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // Hiển thị form cập nhật sản phẩm
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(product);
        }
        // Xử lý cập nhật sản phẩm
        [HttpPost]
        public async Task<IActionResult> Edit(Product product, IFormFile ImageUrl)
        {
            if (ModelState.IsValid)
            {
                if (ImageUrl != null)
                {
                    // Lưu hình ảnh mới
                    product.ImageUrl = await SaveImage(ImageUrl);
                }
                await _productRepository.UpdateAsync(product);
                return RedirectToAction("Index");
            }
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(product);
        }

        // Hiển thị form xác nhận xóa sản phẩm
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // Xử lý xóa sản phẩm
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
         public IActionResult Search()
        {
            return View(); // Trả về view cho giao diện tìm kiếm
        }
        [HttpPost]
        public IActionResult Search(string searchTerm)
        {
            // Gọi phương thức tìm kiếm từ repository
            IEnumerable<Product> searchResults = _productRepository.SearchProducts(searchTerm);

            // Trả về view với kết quả tìm kiếm
            return View("SearchResults", searchResults);
        }
        public async Task<IActionResult> SortByName()
        {
            var sortedProducts = await _productRepository.GetAllAsync();
            sortedProducts = sortedProducts.OrderBy(p => p.Name);
            return View("Index", sortedProducts);
        }

        public async Task<IActionResult> SortByPriceAsc()
        {
            var sortedProducts = await _productRepository.GetAllAsync();
            sortedProducts = sortedProducts.OrderBy(p => p.Price);
            return View("Index", sortedProducts);
        }
        public async Task<IActionResult> SortByPriceDesc()
        {
            var sortedProducts = await _productRepository.GetAllAsync();
            sortedProducts = sortedProducts.OrderByDescending(p => p.Price);
            return View("Index", sortedProducts);
        }
        [HttpPost]
        public async Task<IActionResult> Like(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            product.Likes++;
            await _productRepository.UpdateAsync(product);

            return Ok();
        }

    }
}
  



    

