namespace Core.FileReader {
    public interface IFileReader<out T, in TV> {
        public T ReadFile(TV file);
    }
}