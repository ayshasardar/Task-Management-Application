namespace ToDoDemo.ViewModels
{
    public class ToDoFilterViewModel
    {
        public string Category { get; set; } = "all";
        public string Due { get; set; } = "all";
        public string Status { get; set; } = "all";

        public string FilterString => $"{Category}-{Due}-{Status}";
    }

}
