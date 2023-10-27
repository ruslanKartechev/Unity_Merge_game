﻿using System;
using TMPro;
using UnityEngine;

namespace Game.WorldMap
{
    public class WorldMapLevelNumber : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _text;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Color _colorPlayer;
        [SerializeField] private Color _colorEnemy;

        private void OnValidate()
        {
            // transform.localEulerAngles = new Vector3(90, 180,0);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void SetLevel(int level)
        {
            _text.text = level.ToString();
        }

        public void SetEnemy()
        {
            _spriteRenderer.color = _colorEnemy;
        }

        public void SetPlayer()
        {
            _spriteRenderer.color = _colorPlayer;
        }
    }
}