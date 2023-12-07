using UnityEngine;

namespace Creatives.Office
{
    [CreateAssetMenu(menuName = "SO/Creos/CreosSettings", fileName = "CreosSettings", order = 0)]
    public class CreosSettings : ScriptableObject
    {
        [Header("Office Animal")] 
        public LayerMask biteMask;
        public LayerMask failMask;
        public float moveSpeed;
        public float jumpSpeed;
        public string attackKey;
        public float biteCastRad;
        public float failCastRad;
        public AnimationCurve jumpCurve;
        public bool useFollower;

    }
}