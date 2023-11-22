using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.WorldMap
{
    public class WorldMapEnemyProps : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _animatedParts;

        
        #if UNITY_EDITOR
        public bool autoGetParts = true;
        private void OnValidate()
        {
            // if(getParts)
            //  SetParts();   
        }
        
        [ContextMenu("SetParts")]
        public void SetParts()
        {
            var count = transform.childCount;
            for (var i = 0; i < count; i++)
            {
                var go = transform.GetChild(i).gameObject;
                if(_animatedParts.Contains(go) == false)
                    _animatedParts.Add(go);
                if(go.GetComponent<IMapAnimatedPiece>() != null)
                    continue;
                if (go.name.Contains("Tree"))
                {
                    var scr = go.AddComponent<MapAnimatedTree>();
                }
                else
                {
                    var scr = go.AddComponent<MapAnimatedProp>();
                    
                }
            }
        }
#endif
        
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public IEnumerator AnimatingDown(float duration)
        {
            var pieces = new List<IMapAnimatedPiece>(_animatedParts.Count);
            foreach (var go in _animatedParts)
            {
                var piece = go.GetComponent<IMapAnimatedPiece>();
                piece.Prepare();
                pieces.Add(piece);
            }
            var elapsed = 0f;
            while (elapsed <= duration)
            {
                var t = elapsed / duration;
                foreach (var piece in pieces)
                    piece.Animate(t);
                elapsed += Time.deltaTime;
                yield return null;
            }
            foreach (var piece in pieces)
                piece.Animate(1f);
        }
    }
}