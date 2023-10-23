
namespace Game.Hunting
{
    public interface IPreyHealth : IPredatorTarget
    {
        void Init(float maxHealth);
        void Show();
        void Hide();
        void AddListener(IHealthListener listener);
    }
}