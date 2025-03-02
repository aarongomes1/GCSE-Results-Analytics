using Dapper.Contrib.Extensions;

namespace ResultsModels.SqlIo.Models
{
    internal class FormStudent
    {
        [ExplicitKey]
        public required string FormKey { get; init; }
        [ExplicitKey]
        public required int StudentKey { get; init; }
    }
}
