using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    static public UIManager instance;
    /*
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
    */
    //---½Ì±ÛÅæ Àû¿ë---//
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
    }



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
