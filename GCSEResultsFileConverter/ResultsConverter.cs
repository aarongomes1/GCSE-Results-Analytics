using GCSEResultsFileConverter.Models;
using ResultsModels.Structure;

namespace GCSEResultsFileConverter
{
    internal class ResultsConverter
    {
        public static Results Convert(List<ResultsModel> inputRecords, List<Result> results)
        {
            var students = new List<Student>();
            var subjects = new List<Subject>();
            var forms = new List<Form>();

            var resultMapping = results.ToDictionary(x => x.Grade);

            var studentIdCreator = new IdGenerator();
            var subjectIdCreator = new IdGenerator();

            foreach (var record in inputRecords)
            {
                var form = TryCreateForm(record, forms);
                var student = TryCreateStudent(record, students, studentIdCreator);
                var subject = TryCreateSubject(record, subjects, subjectIdCreator);

                AssignStudentToForm(form, student);
                AddResult(record, student, subject, resultMapping);
            }

            return new Results(students, subjects, forms, results);
        }

        private static Student TryCreateStudent(ResultsModel inputRecord, List<Student> students, IdGenerator studentIdCreator)
        {
            var splitNames = inputRecord.Name.Split(',');

            var firstName = splitNames[1].Trim();
            var lastName = splitNames[0].Trim();

            var existingStudent = students.SingleOrDefault(x =>
            x.FirstName.Equals(firstName, StringComparison.OrdinalIgnoreCase)
            && x.LastName.Equals(lastName, StringComparison.OrdinalIgnoreCase));

            if (existingStudent is null)
            {
                existingStudent = new Student()
                {
                    StudentKey = studentIdCreator.GetNextId(),
                    FirstName = firstName,
                    LastName = lastName,
                    Gender = inputRecord.Gender,
                };

                students.Add(existingStudent);
            }

            return existingStudent;
        }

        private static Subject TryCreateSubject(ResultsModel inputRecord, List<Subject> subjects, IdGenerator subjectIdCreator)
        {
            var existingSubject = subjects.SingleOrDefault(x => x.SubjectName.Equals(inputRecord.Title, StringComparison.OrdinalIgnoreCase));

            if (existingSubject is null)
            {
                existingSubject = new Subject()
                {
                    SubjectKey = subjectIdCreator.GetNextId(),
                    SubjectName = inputRecord.Title,
                    Board = inputRecord.Board,
                };

                subjects.Add(existingSubject);
            }

            return existingSubject;
        }

        private static Form TryCreateForm(ResultsModel inputRecord, List<Form> forms)
        {
            var existingForm = forms.SingleOrDefault(x => x.Name.Equals(inputRecord.Form, StringComparison.Ordinal));

            if (existingForm is null)
            {
                existingForm = new Form()
                {
                    Name = inputRecord.Form
                };

                forms.Add(existingForm);
            }

            return existingForm;
        }

        private static void AssignStudentToForm(Form form, Student student)
        {
            if (!form.Students.Contains(student))
            {
                form.Students.Add(student);
            }

            student.Form = form;
        }

        private static void AddResult(ResultsModel inputRecord, Student student, Subject subject, Dictionary<string, Result> resultsMapping)
        {
            var result = inputRecord.Result;
            string? additionalResult = null;

            // We have a split grade, process both parts
            if (int.TryParse(result, out var x) && result.Length == 2)
            {
                additionalResult = result[1].ToString();
                result = result[0].ToString();
            }

            var studentSubjectResult = new StudentSubjectResult()
            {
                Subject = subject,
                Student = student,
                Result = resultsMapping[result],
                AdditionalResult = !string.IsNullOrWhiteSpace(additionalResult) ? resultsMapping[additionalResult] : null
            };

            student.Results.Add(studentSubjectResult);
            subject.Students.Add(student);
        }
    }
}
