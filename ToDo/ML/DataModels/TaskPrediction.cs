using Microsoft.ML.Data;

namespace ToDoDemo.ML.DataModels
{
    public class TaskPrediction
    {
        //ML.NET internally outputs prediction as "PredictedLabel",
        //so I map it as "PredictedPriority" for clarity.

        [ColumnName("PredictedLabel")]
        public string PredictedPriority { get; set; } = string.Empty;
    }
}