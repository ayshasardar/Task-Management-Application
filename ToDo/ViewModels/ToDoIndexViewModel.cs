using ToDoDemo.Models;
using System.Collections.Generic;

namespace ToDoDemo.ViewModels
{
    public class ToDoIndexViewModel
    {
        //list of tasks to show
        public IEnumerable<ToDo> ToDos { get; set; } = new List<ToDo>();

        //Filter Data
        public Filters Filters { get; set; } = new Filters("all-all-all");
        public ToDoFilterViewModel FilterVm { get; set; } = new();

        //Sorting
        public ToDoSortViewModel SortVm { get; set; } = new();


        //Dropdown Data - Replacing ViewBags
        public IEnumerable<Category> Categories { get; set; }= new List<Category>();
        public IEnumerable<Status> Statuses { get; set; }= new List<Status>();
        public Dictionary<string, string> DueFilters { get; set;}= Filters.DueFilterValues;
        
        //Pagination
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
