namespace RicoShot.Utils
{
    public interface IObjectPoolManager<out T> where T : PoolManagedMonoObject
    {
        public T Get();
        public void Release(PoolManagedMonoObject obj);
    }
}