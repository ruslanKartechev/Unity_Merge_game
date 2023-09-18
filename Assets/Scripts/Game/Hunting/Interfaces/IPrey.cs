using System;
using UnityEngine;

namespace Game.Hunting
{
    public interface IPrey
    {
        event Action<IPrey> OnKilled;
        
        void Init();
        void Activate();
        
        Vector3 GetPosition();
        Quaternion GetRotation();
        float GetReward();
    }
}