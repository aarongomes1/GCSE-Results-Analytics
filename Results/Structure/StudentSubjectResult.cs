namespace ResultsModels.Structure
{
    public class StudentSubjectResult
    {
        public required Student Student { get; set; }
        public required Subject Subject { get; set; }
        public required Result Result { get; set; }
        public Result? AdditionalResult { get; set; }
    }
}
