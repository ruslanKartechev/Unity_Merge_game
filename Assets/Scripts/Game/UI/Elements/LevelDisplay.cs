using Game.Core;
using TMPro;
using UnityEngine;

namespace Game.UI.Elements
{
    [DefaultExecutionOrder(0)]
    public class LevelDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _levelText;

        public void SetCurrent()
        {
            SetLevel(GC.PlayerData.LevelTotal + 1);
        }
        
        public void SetLevel(int level)
        {
            _levelText.text = $"LEVEL {level}";
        }
    }
}