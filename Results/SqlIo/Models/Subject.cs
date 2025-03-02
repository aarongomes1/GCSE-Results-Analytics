
namespace ResultsModels.SqlIo.Models
{
    internal class Subject
    {
        public required int SubjectKey { get; init; }

        public required string SubjectName { get; init; }

        public required string Board { get; init; }

        public required int UsesDoubleGrade { get; init; }
    }
}
