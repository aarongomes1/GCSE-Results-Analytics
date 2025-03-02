using ResultsModels.Structure;

namespace GradeAverageCalculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Expected 1 parameter");
                Console.WriteLine("1) Path to the database");
                return;
            }

            var databaseFilePath = args[0];

            var results = Results.Load(databaseFilePath);

            AverageCalculator.CalculateFormAverage(results.Forms);
            AverageCalculator.CalculateStudentAverage(results);
            AverageCalculator.CalculateSubjectAverage(results.Subjects);
            AverageCalculator.CalculateSubjectGradeCount(results.Subjects);

            results.Save(databaseFilePath);
        }
    }
}
