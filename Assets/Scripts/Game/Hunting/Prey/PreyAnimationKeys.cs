using System.Collections.Generic;
using UnityEngine;

namespace Game.Hunting
{
    [CreateAssetMenu(menuName = "SO/" + nameof(PreyAnimationKeys), fileName = nameof(PreyAnimationKeys), order = 0)]
    public class PreyAnimationKeys : ScriptableObject, IPreyAnimationKeys
    {
        [SerializeField] private List<string> _idleAnimations;
        [SerializeField] private List<RuntimeAnimatorController> _runOverrideControllers;
        [SerializeField] private List<string> _scaredAnimations;
        [SerializeField] private List<string> _winAnimations;
        [SerializeField] private string _runTriggerKey;
        [SerializeField] private string _treeCutAnimation;
        [SerializeField] private string _barrelIdleAnimation;
        [SerializeField] private string _barrelThrowAnimation;
        [SerializeField] private string _grabbedInAirAnimation;

        public IList<string> IdleAnimKeys => _idleAnimations;
        public IList<string> ScaredAnimKeys => _scaredAnimations;
        public IList<string> WinAnimKeys => _winAnimations;
        
        public string TreeCutAnimKey => _treeCutAnimation;
        public string BarrelIdleAnimKey => _barrelIdleAnimation;
        public string BarrelThrowAnimKey => _barrelThrowAnimation;
        public string RunTriggerKey => _runTriggerKey;
        public IList<RuntimeAnimatorController> RunOverrideControllers => _runOverrideControllers;
        public string GrabbedInAir => "Fly";
        public string FallFromAir => "Fall";
    }
}