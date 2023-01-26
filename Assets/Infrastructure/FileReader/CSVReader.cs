using System.IO;
using Core.FileReader;

namespace Infrastructure.FileReader {
    public class CSVReader : IFileReader<string[], string> {
        public string[] ReadFile(string filePath) {
            return File.ReadAllLines(filePath);
        }
    }
}