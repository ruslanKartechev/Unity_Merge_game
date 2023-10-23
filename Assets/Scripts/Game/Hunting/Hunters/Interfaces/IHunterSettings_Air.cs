namespace Game.Hunting.Hunters.Interfaces
{
    public interface IHunterSettings_Air : IHunterSettings
    {
        float MinDistance();
        float ToBitePosFlyTime();
    }
}