using GCSEResultsFileConverter.Models;
using ResultsModels.IO;
using ResultsModels.SqlIo;
using ResultsModels.Structure;

namespace GCSEResultsFileConverter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Expected 3 parameters:");
                Console.WriteLine("1) Path to the results file");
                Console.WriteLine("2) Path to the results points file");
                Console.WriteLine("3) Path to the output folder");
                return;
            }

            var inputFilePath = args[0];
            var resultPointFilePath = args[1];
            var outputFolderPath = args[2];

            var inputRecords = CsvIo.ReadRecords<ResultsModel>(inputFilePath);
            var resultPoints = CsvIo.ReadRecords<Result>(resultPointFilePath);

            var results = ResultsConverter.Convert(inputRecords, resultPoints);

            TableCreator.CreateDatabase(outputFolderPath);

            results.Save(outputFolderPath);
        }
    }
}
