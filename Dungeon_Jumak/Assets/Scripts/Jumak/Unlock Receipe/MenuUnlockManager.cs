//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class MenuUnlockManager : MonoBehaviour
{
    private Data data;

    private void Start()
    {
        data = DataManager.Instance.data;
    }

    private void OnEnable()
    {
        
    }


}
