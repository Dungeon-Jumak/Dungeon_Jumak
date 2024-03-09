using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnaceExitBtn : MonoBehaviour
{
    public GameObject furnacePopUp;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FurnaceBtn()
    {
        furnacePopUp.gameObject.SetActive(false);
    }
}
