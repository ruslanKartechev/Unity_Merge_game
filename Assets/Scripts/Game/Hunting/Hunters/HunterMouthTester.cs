using System.Collections;
using Common.Ragdoll;
using UnityEngine;

namespace Game.Hunting.Hunters
{
    public class HunterMouthTester : MonoBehaviour
    {
        public HunterMouth mouth;
        public Ragdoll ragdoll;
        public Animator anim;
        [Space(10)]
        public Transform parent;
        public Transform refPoint;
        
        
        [ContextMenu("Activate")]
        public void Activate()
        {
            StartCoroutine(Activating());
        }

        private IEnumerator Activating()
        {
            if(anim!= null)
                anim.enabled = false;
            if(ragdoll != null)
                ragdoll.Activate();
            
            mouth.BiteTo( anim.transform.parent, parent, refPoint, transform.position);   
            yield return null;
        }
    }
}