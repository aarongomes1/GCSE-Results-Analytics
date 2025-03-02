using Dapper;
using ResultsModels.SqlIo.Models;
using System.Data.SQLite;

namespace ResultsModels.SqlIo
{
    internal class Loader
    {
        public static Structure.Results Load(string filePath)
        {
            using var sqlConnection = new SQLiteConnection($"Data Source={filePath}; Version = 3; New = False; Compress = True;");
            sqlConnection.Open();

            var students = sqlConnection.Query<Student>(FormQueryString("Student")).Select(x => new Structure.Student()
            {
                StudentKey = x.StudentKey,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Gender = x.Gender,
            }).ToList();

            var subjects = sqlConnection.Query<Subject>(FormQueryString("Subject")).Select(x => new Structure.Subject()
            {
                SubjectKey = x.SubjectKey,
                SubjectName = x.SubjectName,
                Board = x.Board,
                UsesDoubleGrade = x.UsesDoubleGrade == 1,
            }).ToList();

            var forms = sqlConnection.Query<Form>(FormQueryString("Form")).Select(x => new Structure.Form()
            {
                FormKey = Guid.Parse(x.FormKey),
                Name = x.Name,
            }).ToList();

            var resultValues = sqlConnection.Query<Structure.Result>(FormQueryString("Result")).ToList();

            var formStudents = sqlConnection.Query<FormStudent>(FormQueryString("FormStudent")).ToList();
            var studentSubjectResults = sqlConnection.Query<StudentSubjectResult>(FormQueryString("StudentSubjectResult")).ToList();

            MakeFormStudentConnection(forms, students, formStudents);
            MakeStudentSubjectResultsConnection(subjects, students, resultValues, studentSubjectResults);

            var formAverageGrades = sqlConnection.Query<FormAverageGrade>(FormQueryString("FormAverageGrade")).ToList();
            var studentAverageGrades = sqlConnection.Query<StudentAverageGrade>(FormQueryString("StudentAverageGrade")).ToList();
            var subjectAverageGrades = sqlConnection.Query<SubjectAverageGrade>(FormQueryString("SubjectAverageGrade")).ToList();
            var subjectGradeCounts = sqlConnection.Query<SubjectGradeCount>(FormQueryString("SubjectGradeCount")).ToList();

            AssignFormAverage(forms, formAverageGrades);
            AssignStudentAverage(students, studentAverageGrades);
            AssignSubjectAverage(subjects, subjectAverageGrades);
            AssignSubjectGradeCount(subjects, subjectGradeCounts);

            var results = new Structure.Results(students, subjects, forms, resultValues);

            return results;
        }

        private static void MakeFormStudentConnection(List<Structure.Form> forms, List<Structure.Student> students, List<FormStudent> formStudents)
        {
            foreach (var formStudent in formStudents)
            {
                var formKey = Guid.Parse(formStudent.FormKey);

                var student = students.Single(x => x.StudentKey == formStudent.StudentKey);
                var form = forms.Single(x => x.FormKey == formKey);

                student.Form = form;
                form.Students.Add(student);
            }
        }

        private static void MakeStudentSubjectResultsConnection(List<Structure.Subject> subjects, List<Structure.Student> students, List<Structure.Result> resultPoints, List<StudentSubjectResult> grades)
        {
            foreach (var grade in grades)
            {
                var student = students.Single(x => x.StudentKey == grade.StudentKey);
                var subject = subjects.Single(x => x.SubjectKey == grade.SubjectKey);
                var result = resultPoints.Single(x => x.Grade.Equals(grade.Result));
                var additionalResult = resultPoints.SingleOrDefault(x => x.Grade.Equals(grade.AdditionalResult));

                var subjectStudentObj = new Structure.StudentSubjectResult()
                {
                    Student = student,
                    Subject = subject,
                    Result = result,
                    AdditionalResult = additionalResult,
                };

                student.Results.Add(subjectStudentObj);
                subject.Students.Add(student);
            }
        }

        private static void AssignFormAverage(List<Structure.Form> forms, List<FormAverageGrade> formAverageGrades)
        {
            foreach (var formAverageGrade in formAverageGrades)
            {
                var formKey = Guid.Parse(formAverageGrade.FormKey);

                var form = forms.Single(x => x.FormKey == formKey);

                form.MaleAverageGrade = new Structure.Average() { Name = "Male", Value = formAverageGrade.MaleAverageGrade };
                form.FemaleAverageGrade = new Structure.Average() { Name = "Female", Value = formAverageGrade.FemaleAverageGrade};
                form.AverageGrade = new Structure.Average() { Name = "Form Average", Value = formAverageGrade.AverageGrade };
            }
        }

        private static void AssignStudentAverage(List<Structure.Student> students, List<StudentAverageGrade> studentAverageGrades)
        {
            foreach (var studentAverageGrade in studentAverageGrades)
            {
                var student = students.Single(x => x.StudentKey == studentAverageGrade.StudentKey);
                student.AverageGrade = new Structure.Average() { Name = "Student Average", Value = studentAverageGrade.AverageGrade };
            }
        }

        private static void AssignSubjectAverage(List<Structure.Subject> subjects, List<SubjectAverageGrade> subjectAverageGrades)
        {
            foreach (var subjectAverageGrade in subjectAverageGrades)
            {
                var subject = subjects.Single(x => x.SubjectKey == subjectAverageGrade.SubjectKey);
                subject.AverageGrade = new Structure.Average() { Name = "Subject Average", Value = subjectAverageGrade.AverageGrade };
                subject.MaleAverageGrade = new Structure.Average() { Name = "Male", Value = subjectAverageGrade.MaleAverageGrade };
                subject.FemaleAverageGrade = new Structure.Average() { Name = "Female", Value = subjectAverageGrade.FemaleAverageGrade };
            }
        }

        private static void AssignSubjectGradeCount(List<Structure.Subject> subjects, List<SubjectGradeCount> subjectGradeCounts)
        {
            foreach (var subjectGradeCount in subjectGradeCounts)
            {
                var subject = subjects.Single(x => x.SubjectKey == subjectGradeCount.SubjectKey);

                var gradeCount = new Structure.GradeCount()
                {
                    Grade = subjectGradeCount.Grade,
                    Count = subjectGradeCount.Count,
                };

                subject.GradeCounts.Add(gradeCount);
            }
        }

        private static string FormQueryString(string tableName)
        {
            var query = $"SELECT * FROM {tableName}";
            return query;
        }
    }
}