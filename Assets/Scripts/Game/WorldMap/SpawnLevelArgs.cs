namespace Game.WorldMap
{
    public struct SpawnLevelArgs
    {
        public SpawnLevelArgs(int index, bool dead)
        {
            Index = index;
            Dead = dead;
        }

        public int Index;
        public bool Dead;
        
    }
}