﻿using System;
using UnityEngine;

namespace Common.UILayout
{
    public abstract class LayoutBase : MonoBehaviour
    {
        [SerializeField] protected float _setTime;
        public float SetTime => _setTime;
        
        public abstract void SetLayout(Action onDone, bool animated = true);
    }
}