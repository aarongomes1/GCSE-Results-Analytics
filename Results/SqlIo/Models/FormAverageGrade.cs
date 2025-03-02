
namespace ResultsModels.SqlIo.Models
{
    internal class FormAverageGrade
    {
        public required string FormKey { get; init; }
        public required decimal AverageGrade { get; init; }
        public required decimal MaleAverageGrade { get; init; }
        public required decimal FemaleAverageGrade { get; init; }
    }
}
