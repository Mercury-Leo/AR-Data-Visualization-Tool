using Core.DataProcessor;
using Core.FileReader;

namespace Infrastructure.DataProcessor {
    public class CSVDataProcessor : IDataProcessor<string, string[,]> {
        readonly IFileReader<string[], string> _fileReader;
        string[,] _fileData;

        const char CSVSplit = ',';

        public CSVDataProcessor(IFileReader<string[], string> fileReader) {
            _fileReader = fileReader;
        }

        public string[,] ProcessData(string file) {
            var lines = _fileReader.ReadFile(file);

            var rows = lines.Length;
            var columns = lines[0].Split(CSVSplit).Length;

            _fileData = new string[rows, columns];

            for (var i = 0; i < rows; i++) {
                var cols = lines[i].Split(CSVSplit);
                for (var j = 0; j < columns; j++) {
                    if (string.IsNullOrWhiteSpace(cols[j]))
                        continue;
                    _fileData[i, j] = cols[j];
                }
            }

            return _fileData;
        }
    }
}