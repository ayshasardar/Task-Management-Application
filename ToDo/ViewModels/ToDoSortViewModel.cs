namespace ToDoDemo.ViewModels
{
    public class ToDoSortViewModel
    {
        public string SortBy { get; set; } = "duedate";
        public string Direction { get; set; } = "asc";

        public string ToggleDirection =>
            Direction == "asc" ? "desc" : "asc";
    }

}
