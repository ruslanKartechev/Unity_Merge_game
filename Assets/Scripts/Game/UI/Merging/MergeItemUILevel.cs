using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI.Merging
{
    public class MergeItemUILevel : MonoBehaviour
    {
        private const float PunchTime = .25f;
        private const float PunchScale = .12f;
        
        [SerializeField] private GameObject _block;
        [SerializeField] private RectTransform _parent;
        private List<Image> _spawned = new List<Image>();

        public List<Image> SpawnedStars => _spawned;

        public void SetLevel(int level)
        {
            foreach (var spawned in _spawned)
                spawned.gameObject.SetActive(false);
            SpawnIcons(level);
            OrderIcons(level);
        }

        private void OrderIcons(int level)
        {
            var spacing = GC.ItemViews.LevelIconsSpacing();
            var count = level;
            var farLeft = 0f;
            if (count % 2 == 0)
                farLeft = -(count / 2) * spacing + spacing / 2f;
            else
                farLeft = -(count / 2) * spacing;
            for (var x = 0; x < count; x++)
            {
                _spawned[x].gameObject.SetActive(true);
                _spawned[x].rectTransform.anchoredPosition = new Vector2(farLeft + x * spacing, 0);
            }
        }

        private void SpawnIcons(int count)
        {
            var toSpawn = count - _spawned.Count;
            var prefab = GC.ItemViews.GetLevelIconPrefab();
            for (var i = 0; i < toSpawn; i++)
            {
                var instance = Instantiate(prefab, _parent);
                _spawned.Add(instance.GetComponent<Image>());
            }
        }

        public void Show()
        {
            _block.SetActive(true);
        }

        public void Hide()
        {
            _block.SetActive(false);
        }

        public void PlayScale()
        {
            transform.localScale = Vector3.one;
            transform.DOPunchScale(PunchScale * Vector3.one, PunchTime);
        }
    }
}