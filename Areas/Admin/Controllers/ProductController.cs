using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Protocol.Plugins;
using System.Collections.Generic;


namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {

        private readonly IUnitOfWork _unitofWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitofWork = unitOfWork;
            _webHostEnvironment=webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Product> objCategoryList = _unitofWork.Product.GetAll().ToList();
            return View(objCategoryList);
        }


        //public IActionResult Create()
        //{
        //   // ViewBag.CategoryList = CategoryList;
        //   ProductVM productVM = new()
        //   {
        //       CategoryList =_unitofWork.Catagory.GetAll().Select(u => new SelectListItem
        //       {
        //           Text = u.Name,
        //           Value = u.Id.ToString()
        //       }),

        //       Product = new Product()
        //   };
        //    return View(productVM);
        //}
        //[HttpPost]
        //public IActionResult Create(ProductVM vm, IFormFile? file)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        string wwwRootPath = _webHostEnvironment.WebRootPath;
        //        if(file !=null)
        //        {
        //            string fileName = Guid.NewGuid().ToString()+Path.GetExtension(file.FileName);
        //            string productPath =  Path.Combine(wwwRootPath, @"images\product");
        //            using( var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
        //            {
        //                file.CopyTo(fileStream);
        //            }
        //            vm.Product.ImageUrl=@"\images\product"+fileName;

        //        }
        //        _unitofWork.Product.Add(vm.Product);
        //        _unitofWork.Save();
        //        TempData["success"] = "Category created sucessfully";
        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        IEnumerable<SelectListItem> CategoryList = _unitofWork.Catagory
        //       .GetAll().Select(u => new SelectListItem
        //       {
        //           Text = u.Name,
        //           Value = u.Id.ToString()
        //       }
        //       );
        //       vm.CategoryList = CategoryList;
        //    }

        //    return View();
        //}

        //public IActionResult Edit(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    Product? productFromDb = _unitofWork.Product.Get(u => u.Id == id);
        //    //Category? categoryFromDb2 = _db.Categories.Where(u=>u.Id==id).FirstOrDefault();
        //    if (productFromDb == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(productFromDb);
        //}

        //[HttpPost]
        //public IActionResult Edit(Product obj)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _unitofWork.Product.Update(obj);
        //        _unitofWork.Save();
        //        TempData["success"] = "Category updated sucessfully";
        //        return RedirectToAction("Index");
        //    }
        //    return View();
        //}

        public IActionResult Upsert(int? id)
        {
            // ViewBag.CategoryList = CategoryList;
            ProductVM productVM = new()
            {
                CategoryList =_unitofWork.Catagory.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),

                Product = new Product()
            };
            if(id==null || id == 0)
            {
                //create
                return View(productVM);
            }
            else
            {
                //update
                productVM.Product = _unitofWork.Product.Get(u => u.Id ==id);
                return View(productVM);
            }
            
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {

            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file !=null)
                {
                    string fileName = Guid.NewGuid().ToString()+Path.GetExtension(file.FileName);
                    string finalPath = @"images\product\";
                    string productPath = Path.Combine(wwwRootPath, finalPath);
                    

                    if(!string.IsNullOrEmpty(productVM.Product.ImageUrl)) 
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVM.Product.ImageUrl=@"\images\product\"+ fileName;

                }

                if(productVM.Product.Id == 0)
                {
                    _unitofWork.Product.Add(productVM.Product);
                }
                else
                {
                    _unitofWork.Product.Update(productVM.Product);
                }
                _unitofWork.Save();
                TempData["success"] = "Category created sucessfully";
                return RedirectToAction("Index");
            }
            else
            {
                IEnumerable<SelectListItem> CategoryList = _unitofWork.Catagory
               .GetAll().Select(u => new SelectListItem
               {
                   Text = u.Name,
                   Value = u.Id.ToString()
               }
               );
                productVM.CategoryList = CategoryList;
            }

            return View();
        }


        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product productFromDb = _unitofWork.Product.Get(u => u.Id == id);
            if (productFromDb == null)
            {
                return NotFound();
            }
            return View(productFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Product? obj = _unitofWork.Product.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }


            _unitofWork.Product.Remove(obj);
            _unitofWork.Save();
            TempData["success"] = "Category deleted sucessfully";
            return RedirectToAction("Index");


        }




    }
}
