using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Creatives
{
    public class EditorSceneRestarter : MonoBehaviour
    {
        private void Start()
        {
            Application.targetFrameRate = 60;
            StartCoroutine(Working());
        }

        private IEnumerator Working()
        {
            while (true)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Debug.Break();
                }

                yield return null;
            }
        }
    }
}