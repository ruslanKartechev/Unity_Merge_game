using System;
using MAXHelper;
using UnityEngine;
using UnityEngine.Events;

namespace Game.UI
{
    public class PregamePage : MonoBehaviour
    {
        [SerializeField] private TermsAndATT _terms;
        [SerializeField] private UITermsPanel _termsPanel;
        [SerializeField] private PregameCheat _cheat;

        public void ShowWithTermsPanel(UnityAction onPlay)
        {
            _cheat.Hide();
            gameObject.SetActive(true);
            _terms.EventOnTermsAccepted += onPlay;
            _terms.BeginPlay();
        }

        public void Hide()
        {
            Debug.Log("PREGAME CANVAS HIDE");
            gameObject.SetActive(false);
            if(_termsPanel != null)
                _termsPanel.gameObject.SetActive(false);
        }

        public void ShowCheat(UnityAction onClose)
        {
            gameObject.SetActive(true);
            if(_termsPanel != null)
                _termsPanel.gameObject.SetActive(false);
            _cheat.Show(onClose);
        }

        public void ShowDarkening()
        {
            _cheat.Hide();
            gameObject.SetActive(true);
            if(_termsPanel != null)
                _termsPanel.gameObject.SetActive(false);
        }
    }
}