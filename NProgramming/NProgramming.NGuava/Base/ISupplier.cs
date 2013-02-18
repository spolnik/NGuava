namespace NProgramming.NGuava.Base
{
    public interface ISupplier<out T>
    {
        T Get();
    }
}