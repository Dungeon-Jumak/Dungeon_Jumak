//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;

//TMPro
using TMPro;

[DisallowMultipleComponent]
public class PrintString : MonoBehaviour
{
    [Header("TMP")]
    [SerializeField] private TextMeshProUGUI tmp;

    [Header("출력할 스트링")]
    [SerializeField] private string tmpString;

    private int index = 0;

    private void OnEnable()
    {
        index = 1;
        tmpString = tmp.text;

        StartCoroutine(PrintOneByOne());
    }

    IEnumerator PrintOneByOne()
    {
        tmp.text = tmpString.Substring(0, index);

        if (index < tmpString.Length)
            index++;

        if (index >= tmpString.Length)
            index = 0;

        yield return new WaitForSeconds(0.2f);

        StartCoroutine(PrintOneByOne());
    }
}
