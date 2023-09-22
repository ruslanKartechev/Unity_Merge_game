﻿using UnityEngine;

namespace Game.Merging
{
    [CreateAssetMenu(menuName = "SO/" + nameof(InputSettings), fileName = nameof(InputSettings), order = 0)]
    public class InputSettings : ScriptableObject
    {
        [SerializeField] private float _sensitivity = 12;
        
        public LayerMask mergingMask;
        public LayerMask groundMask;
        public float inflectionOffset = 0.66f;
        public float maxAimDistance = 6;
        public float draggingUpOffset = .3f;
        public Vector2 verticalMinMax;


        public float Sensitivity
        {
            get
            {
                #if UNITY_EDITOR
                return _sensitivity * 0.6f;
                #endif
                return _sensitivity;
            }
            set => _sensitivity = value;
        }
    }


}