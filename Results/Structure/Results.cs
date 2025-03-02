using ResultsModels.SqlIo;

namespace ResultsModels.Structure
{
    public class Results
    {
        public List<Student> Students { get; set; }

        public List<Subject> Subjects { get; set; }

        public List<Form> Forms { get; set; }

        public List<Result> ResultValues { get; set; }

        public Average MaleAverageGrade { get; set; }

        public Average FemaleAverageGrade { get; set; }


        public Results(List<Student> students, List<Subject> subjects, List<Form> forms, List<Result> resultValues)
        {
            Students = students;
            Subjects = subjects;
            Forms = forms;
            ResultValues = resultValues;

            var allResults = Students.SelectMany(x => x.Results).ToList();

            MaleAverageGrade = CalculateAverageGrade(allResults, "Male", "M");
            FemaleAverageGrade = CalculateAverageGrade(allResults, "Female", "F");
        }

        public static Results Load(string filePath)
        {
            var results = Loader.Load(filePath);

            return results;
        }

        public void Save(string filePath)
        {
            Saver.Save(filePath, this);
        }

        public static Average CalculateAverageGrade(List<StudentSubjectResult> results, string displayName, string genderFilter = "")
        {
            if (!string.IsNullOrWhiteSpace(genderFilter))
            {
                results = results.Where(x => x.Student.Gender.Equals(genderFilter, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            var resultValues = results.SelectMany(x =>
            {
                var results = new List<decimal> { x.Result.Value };

                if (x.AdditionalResult is not null)
                {
                    results.Add(x.AdditionalResult.Value);
                }

                return results;
            }).ToList();

            var averageValue = resultValues.Count != 0 ? resultValues.Average() : 0;

            var average = new Average()
            {
                Name = displayName,
                Value = Math.Round(averageValue, 3)
            };

            return average;
        }
    }
}
