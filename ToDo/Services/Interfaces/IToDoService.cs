using Microsoft.AspNetCore.Mvc;
using ToDoDemo.Models;
using ToDoDemo.ViewModels;
namespace ToDoDemo.Services.Interfaces
{
    public interface IToDoService
    {
        public ToDoIndexViewModel Index(ToDoFilterViewModel filterVm, int page, int pageSize);
        public AddViewModel Add();
        public void AddTask(AddViewModel vm);
        public IEnumerable<Category> GetCategories();
        public IEnumerable<Status> GetStatuses();

        public void MarkComplete(ToDo selected);
        public void DeleteComplete();

    }
}
