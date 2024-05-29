//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;

[DisallowMultipleComponent]
public class SelectFood : MonoBehaviour
{
    //if click category button, change category
    //Category String
    public string category = "";

    private void Start()
    {
        //start category is gukbab
        category = "Gukbab";
    }

    //select gukbab
    public void SelectGukbab()
    {
        category = "Gukbab";
    }

    //select pajeon
    public void SelectPajeon()
    {
        category = "Pajeon";
    }

    //select ricejuice
    public void SelectRiceJuice()
    {
        category = "RiceJuice";
    }


}
