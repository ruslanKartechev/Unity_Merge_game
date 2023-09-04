using System.Collections.Generic;
using Common.Utils;
using UnityEngine;

namespace Common.Ragdoll
{
    public partial class Ragdoll : IRagdoll
    {
        public List<RagdollPart> parts;
        
        public override bool IsActive { get; protected set; }
        public override void Activate()
        {
            #if UNITY_EDITOR
            if (Application.isPlaying == false)
            {
                foreach (var part in parts)
                    part.On();
                return;
            }
            #endif
            if (IsActive)
                return;
            IsActive = true;
            foreach (var part in parts)
            {
                part.On();
            }   
        }

        public override void Deactivate()
        {
#if UNITY_EDITOR
            if (Application.isPlaying == false)
            {
                foreach (var part in parts)
                    part.Off();
                return;
            }
#endif
            if (!IsActive)
                return;
            IsActive = false;
            foreach (var part in parts)
            {
                part.Off();
            }   
        }

        public override void ActivateAndPush(Vector3 force)
        {
            var pushable = new List<RagdollPart>();
            IsActive = true;
            foreach (var part in parts)
            {
                part.On();
                // part.rb.velocity = Vector3.zero;
                if(part.push)
                    pushable.Add(part);
            }
            foreach (var part in pushable)
            {
                part.Push(force);
            }
        }

        public RagdollPart GetClosestPart(Vector3 position)
        {
            var shortest = float.MaxValue;
            RagdollPart result = null;
            foreach (var part in parts)
            {
                var d = (position - part.rb.position).sqrMagnitude;
                if (d < shortest)
                {
                    shortest = d;
                    result = part;
                }
            }
            return result;
        }
        
#if UNITY_EDITOR
        public void GetAllParts()
        {
            List<Transform> gos = HierarchyUtils.GetFromAllChildren<Transform>(transform, (go) =>
            {
                if (go == transform)
                    return false;
                var coll = go.GetComponent<Collider>();
                var rb = go.GetComponent<Rigidbody>();
                if (rb != null)
                    return true;
                return false;
            });
            parts = new List<RagdollPart>();
            foreach (var go in gos)
            {
                var part = new RagdollPart()
                {
                    rb = go.GetComponent<Rigidbody>(),
                    collider =  go.GetComponent<Collider>(),
                    name = go.name
                };
                if (part.collider == null)
                {
                    Debug.Log($"!!!!!! COLLIDER IS NULL on : {part.name}");
                }
                parts.Add(part);
            }
        }
        
        public void SetAllInterpolate()
        {
            foreach (var part in parts)
                part.rb.interpolation = RigidbodyInterpolation.Interpolate;
        }
        
        public void SetAllExtrapolate()
        {
            foreach (var part in parts)
                part.rb.interpolation = RigidbodyInterpolation.Extrapolate;
        }

        public void SetAllNoInterpolate()
        {
            foreach (var part in parts)
            {
                part.rb.interpolation = RigidbodyInterpolation.None;
            }
        }
        
        public void SetProjection()
        {
            foreach (var part in parts)
            {
                var joint = part.rb.GetComponent<CharacterJoint>();
                if(joint == null)
                    continue;
                joint.enableProjection = true;
            }
        }

        public void SetNoProjection()
        {
            foreach (var part in parts)
            {
                var joint = part.rb.GetComponent<CharacterJoint>();
                if(joint == null)
                    continue;
                joint.enableProjection = false;
            }
        }
        
        public void SetAllPreprocess(bool preprocess)
        {
            foreach (var part in parts)
            {
                var joint = part.rb.gameObject.GetComponent<Joint>();
                if(joint != null)
                    joint.enablePreprocessing = preprocess;
            }
        }
#endif

      
    }
}