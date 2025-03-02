using Dapper;
using System.Data.SQLite;

namespace ResultsModels.SqlIo
{
    public class TableCreator
    {
        public static void CreateDatabase(string filePath)
        {
            SQLiteConnection.CreateFile(filePath);

            using var newDb = new SQLiteConnection($"Data Source={filePath}; Version = 3; New = True; Compress = True;");

            newDb.Open();

            var studentTable = @"
            CREATE TABLE ""Student"" (
	            ""StudentKey""	INTEGER NOT NULL UNIQUE,
	            ""FirstName""	TEXT NOT NULL,
	            ""LastName""	TEXT NOT NULL,
                ""Gender""      TEXT NOT NULL,
	            PRIMARY KEY(""StudentKey"" AUTOINCREMENT)
            );";

            var formTable = @"
            CREATE TABLE ""Form"" (
	            ""FormKey""	TEXT NOT NULL,
	            ""Name""	TEXT NOT NULL,
	            PRIMARY KEY(""FormKey"")
            );";

            var subjectTable = @"
            CREATE TABLE ""Subject"" (
	            ""SubjectKey""	INTEGER NOT NULL UNIQUE,
	            ""SubjectName""	TEXT NOT NULL UNIQUE,
	            ""Board""	TEXT NOT NULL,
	            ""UsesDoubleGrade""	INTEGER NOT NULL,
	            PRIMARY KEY(""SubjectKey"" AUTOINCREMENT)
            );";

            var resultTable = @"
            CREATE TABLE ""Result"" (
	            ""Grade""	TEXT NOT NULL,
	            ""Value""	REAL NOT NULL,
	            PRIMARY KEY(""Grade"")
            );";

            var studentSubjectResultTable = @"
            CREATE TABLE ""StudentSubjectResult"" (
	            ""StudentKey""	INTEGER NOT NULL,
	            ""SubjectKey""	INTEGER NOT NULL,
	            ""Result""	TEXT NOT NULL,
	            ""AdditionalResult""	TEXT,
	            PRIMARY KEY(""StudentKey"",""SubjectKey""),
	            FOREIGN KEY(""AdditionalResult"") REFERENCES ""ResultPoints""(""ResultType""),
	            FOREIGN KEY(""Result"") REFERENCES ""ResultPoints""(""ResultType""),
	            FOREIGN KEY(""StudentKey"") REFERENCES ""Student""(""StudentKey""),
	            FOREIGN KEY(""SubjectKey"") REFERENCES ""Subject""(""SubjectKey"")
            );";

            var formStudentTable = @"
            CREATE TABLE ""FormStudent"" (
	            ""FormKey""	TEXT NOT NULL,
	            ""StudentKey""	INTEGER NOT NULL,
	            PRIMARY KEY(""FormKey"",""StudentKey""),
	            FOREIGN KEY(""FormKey"") REFERENCES ""Form""(""FormKey""),
	            FOREIGN KEY(""StudentKey"") REFERENCES ""Student""(""StudentKey"")
            );";

            var studentAverageTable = @"
            CREATE TABLE ""StudentAverageGrade"" (
	            ""StudentKey""	INTEGER NOT NULL UNIQUE,
	            ""AverageGrade""    NUMERIC NOT NULL,
	            PRIMARY KEY(""StudentKey""),
	            FOREIGN KEY(""StudentKey"") REFERENCES ""Student""(""StudentKey"")
            );";

            var formAverageTable = @"
            CREATE TABLE ""FormAverageGrade"" (
	            ""FormKey""	TEXT NOT NULL UNIQUE,
	            ""AverageGrade""	REAL NOT NULL,
                ""MaleAverageGrade""	REAL NOT NULL,
                ""FemaleAverageGrade""	REAL NOT NULL,
	            PRIMARY KEY(""FormKey"")
            );";

            var subjectAverageTable = @"
            CREATE TABLE ""SubjectAverageGrade"" (
	            ""SubjectKey""	INTEGER NOT NULL UNIQUE,
	            ""AverageGrade""	REAL NOT NULL,
                ""MaleAverageGrade""	REAL NOT NULL,
                ""FemaleAverageGrade""	REAL NOT NULL,
	            PRIMARY KEY(""SubjectKey""),
	            FOREIGN KEY(""SubjectKey"") REFERENCES ""Subject""(""SubjectKey"")
            );
            ";

            var subjectGradeCountTable = @"
            CREATE TABLE ""SubjectGradeCount"" (
	            ""SubjectKey""	INTEGER NOT NULL,
	            ""Grade""	TEXT NOT NULL,
	            ""Count""	INTEGER NOT NULL,
	            PRIMARY KEY(""SubjectKey"",""Grade""),
	            FOREIGN KEY(""SubjectKey"") REFERENCES ""Subject""(""SubjectKey"")
            );";



            newDb.Execute(studentTable);
            newDb.Execute(formTable);
            newDb.Execute(subjectTable);
            newDb.Execute(resultTable);
            newDb.Execute(studentSubjectResultTable);
            newDb.Execute(formStudentTable);
            newDb.Execute(studentAverageTable);
            newDb.Execute(formAverageTable);
            newDb.Execute(subjectAverageTable);
            newDb.Execute(subjectGradeCountTable);

            newDb.Shutdown();
        }
    }
}
