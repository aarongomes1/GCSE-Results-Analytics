namespace ResultsModels.Structure
{
    public class Subject
    {
        public required int SubjectKey { get; init; }

        public required string SubjectName { get; init; }

        public required string Board { get; init; }

        public List<Student> Students { get; set; } = [];

        public bool UsesDoubleGrade = false;

        public List<GradeCount> GradeCounts { get; set; } = [];

        public Average? AverageGrade { get; set; }

        public Average? MaleAverageGrade { get; set; }

        public Average? FemaleAverageGrade { get; set; }
    }
}
