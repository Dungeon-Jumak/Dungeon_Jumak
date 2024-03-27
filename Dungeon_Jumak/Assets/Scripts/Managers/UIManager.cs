using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private GameObject firePanel;
    [SerializeField] private Tracker tracker;

    public void exitOptionPanel()
    {
        optionPanel.SetActive(false);
    }

    public void openOptionPanel()
    {
        optionPanel.SetActive(true);
    }

    public void exitFirePanel() {
        firePanel.SetActive(false);
        tracker.inputEnabled = true;
    }
}
