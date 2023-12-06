using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {

        private readonly IUnitOfWork _unitofWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitofWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _unitofWork.Catagory.GetAll().ToList();
            return View(objCategoryList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The display order cannot exactly match the Name.");
            }
            if (obj.Name.ToLower() == "test")
            {
                ModelState.AddModelError("name", "Invalid Value");
            }
            if (ModelState.IsValid)
            {
                _unitofWork.Catagory.Add(obj);
                _unitofWork.Save();
                TempData["success"] = "Category created sucessfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = _unitofWork.Catagory.Get(u => u.Id == id);
            //Category? categoryFromDb2 = _db.Categories.Where(u=>u.Id==id).FirstOrDefault();
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The display order cannot exactly match the Name.");
            }
            if (obj.Name.ToLower() == "test")
            {
                ModelState.AddModelError("name", "Invalid Value");
            }
            if (ModelState.IsValid)
            {
                _unitofWork.Catagory.Update(obj);
                _unitofWork.Save();
                TempData["success"] = "Category updated sucessfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category categoryFromDb = _unitofWork.Catagory.Get(u => u.Id == id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? obj = _unitofWork.Catagory.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }


            _unitofWork.Catagory.Remove(obj);
            _unitofWork.Save();
            TempData["success"] = "Category deleted sucessfully";
            return RedirectToAction("Index");


        }




    }
}
