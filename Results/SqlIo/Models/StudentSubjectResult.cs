using Dapper.Contrib.Extensions;

namespace ResultsModels.SqlIo.Models
{
    internal class StudentSubjectResult
    {
        [ExplicitKey]
        public required int StudentKey { get; init; }
        [ExplicitKey]
        public required int SubjectKey { get; init; }
        public required string Result { get; init; }
        public required string? AdditionalResult { get; init; }
    }
}
