using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectFood : MonoBehaviour
{
    public string category = "";

    private void Start()
    {
        category = "Gukbab";
    }

    public void SelectGukbab()
    {
        category = "Gukbab";
    }

    public void SelectPajeon()
    {
        category = "Pajeon";
    }

    public void SelectRiceJuice()
    {
        category = "RiceJuice";
    }


}
