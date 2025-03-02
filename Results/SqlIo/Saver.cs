using ResultsModels.SqlIo.Models;
using System.Data.SQLite;
using Z.Dapper.Plus;

namespace ResultsModels.SqlIo
{
    public class Saver
    {
        public static void Save(string filePath, Structure.Results results)
        {
            using var sqlConnection = new SQLiteConnection($"Data Source={filePath}; Version = 3; New = False; Compress = True;");
            sqlConnection.Open();

            var forms = results.Forms.Select(x => new Form() { FormKey = x.FormKey.ToString(), Name = x.Name });
            var students = results.Students.Select(x => new Student() { StudentKey = x.StudentKey, FirstName = x.FirstName, LastName = x.LastName, Gender = x.Gender }).DistinctBy(x => x.StudentKey);
            var subjects = results.Subjects.Select(x => new Subject() { SubjectKey = x.SubjectKey, SubjectName = x.SubjectName, Board = x.Board, UsesDoubleGrade = x.UsesDoubleGrade ? 1 : 0 });
            var studentSubjectResults = results.Students.SelectMany(x => x.Results.Select(y => new StudentSubjectResult() { SubjectKey = y.Subject.SubjectKey, StudentKey = x.StudentKey, Result = y.Result.Grade, AdditionalResult = y.AdditionalResult?.Grade }));
            var formStudents = results.Students.Select(x => new FormStudent() { FormKey = x.Form!.FormKey.ToString(), StudentKey = x.StudentKey });

            var formGradeAverages = results.Forms.Where(x => x.AverageGrade is not null).Select(x => new FormAverageGrade() { FormKey = x.FormKey.ToString(), AverageGrade = x.AverageGrade!.Value, FemaleAverageGrade = x.FemaleAverageGrade!.Value, MaleAverageGrade = x.MaleAverageGrade!.Value });
            var studentGradeAverages = results.Students.Where(x => x.AverageGrade is not null).Select(x => new StudentAverageGrade() { StudentKey = x.StudentKey, AverageGrade = x.AverageGrade!.Value });
            var subjectGradeAverages = results.Subjects.Where(x => x.AverageGrade is not null).Select(x => new SubjectAverageGrade() { SubjectKey = x.SubjectKey, AverageGrade = x.AverageGrade!.Value, FemaleAverageGrade = x.FemaleAverageGrade!.Value, MaleAverageGrade = x.MaleAverageGrade!.Value });
            var subjectGradeCounts = results.Subjects.SelectMany(x => x.GradeCounts.Select(y => new SubjectGradeCount() { SubjectKey = x.SubjectKey, Grade = y.Grade, Count = y.Count }));

            DapperPlusManager.Entity<Subject>("Subject").Table("Subject").KeepIdentity(true).InsertIfNotExists(true).Identity(x => x.SubjectKey);
            DapperPlusManager.Entity<Student>("Student").Table("Student").KeepIdentity(true).InsertIfNotExists(true).Identity(x => x.StudentKey);
            DapperPlusManager.Entity<Form>("Form").Table("Form").InsertIfNotExists(true).KeepIdentity(true).Identity(x => x.FormKey);
            DapperPlusManager.Entity<FormStudent>("FormStudent").Table("FormStudent").InsertIfNotExists(true).KeepIdentity(true);
            DapperPlusManager.Entity<StudentSubjectResult>("StudentSubjectResult").Table("StudentSubjectResult").InsertIfNotExists(true).KeepIdentity(true);
            DapperPlusManager.Entity<Structure.Result>("Result").Table("Result").InsertIfNotExists(true).KeepIdentity(true);
            DapperPlusManager.Entity<StudentSubjectResult>("StudentAverageGrade").Table("StudentAverageGrade").InsertIfNotExists(true).KeepIdentity(true);
            DapperPlusManager.Entity<StudentSubjectResult>("FormAverageGrade").Table("FormAverageGrade").InsertIfNotExists(true).KeepIdentity(true);
            DapperPlusManager.Entity<StudentSubjectResult>("SubjectAverageGrade").Table("SubjectAverageGrade").InsertIfNotExists(true).KeepIdentity(true);
            DapperPlusManager.Entity<StudentSubjectResult>("SubjectGradeCount").Table("SubjectGradeCount").InsertIfNotExists(true).KeepIdentity(true);


            sqlConnection.BulkInsert("Form", forms);
            sqlConnection.BulkInsert("Student", students);
            sqlConnection.BulkInsert("Subject", subjects);
            sqlConnection.BulkInsert("StudentSubjectResult", studentSubjectResults);
            sqlConnection.BulkInsert("FormStudent", formStudents);
            sqlConnection.BulkInsert("StudentAverageGrade", studentGradeAverages);
            sqlConnection.BulkInsert("Result", results.ResultValues);
            sqlConnection.BulkInsert("FormAverageGrade", formGradeAverages);
            sqlConnection.BulkInsert("SubjectAverageGrade", subjectGradeAverages);
            sqlConnection.BulkInsert("SubjectGradeCount", subjectGradeCounts);
        }
    }
}
