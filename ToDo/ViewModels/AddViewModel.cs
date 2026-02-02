using ToDoDemo.Models;

namespace ToDoDemo.ViewModels
{
    public class AddViewModel
    {
        //Dropdown Data - Replacing ViewBags
        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
        public IEnumerable<Status> Statuses { get; set; } = new List<Status>();
        public ToDo ToDo { get; set; } = new ToDo();
    }
}
