using Microsoft.AspNetCore.Mvc;
using ToDoDemo.Services.Interfaces;
using ToDoDemo.ViewModels;

public class CategoriesController : Controller
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }


    // GET: Add Category Page
    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }

    // POST: Save Category
    [HttpPost]
    public IActionResult Add(AddCategoryViewModel vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        //Call service instead of DB directly
        bool CategoryNotExist = _categoryService.Add(vm.Name);
        if (CategoryNotExist)
        {
            TempData["ToastMessage"] = "Category added successfully";
            TempData["ToastType"] = "success";
            return RedirectToAction("Add", "Home");
        }
        else
        {
            TempData["ToastMessage"] = "Category already exist!";
            TempData["ToastType"] = "error";
            return View();
        }

        
    }
}