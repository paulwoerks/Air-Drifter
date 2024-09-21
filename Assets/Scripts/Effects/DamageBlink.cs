using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class DamageBlink : MonoBehaviour
    {
        Renderer renderer;
        Color originalColor;

        float blinkTime = 0.1f;

        bool isBlinking => blinkRoutine != null;

        Coroutine blinkRoutine;

        private void Awake()
        {
            renderer = GetComponent<Renderer>();
        }

        private void OnDisable()
        {
            if (isBlinking)
            {
                StopCoroutine(blinkRoutine);
                SetBlinkActive(false);
            }
        }

        public void Blink()
        {
            if (isBlinking) return;

            blinkRoutine = StartCoroutine(ExecuteBlink());
        }

        void SetBlinkActive(bool blinking)
        {
            if (blinking)
            {
                originalColor = renderer.material.color;
                renderer.material.color = Color.white;
            } else
            {
                renderer.material.color = originalColor;
                blinkRoutine = null;
            }
        }

        IEnumerator ExecuteBlink()
        {
            SetBlinkActive(true);
            yield return new WaitForSeconds(blinkTime);
            SetBlinkActive(false);
        }
    }
}
