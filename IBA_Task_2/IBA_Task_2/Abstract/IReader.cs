namespace IBA_Task_1.Abstract
{
    public interface IReader<T> where T : class
    {
        T ReadFromeFile(string path);
    }
}