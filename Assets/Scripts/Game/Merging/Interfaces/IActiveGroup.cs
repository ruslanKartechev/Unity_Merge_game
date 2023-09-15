﻿namespace Game.Merging
{
    public interface IActiveGroup
    {
        int RowsCount { get; }
        IActiveGroupRow GetRow(int index);
        void ClearCell(int x, int y);
    }
}