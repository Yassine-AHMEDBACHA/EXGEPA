namespace CORESI.Tools.Communication
{
    public interface IBus
    {
        void RaiseEvent(object sender, object args);

        void Subscribe(object sender, object args);

        void Unsubscribe(object sender, object args);
    }
}
