using UnityEngine;

namespace Game.Hunting
{
    public class HunterMouthTester : MonoBehaviour
    {
        public HunterMouth mouth;
        public Transform parent;
        public Transform refPoint;

        
        
        [ContextMenu("Activate")]
        public void Activate()
        {
            mouth.BiteTo(parent, refPoint);
        }
    }
}