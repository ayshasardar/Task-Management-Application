using ToDoDemo.Models;

namespace ToDoDemo.ViewModels
{
    public class ToDoFilterPartialViewModel
    {
        public ToDoFilterViewModel FilterVm { get; set; } = new();

        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
        public IEnumerable<Status> Statuses { get; set; } = new List<Status>();
        public Dictionary<string, string> DueFilters { get; set; } = Filters.DueFilterValues;
    }
}
