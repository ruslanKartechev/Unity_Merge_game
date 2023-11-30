using Utils;

namespace Game.Hunting
{
    public static class SlowMoPicker
    {
        public static bool UseSlowMo(IHunter hunter, IPreyPack preyPack)
        {
            // CLog.LogRed($"Prey count: {preyPack.PreyCount}");
            return preyPack.PreyCount == 1;
        }
    }
}