using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Core.Extensions {
    public static class FileExtensions {
        public static IEnumerable<string> GetAllFiles(string folderPath) {
            return Directory.Exists(folderPath) ? Directory.GetFiles(folderPath) : null;
        }

        public static string GetFileName(string filePath) {
            return File.Exists(filePath) ? Path.GetFileName(filePath) : string.Empty;
        }

        public static bool CheckIfValidFileExtension(string filePath, params string[] extensions) {
            return File.Exists(filePath) && extensions.Contains(Path.GetExtension(filePath));
        }
    }
}