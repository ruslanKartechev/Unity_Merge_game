namespace Creatives.Kong
{
    public interface IKongPushTarget
    {
        void Push();
        bool Animated { get; }
    }
}