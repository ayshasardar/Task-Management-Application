using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Web.Mvc;
using ToDoDemo.ML.Services;
using ToDoDemo.Models;
using ToDoDemo.Services.Interfaces;
using ToDoDemo.ViewModels;

namespace ToDoDemo.Services.Implementations
{
    public class ToDoService : IToDoService
    {
        private readonly ToDoContext _context;
        private readonly MLPriorityService _mlService;
        public ToDoService(ToDoContext context, MLPriorityService mlService)
        {
            _context = context;
            _mlService = mlService;
        }
        public ToDoIndexViewModel Index(ToDoFilterViewModel filterVm, int page, int pageSize)
        {
            // Safety (if user manually edits URL)
            filterVm ??= new ToDoFilterViewModel();

            var filters = new Filters(filterVm.FilterString);


            //ToDo is record and ToDos is table of that records
            IEnumerable<ToDo> query = _context.ToDos
                                    .Include(t => t.Category)
                                    .Include(t => t.Status);
            //Exclude deleted records
            query = query.Where(t => !t.IsDeleted);

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
            //Priority
            if (filterVm.Priority == PriorityLevel.High)
            {
                query = query.Where(t => t.Priority == PriorityLevel.High);
            }
            else if (filterVm.Priority == PriorityLevel.Medium)
            {
                query = query.Where(t => t.Priority == PriorityLevel.Medium);

            }
            else if (filterVm.Priority == PriorityLevel.Low)
            {
                query = query.Where(t => t.Priority == PriorityLevel.Low);

            }

            //OLD Sorting - when sorting was handled by header click
            //query = sortVm.SortBy switch
            //{
            //    "category" => sortVm.Direction == "asc"
            //        ? query.OrderBy(t => t.Category.Name)
            //        : query.OrderByDescending(t => t.Category.Name),

            //    "status" => sortVm.Direction == "asc"
            //        ? query.OrderBy(t => t.Status.Name)
            //        : query.OrderByDescending(t => t.Status.Name),

            //    _ => sortVm.Direction == "asc"
            //        ? query.OrderBy(t => t.DueDate)
            //        : query.OrderByDescending(t => t.DueDate)
            //};

            // Sorting

            query = filterVm.SortBy switch
            {
                "priority" => query.OrderByDescending(t => t.Priority),
                "category" => query.OrderBy(t => t.Category.Name),
                "desc" => query.OrderBy(t => t.Description),
                _ => query.OrderBy(t => t.DueDate)
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
                Categories = _context.Categories.ToList(),
                Statuses = _context.Statuses.ToList(),
                Priorities = Enum.GetValues(typeof(PriorityLevel)).Cast<PriorityLevel>(),
                ToDos = tasks,
                //SortVm = sortVm,
                CurrentPage = page,
                TotalPages = totalPages
            };
            return vm;
        }
        public AddViewModel Add()
        {

            var task = new ToDo { StatusId = "open", Priority = PriorityLevel.Medium };
            var vm = new AddViewModel
            {
                Categories = _context.Categories.ToList(),
                Statuses = _context.Statuses.ToList(),
                ToDo = task
            };
            return vm;
        }
        public IEnumerable<Category> GetCategories()
        {
            var cat = _context.Categories.ToList();
            return cat;
        }
        public IEnumerable<Status> GetStatuses()
        {
            var status = _context.Statuses.ToList();
            return status;
        }
        public void AddTask(AddViewModel vm)
        {
            _context.ToDos.Add(vm.ToDo);
            _context.SaveChanges();

        }

        //old method - now using new one to update using fetch API (AJAX)
        //public void MarkComplete(ToDo selected)
        //{
        //    var selectedToDo = _context.ToDos.Find(selected.Id);
        //    if (selectedToDo != null)
        //    {
        //        selectedToDo.StatusId = "close";
        //        _context.SaveChanges();
        //    }
        //}
        public bool MarkComplete(int id)
        {
            var task = _context.ToDos.Find(id);
            if (task == null) return false;

            task.StatusId = "close";
            _context.SaveChanges();

            return true;
        }
        public void DeleteComplete()
        {
            var ToDelete = _context.ToDos.Where(t => t.StatusId == "close").ToList();

            foreach (var task in ToDelete)
            {
                //_context.ToDos.Remove(task);
                task.IsDeleted = true;
            }
            _context.SaveChanges();
        }
        public void Delete(ToDo selected)
        {
            var selectedToDo = _context.ToDos.Find(selected.Id);
            if (selectedToDo != null)
            {
                selectedToDo.IsDeleted = true;
                _context.SaveChanges();
            }
        }
        public string PredictPriority(ToDo task)
        {
            float dueDays = 0;

            if (task.DueDate.HasValue)
                dueDays = (float)(task.DueDate.Value - DateTime.Today).TotalDays;

            return _mlService.Predict(
                task.Description,
                dueDays,
                task.CategoryId   // OK for now
            );
        }
    }
}
