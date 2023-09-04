using TMPro;
using UnityEngine;

namespace Game.UI.Elements
{
    public class LevelDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _levelText;

        public void SetLevel(int level)
        {
            _levelText.text = $"LEVEL {level}";
        }
    }
}