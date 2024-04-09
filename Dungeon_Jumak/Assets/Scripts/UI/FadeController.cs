using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    public Image black;
    private Color color;

    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);

    public void FadeOut(float speed = 0.02f)
    {
        Color newColor = new Color(0, 0, 0);
        newColor.a = 0f;

        StartCoroutine(FadeOutCoroutine(speed));
    }

    IEnumerator FadeOutCoroutine(float _speed)
    {
        black.gameObject.SetActive(true);
        color = black.color;

        while(color.a < 1f)
        {
            color.a += _speed;
            black.color = color;
            yield return waitTime;
        }

        color.a = 0f;
        black.color = color;

        black.gameObject.SetActive(false);
    }

    public void FadeIn(float speed = 0.02f)
    {
        Color newColor = new Color(0, 0, 0);
        newColor.a = 1f;

        StartCoroutine(FadeInCoroutine(speed));
    }
    IEnumerator FadeInCoroutine(float _speed)
    {
        black.gameObject.SetActive(true);
        color = black.color;

        while (color.a < 1f)
        {
            color.a += _speed;
            black.color = color;
            yield return waitTime;
        }

        color.a = 1f;
        black.color = color;

        black.gameObject.SetActive(false);
    }
}
