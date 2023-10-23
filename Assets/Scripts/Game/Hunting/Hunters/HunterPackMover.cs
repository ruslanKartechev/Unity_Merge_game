using System.Collections.Generic;
using Game.Hunting.Hunters.Interfaces;
using UnityEngine;

namespace Game.Hunting
{
    [DefaultExecutionOrder(101)]
    public class HunterPackMover : MonoBehaviour
    {
        [SerializeField] private Transform _movable;
        private IPreyPack _prey;
        private Coroutine _moving;
        private IList<IHunter> _hunters;
        


    }
}