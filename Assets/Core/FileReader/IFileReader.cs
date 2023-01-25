namespace Core.FileReader {
    public interface IFileReader<out T> {
        public T ReadFile();
    }
}