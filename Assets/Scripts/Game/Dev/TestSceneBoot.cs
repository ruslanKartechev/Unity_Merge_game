using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Game.Dev
{
    public class TestSceneBoot : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        private void Start()
        {
            StartCoroutine(Working());
        }

        private IEnumerator Working()
        {
            var delay = 1f;
            Print("Started scene");
            yield return new WaitForSeconds(delay);
            Print("Something 1");
            yield return new WaitForSeconds(delay);
            Print("Something 2");
            yield return new WaitForSeconds(delay);
            Print("Something 3");
            yield return new WaitForSeconds(delay);
            Print("Init framerate");
            Application.targetFrameRate = 60;
            yield return new WaitForSeconds(delay);
            
            
            Print("End");
        }

        private void Print(string msg)
        {
            _text.text = msg;
        }

    }
}