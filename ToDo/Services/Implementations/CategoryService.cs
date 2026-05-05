using System;
using ToDoDemo.Models;
using ToDoDemo.Services.Interfaces;

public class CategoryService : ICategoryService
{
    private readonly ToDoContext _context;

    public CategoryService(ToDoContext context)
    {
        _context = context;
    }

    public List<Category> GetAll()
    {
        // 👉 Return all categories from DB
        return _context.Categories.ToList();
    }

    public void Add(string name)
    {
        // 👉 Create new category object
        var category = new Category
        {
            CategoryId = Guid.NewGuid().ToString(),
            Name = name
        };

        // 👉 Save to DB
        _context.Categories.Add(category);
        _context.SaveChanges();
    }
}