using UnityEngine;

namespace Creatives.Office
{
    [System.Serializable]
    public struct HumanAnim
    {
        public string name;
        public string boolVal;

        public void Play(Animator animator)
        {
            animator.SetBool(boolVal, true);
            animator.Play(name);            
        }
    }
}