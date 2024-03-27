using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    static UIManager instance;
    public UIManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }



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
