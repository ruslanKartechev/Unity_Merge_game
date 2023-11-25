using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Dev
{
    public class EmptySceneLoaderCountdown : MonoBehaviour
    {
        [SerializeField] private string _sceneName;
        [SerializeField] private TextMeshProUGUI _text;

        private void Start()
        {
            StartCoroutine(Working());
        }

        private IEnumerator Working()
        {
            var time = 2f;
            var step = .5f;
            while (time > 0f)
            {
                Print($"Load in {time}");
                yield return new WaitForSeconds(step);
                time -= step;
            }
            SceneManager.LoadSceneAsync(_sceneName);
        }
        
        private void Print(string msg)
        {
            _text.text = msg;
        }

    }
}