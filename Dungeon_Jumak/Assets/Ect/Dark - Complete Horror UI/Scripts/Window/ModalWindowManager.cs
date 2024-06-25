using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace Michsky.UI.Dark
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CanvasGroup))]
    public class ModalWindowManager : MonoBehaviour
    {
        // Content
        public string title = "Title";
        [TextArea] public string description = "Description.";

        // Resources
        public Animator windowAnimator;
        public TextMeshProUGUI titleObject;
        public TextMeshProUGUI descriptionObject;
        public BlurManager blurManager;

        // Effects
        public bool enableDissolve = true;
        public UIDissolveEffect dissolveEffect;

        // Settings
        public bool useCustomContent;
        public bool disableOnOut = true;
        [Range(0.1f, 5)] public float disableAfter = 1;

        // Events
        public UnityEvent onEnable;
        public UnityEvent onCancel;

        [HideInInspector] public bool isOn;

        public void ModalWindowIn()
        {
            if (isOn == true)
                return;

            if (windowAnimator == null) { windowAnimator = gameObject.GetComponent<Animator>(); }
            if (useCustomContent == false) { UpdateUI(); }
            if (blurManager != null) { blurManager.BlurInAnim(); }

            StopCoroutine("DisableModal");
            windowAnimator.gameObject.SetActive(true);
            windowAnimator.Play("In");
            onEnable.Invoke();

            if (enableDissolve == true && dissolveEffect != null)
            {
                dissolveEffect.location = 1;
                dissolveEffect.DissolveIn();
            }

            StartCoroutine("IsOnLatency");
        }

        public void ModalWindowOut()
        {
            if (isOn == false)
                return;

            windowAnimator.Play("Out");
            onCancel.Invoke();

            if (enableDissolve == true && dissolveEffect != null) { dissolveEffect.DissolveOut(); }
            if (disableOnOut == true) { StartCoroutine("DisableModal"); }
            if (blurManager != null) { blurManager.BlurOutAnim(); }

            isOn = false;
        }

        public void UpdateUI()
        {
            titleObject.text = title;
            descriptionObject.text = description;
        }

        IEnumerator DisableModal()
        {
            yield return new WaitForSecondsRealtime(disableAfter);
            gameObject.SetActive(false);
        }

        IEnumerator IsOnLatency()
        {
            yield return new WaitForSecondsRealtime(0.1f);
            isOn = true;
        }
    }
}