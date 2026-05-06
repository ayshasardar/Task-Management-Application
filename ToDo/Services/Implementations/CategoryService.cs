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

    public bool Add(string name)
    {
        var CategoryExist = _context.Categories.Any(e => e.Name.ToLower() == name.ToLower());
        if (CategoryExist)
            return false;

        // 👉 Create new category object
        var category = new Category
        {
            CategoryId = Guid.NewGuid().ToString(),
            Name = name
        };

        // 👉 Save to DB
        _context.Categories.Add(category);
        _context.SaveChanges();
        return true;
    }
}