
namespace Game.Hunting
{
    public interface IHealthListener
    {
        void OnHealthChange(float health, float maxHealth);
    }
}