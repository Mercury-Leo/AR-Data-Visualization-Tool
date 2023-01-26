namespace Core.DataProcessor {
    public interface IDataProcessor<in T, out TV> {
        public TV ProcessData(T data);
    }
}