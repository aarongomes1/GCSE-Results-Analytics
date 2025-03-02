using ResultsModels.Structure;

namespace GradeAverageCalculator
{
    internal class AverageCalculator
    {
        public static void CalculateFormAverage(List<Form> forms)
        {
            foreach (var form in forms)
            {
                var results = form.Students.SelectMany(x => x.Results).ToList();

                form.AverageGrade = Results.CalculateAverageGrade(results, "Form Average");
                form.MaleAverageGrade = Results.CalculateAverageGrade(results, "Male", "M");
                form.FemaleAverageGrade = Results.CalculateAverageGrade(results, "Female", "F");
            }
        }

        public static void CalculateStudentAverage(Results results)
        {
            foreach (var student in results.Students)
            {
                student.AverageGrade = Results.CalculateAverageGrade(student.Results, "Student Average");
            }
        }

        public static void CalculateSubjectAverage(List<Subject> subjects)
        {
            foreach (var subject in subjects)
            {
                var results = subject.Students.SelectMany(x => x.Results).ToList();

                subject.AverageGrade = Results.CalculateAverageGrade(results, "Subject Average");
                subject.MaleAverageGrade = Results.CalculateAverageGrade(results, "Male", "M");
                subject.FemaleAverageGrade = Results.CalculateAverageGrade(results, "Female", "F");
            }
        }

        public static void CalculateSubjectGradeCount(List<Subject> subjects)
        {
            foreach (var subject in subjects)
            {
                var results = subject.Students.Select(x => x.Results.Single(y => y.Subject.SubjectKey == subject.SubjectKey)).ToList();
                var groupedByGrade = results.GroupBy(x => x.Result.Grade, StringComparer.Ordinal);

                foreach(var grouping in groupedByGrade)
                {
                    var gradeCount = new GradeCount()
                    {
                        Grade = grouping.Key,
                        Count = grouping.ToList().Count
                    };

                    subject.GradeCounts.Add(gradeCount);
                }
            }
        }
    }
}
