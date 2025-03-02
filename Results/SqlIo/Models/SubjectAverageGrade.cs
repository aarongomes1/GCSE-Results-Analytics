
namespace ResultsModels.SqlIo.Models
{
    internal class SubjectAverageGrade
    {
        public required int SubjectKey { get; init; }

        public required decimal AverageGrade { get; init; }

        public required decimal MaleAverageGrade { get; init; }

        public required decimal FemaleAverageGrade { get; init; }
    }
}
