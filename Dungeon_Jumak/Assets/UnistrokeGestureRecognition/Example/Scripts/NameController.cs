using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UnistrokeGestureRecognition.Example {
    public sealed class NameController : MonoBehaviour {

        public CoolTime cooltime;

        private static readonly Color _transparent = new(1, 1, 1, 0);

        [SerializeField] private Text _text;

        private void Awake() {
            Clear();
        }

        public void Clear() {
            StopAllCoroutines();

            _text.text = "";
            _text.color = _transparent;
        }

        public void Set(string name) {
            _text.text = name;
            StartCoroutine(ShowNameAndHide());

            if (name == "Hi")
            {
                Debug.Log("마법 발동 짜라란~");
                cooltime.CoolStart();
            }

        }

        private IEnumerator ShowNameAndHide() {
            const float speed = 0.3f;
            float t = 0;

            while (t < 1f) {
                _text.color = Color.Lerp(_transparent, Color.white, t);
                t += Time.deltaTime * speed;
                yield return null;
            }
        }
    }
}