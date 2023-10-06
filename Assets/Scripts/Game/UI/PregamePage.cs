using System;
using MAXHelper;
using UnityEngine;
using UnityEngine.Events;

namespace Game.UI
{
    public class PregamePage : MonoBehaviour
    {
        [SerializeField] private UITermsPanel _termsPanel;

        public void ShowWithTermsPanel(UnityAction onPlay)
        {
            gameObject.SetActive(true);
            _termsPanel.EventOnAcceptClick += onPlay;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}