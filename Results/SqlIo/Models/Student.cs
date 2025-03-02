
namespace ResultsModels.SqlIo.Models
{
    public class Student
    {
        public required int StudentKey { get; init; }
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public required string Gender { get; init; }
    }
}
