using ToDoDemo.Models;
namespace ToDoDemo.Services.Interfaces
{

    public interface ICategoryService
    {
        List<Category> GetAll();
        void Add(string name);
    }
}
