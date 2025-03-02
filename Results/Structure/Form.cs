namespace ResultsModels.Structure
{
    public class Form
    {
        public Guid FormKey { get; init; } = Guid.NewGuid();

        public List<Student> Students { get; set; } = [];

        public required string Name { get; init; }

        public Average? AverageGrade { get; set; }

        public Average? MaleAverageGrade { get; set; }

        public Average? FemaleAverageGrade { get; set; }
    }
}
