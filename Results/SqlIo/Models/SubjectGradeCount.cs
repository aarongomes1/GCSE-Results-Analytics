
namespace ResultsModels.SqlIo.Models
{
    internal class SubjectGradeCount
    {
        public required int SubjectKey { get; init; }
        public required string Grade { get; init; }
        public required int Count { get; init; }
    }
}
