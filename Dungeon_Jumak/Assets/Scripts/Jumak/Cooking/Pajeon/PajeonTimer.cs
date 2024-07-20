//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class PajeonTimer : MonoBehaviour
{
    [Header("파전 스프라이트 이미지")]
    [SerializeField] private Image pajeonSR;

    [Header("파먼 매니저")]
    [SerializeField] private PaJeonManager pajeonManager;

    private void OnEnable()
    {
        StartCoroutine(PajeonTimerCoroutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator PajeonTimerCoroutine()
    {
        for (float i = 1; i > 0; i -= 0.1f)
        {
            pajeonSR.color = new Color(i, i, i, 1);

            yield return new WaitForSeconds(0.2f);
        }

        pajeonManager.Fail();
    }
}
