using Microsoft.ML.Data;

namespace ToDoDemo.ML.DataModels
{
    public class TaskData
    {
        [LoadColumn(0)]
        public string Description { get; set; } = string.Empty;

        // Number of days until due date
        [LoadColumn(1)]
        public float DueDays { get; set; }
        [LoadColumn(2)]
        public string Category { get; set; } = string.Empty;

        // This is the LABEL (what we want to predict)
        [LoadColumn(3)]
        public string Priority { get; set; } = string.Empty;
    }
}