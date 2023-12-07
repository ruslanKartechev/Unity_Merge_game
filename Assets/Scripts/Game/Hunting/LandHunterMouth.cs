﻿using System;
using Game.Hunting.Hunters;
using UnityEngine;

namespace Game.Hunting
{
    public class LandHunterMouth : HunterMouth
    {
        [SerializeField] private Joint _headJoint;
        [SerializeField] private Rigidbody _headRb;
        [Space(10)]
        [SerializeField] private RagdollPositioner _ragdollPositioner;

        private void Start()
        {
            gameObject.SetActive(false);
        }

        public override void BiteTo(Transform parent, Vector3 position)
        {
            gameObject.SetActive(true);
            var joint = transform;
            var rotVec = (parent.position - position);
            var joint_pos = position;
            var joint_rot = Quaternion.LookRotation(rotVec);
            joint.SetPositionAndRotation(joint_pos, joint_rot);
            joint.SetParent(parent);
            _headJoint.connectedBody = _headRb;
            _ragdollPositioner?.SetPosition();
        }

    }
    
}