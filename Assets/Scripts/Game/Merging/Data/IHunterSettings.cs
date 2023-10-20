
namespace Game.Merging
{
    public interface IHunterSettings
    {
        float Damage { get; }
        float JumpSpeed { get; }
        public float Radius { get; }
    }

    public interface IAirHunterSettings : IHunterSettings
    {
        float MinDistance();
        float ToBitePosFlyTime();
    }
    
    public interface IFishSettings : IHunterSettings
    {
    }
}