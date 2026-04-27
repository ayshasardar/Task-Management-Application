using Microsoft.ML;
using ToDoDemo.ML.DataModels;

namespace ToDoDemo.ML.Services
{
    public class MLPriorityService
    {
        private readonly MLContext _mlContext;
        private ITransformer? _model;

        public MLPriorityService()
        {
            _mlContext = new MLContext();
        }

        //Training Method - must call TrainModel() once before prediction
        //(e.g., app startup or manually trigger)
        public void TrainModel()
        {
            var dataPath = Path.Combine("ML", "Model", "data.csv");

            var data = _mlContext.Data.LoadFromTextFile<TaskData>(
                dataPath,
                hasHeader: true,
                separatorChar: ',');

            var pipeline = _mlContext.Transforms.Text
                .FeaturizeText("DescriptionFeaturized", nameof(TaskData.Description))

                .Append(_mlContext.Transforms.Categorical
                .OneHotEncoding("CategoryEncoded", nameof(TaskData.Category)))

                .Append(_mlContext.Transforms.Conversion
                .MapValueToKey("Label", nameof(TaskData.Priority)))

                .Append(_mlContext.Transforms.Concatenate(
                    "Features",
                    "DescriptionFeaturized",
                    "CategoryEncoded",
                    nameof(TaskData.DueDays)))

                .Append(_mlContext.MulticlassClassification
                .Trainers.SdcaMaximumEntropy())

                .Append(_mlContext.Transforms.Conversion
                .MapKeyToValue("PredictedLabel"));

            _model = pipeline.Fit(data);

            // Save model
            var modelPath = Path.Combine("ML", "Model", "model.zip");
            _mlContext.Model.Save(_model, data.Schema, modelPath);
        }

        //Prediction Method
        public string Predict(string description, float dueDays, string category)
        {
            if (_model == null)
            {
                var modelPath = Path.Combine("ML", "Model", "model.zip");
                _model = _mlContext.Model.Load(modelPath, out _);
            }

            var engine = _mlContext.Model.CreatePredictionEngine<TaskData, TaskPrediction>(_model);

            var input = new TaskData
            {
                Description = description,
                DueDays = dueDays,
                Category = category
            };

            var result = engine.Predict(input);

            return result.PredictedPriority;
        }
    }
}