using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private GameObject firePanel;

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
    }
}
