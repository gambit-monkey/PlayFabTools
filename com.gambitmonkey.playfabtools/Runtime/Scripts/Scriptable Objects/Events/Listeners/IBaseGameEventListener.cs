namespace GambitMonkey.ScriptableObjects
{
    public interface IBaseGameEventListener<T>
    {
        void OnEventRaised(T item);
    }
}
