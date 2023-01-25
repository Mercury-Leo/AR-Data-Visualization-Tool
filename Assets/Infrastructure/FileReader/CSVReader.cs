using System.IO;
using Core.FileReader;

namespace Infrastructure.FileReader {
    public class CSVReader : IFileReader<string[]> {
        readonly string _filePath;

        public CSVReader(string filePath) {
            _filePath = filePath;
        }

        public string[] ReadFile() {
            return File.ReadAllLines(_filePath);
        }
    }
}