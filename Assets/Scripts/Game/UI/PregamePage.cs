#define SDK__
#if SDK
using MAXHelper;
#endif
using UnityEngine;
using UnityEngine.Events;

namespace Game.UI
{
    public class PregamePage : MonoBehaviour
    {
#if SDK
        [SerializeField] private TermsAndATT _terms;
        [SerializeField] private UITermsPanel _termsPanel;
 #endif      
        [SerializeField] private PregameCheat _cheat;

        public void ShowWithTermsPanel(UnityAction onPlay)
        {
            _cheat.Hide();
            gameObject.SetActive(true);
#if SDK
            _terms.EventOnTermsAccepted += onPlay;
            _terms.BeginPlay();
#endif
        }

        public void Hide()
        {
            Debug.Log("PREGAME CANVAS HIDE");
            gameObject.SetActive(false);
#if SDK
            if(_termsPanel != null)
                _termsPanel.gameObject.SetActive(false);
#endif
        }

        public void ShowCheat(UnityAction onClose)
        {
            gameObject.SetActive(true);
#if SDK
            if(_termsPanel != null)
                _termsPanel.gameObject.SetActive(false);
     #endif  
            _cheat.Show(onClose);
        }

        public void ShowDarkening()
        {
            _cheat.Hide();
            gameObject.SetActive(true);
#if SDK
            if(_termsPanel != null)
                _termsPanel.gameObject.SetActive(false);
#endif
        }
    }
}