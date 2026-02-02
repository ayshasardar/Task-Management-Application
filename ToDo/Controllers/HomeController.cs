using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ToDoDemo.Models;
using ToDoDemo.Services.Interfaces;
using ToDoDemo.ViewModels;

namespace ToDoDemo.Controllers
{
    public class HomeController : Controller
    {
        //private ToDoContext context;
        private readonly IToDoService _toDoService;

        public HomeController(IToDoService toDoService)
        {
            _toDoService = toDoService;
        }

        public IActionResult Index(ToDoFilterViewModel filterVm, int page = 1)
        {
            const int pageSize = 5;

            //call the IService for ToDo
            var vm = _toDoService.Index(filterVm,page,pageSize);
            

            return View(vm);
        }
        [HttpGet]
        public IActionResult Add()
        {

            var vm = _toDoService.Add();
            return View(vm);
        }
        [HttpPost]
        public IActionResult AddTask(AddViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _toDoService.AddTask(vm);
                return RedirectToAction("Index");
            }
            else
            {
                vm.Categories = _toDoService.GetCategories();
                vm.Statuses = _toDoService.GetStatuses();
                return View(vm);
            }
        }
        [HttpPost]
        public IActionResult Filter(string[] filter)
        {
            string id = string.Join('-', filter);
            return RedirectToAction("Index", new { ID = id });
        }

        [HttpPost]
        public IActionResult MarkComplete([FromRoute] string id, ToDo selected)
        {
            _toDoService.MarkComplete(selected);
            return RedirectToAction("Index", new { ID = id });
        }
        [HttpPost]
        public IActionResult DeleteComplete(string id)
        {
            _toDoService.DeleteComplete();

            return RedirectToAction("Index", new { ID = id });
        }
    }
}
