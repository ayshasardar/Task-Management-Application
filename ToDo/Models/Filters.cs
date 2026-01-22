using Microsoft.VisualBasic;

namespace ToDoDemo.Models
{
    public class Filters
    {
        public Filters(string filterstring) 
        {
            FilterString = filterstring ?? "all-all-all";
            string[] filter = FilterString.Split('-');
            CategoryId = filter[0];
            Due = filter[1];
            StatusId = filter[2];

        }
        //read-only properties -  can set them only inside constructor
        public string FilterString { get; }
        public string CategoryId { get; }
        public string Due { get; }
        public string StatusId { get; }

        public bool HasCategory => CategoryId.ToLower() != "all";
        public bool HasDue => Due.ToLower() != "all";
        public bool HasStatus => StatusId.ToLower() != "all";
        public static Dictionary<string, string> DueFilterValues =>
            new Dictionary<string, string>
            {
                {"future","Future" },
                {"past","Past" },
                {"today","Today" }
            };
        public bool IsPast => Due.ToLower() == "past";
        public bool IsFuture => Due.ToLower() == "future";
        public bool IsToday => Due.ToLower() == "today";
    }
}
