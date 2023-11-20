using System.Collections.Generic;
using Game.Hunting.Hunters.Interfaces;
using Game.Hunting.Prey.Interfaces;
using UnityEngine;

namespace Game.Hunting.Hunters
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