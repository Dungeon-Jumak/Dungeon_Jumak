//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;
using TMPro;

[DisallowMultipleComponent]
public class DialogTest : MonoBehaviour
{
    [SerializeField] private DialogSystem dialogSystem;

    public void StartDialog()
    {
        dialogSystem.UpdateDialog();
    }

    public void SkipDialog()
    {
        dialogSystem.SkipDialog();
    }
}
