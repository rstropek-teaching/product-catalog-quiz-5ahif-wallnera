using Microsoft.AspNetCore.Mvc;
using ProductCatalogMVC.Models;
using ProductCatalogMVC.Services;

namespace ProductCatalogMVC.Controllers
{
    public class ProductController : Controller
    {

        private IProductRepository productRepository;


        public ProductController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        /// <summary>
        /// return startpage
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View(productRepository.GetAll());
        }

        /// <summary>
        /// return product Details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Details(int id)
        {
            var proddetails = this.productRepository.GetById(id);
            if(proddetails == null)
            {
                return NotFound();
            }
            return View(proddetails);
        }

        /// <summary>
        /// adds a product to the database
        /// </summary>
        /// <param name="prod"></param>
        /// <returns></returns>
        public IActionResult Create(Product prod)
        {
            if (ModelState.IsValid)
            {
                if (prod != null)
                {
                    this.productRepository.CreateProduct(prod);
                }
            }
            return View();
        }

        /// <summary>
        /// show product which is about to be deleted
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Delete(int id)
        {
            var productdelete = productRepository.GetById(id);
            if(productdelete == null)
            {
                return NotFound();
            }
            return View(productdelete);
        }

        /// <summary>
        /// delete the Product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult DeleteConfirmed(int id)
        {
            productRepository.DeleteProduct(id);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// returns a list of products which name are included or the same es the 
        /// searched ones
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public IActionResult ProductSearch(string name)
        {
            return View(productRepository.ProductSearch(name));
        }
    }
}