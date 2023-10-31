using System.Collections.Generic;
using UnityEngine;

namespace Game.WorldMap
{
    public class FogManager : MonoBehaviour
    {
        [SerializeField] private List<Renderer> _fogParts;
        [SerializeField] private Material _fogMaterial;
        [SerializeField] private float _yCoord;

        public List<Renderer> Parts
        {
            get => _fogParts;
            set => _fogParts = value;
        }

        public Material FogMaterial => _fogMaterial;


        [ContextMenu("Show All")]
        public void ShowAll()
        {
            foreach (var part in _fogParts)
            {
                if (part == null)
                    return;
                part.gameObject.SetActive(true);
            }
        }

        [ContextMenu("Hide All")]
        public void HideAll()
        {
            foreach (var part in _fogParts)
            {
                if (part == null)
                    return;
                part.gameObject.SetActive(false);
            }
        }

        [ContextMenu("Set Material To All")]
        public void SetMaterialToAll()
        {
            foreach (var part in _fogParts)
            {
                if (part == null)
                    return;
                part.sharedMaterial = _fogMaterial;
            }
        }


        [ContextMenu("Set local_Y")]
        public void SetYToAll()
        {
            foreach (var part in Parts)
            {
                var pos = part.transform.localPosition;
                pos.y = _yCoord;
                part.transform.localPosition = pos;
            }
        }



    }
}