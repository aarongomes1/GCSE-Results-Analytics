namespace ResultsModels.Structure
{
    public class Student
    {
        public required int StudentKey { get; init; }

        public required string FirstName { get; init; }

        public required string LastName { get; init; }

        public required string Gender { get; init; }

        public Form? Form { get; set; }

        public List<StudentSubjectResult> Results { get; set; } = [];

        public Average? AverageGrade { get; set; }

        public string GetFullName()
        {
            return $"{FirstName} {LastName}";
        }
    }
}
