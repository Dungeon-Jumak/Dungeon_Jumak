using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grandma : MonoBehaviour
{
    [SerializeField]
    private GameObject grandmaSpeechBox;

    private void OnTriggerStay2D(Collider2D collision)
    {
        grandmaSpeechBox.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        grandmaSpeechBox.SetActive(false);
    }
}
