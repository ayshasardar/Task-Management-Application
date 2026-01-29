using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoDemo.Models;
using ToDoDemo.ViewModels;

namespace ToDoDemo.Controllers
{
    public class HomeController : Controller
    {
        private ToDoContext context;
        public HomeController(ToDoContext _context)
        {
            this.context = _context;
        }

        public IActionResult Index(ToDoFilterViewModel filterVm, ToDoSortViewModel sortVm, int page = 1)
        {
            const int pageSize = 5;

            // Safety (if user manually edits URL)
            filterVm ??= new ToDoFilterViewModel();
            sortVm??= new ToDoSortViewModel();

            var filters = new Filters(filterVm.FilterString);


            //ToDo is record and ToDos is table of that records
            IEnumerable<ToDo> query = context.ToDos
                                    .Include(t => t.Category)
                                    .Include(t => t.Status);
            if (filters.HasCategory)
            {
                query = query.Where(t => t.CategoryId == filters.CategoryId);
            }
            if (filters.HasStatus)
            {
                query = query.Where(t => t.StatusId == filters.StatusId);
            }
            if (filters.HasDue)
            {
                var today = DateTime.Today;
                if (filters.IsPast)
                {
                    query = query.Where(t => t.DueDate < today);

                }
                else if (filters.IsFuture)
                {
                    query = query.Where(t => t.DueDate > today);

                }
                else if (filters.IsToday)
                {
                    query = query.Where(t => t.DueDate == today);

                }
            }

            //Sorting
            query = sortVm.SortBy switch
            {
                "category" => sortVm.Direction == "asc"
                    ? query.OrderBy(t => t.Category.Name)
                    : query.OrderByDescending(t => t.Category.Name),

                "status" => sortVm.Direction == "asc"
                    ? query.OrderBy(t => t.Status.Name)
                    : query.OrderByDescending(t => t.Status.Name),

                _ => sortVm.Direction == "asc"
                    ? query.OrderBy(t => t.DueDate)
                    : query.OrderByDescending(t => t.DueDate)
            };

            // Pagination calculation
            int totalItems = query.Count();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var tasks = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            //var tasks = query.OrderBy(t => t.DueDate).ToList();


            var vm = new ToDoIndexViewModel
            {
                Filters = filters,
                FilterVm = filterVm,
                Categories = context.Categories.ToList(),
                Statuses = context.Statuses.ToList(),
                ToDos = tasks,
                SortVm = sortVm,
                CurrentPage = page,
                TotalPages = totalPages
            };

            return View(vm);
        }
        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Categories = context.Categories.ToList();
            ViewBag.Statuses = context.Statuses.ToList();
            var task = new ToDo { StatusId = "open" };
            return View(task);
        }
        [HttpPost]
        public IActionResult Add(ToDo task)
        {
            if (ModelState.IsValid)
            {
                context.ToDos.Add(task);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            else 
            {
                ViewBag.Categories = context.Categories.ToList();
                ViewBag.Statuses = context.Statuses.ToList();
                return View(task);
            }
        }
        [HttpPost]
        public IActionResult Filter(string[] filter) 
        {
            string id = string.Join('-', filter);
            return RedirectToAction("Index", new { ID = id});
        }

        [HttpPost]
        public IActionResult MarkComplete([FromRoute]string id, ToDo selected) 
        {
            var selectedToDo = context.ToDos.Find(selected.Id);
            if (selectedToDo != null) 
            {
                selectedToDo.StatusId = "close";
                context.SaveChanges();
            }
            return RedirectToAction("Index", new { ID = id});
        }
        [HttpPost]
        public IActionResult DeleteComplete(string id) 
        {
            var ToDelete = context.ToDos.Where(t => t.StatusId == "close").ToList();
            
            foreach (var task in ToDelete) 
            {
                context.ToDos.Remove(task);
            }
            context.SaveChanges();

            return RedirectToAction("Index", new { ID = id});
        }
    }
}
