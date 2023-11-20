using TMPro;
using UnityEngine;

namespace Game.UI.Hunting
{
    public class KillCountDisplayUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        
        public void SetKillCount(int killed, int target)
        {
            _text.text = $"{killed}/{target}";
        }
    }
}