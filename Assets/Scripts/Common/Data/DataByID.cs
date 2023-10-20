namespace Common.Data
{
    [System.Serializable]
    public class DataByID<T>
    {
        public string id;
        public T type;
    }
}