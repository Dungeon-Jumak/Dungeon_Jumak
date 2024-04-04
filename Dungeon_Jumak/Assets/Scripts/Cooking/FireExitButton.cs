using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireExitButton : MonoBehaviour
{
    public GameObject FireMiniGamePopUp;
    public Tracker tracker;

    public void ExitFireMiniGameBtn()
    {
        FireMiniGamePopUp.gameObject.SetActive(false);
        tracker.inputEnabled = true;
    }
}
