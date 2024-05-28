//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;

//This script is Avoid Duplication Collision
//Attach Food Object (Serving Food)

[DisallowMultipleComponent]
public class FoodScript : MonoBehaviour
{
    //Check isOnTable (not Serving table)
    [SerializeField] private bool isOnTable = false;

    public bool IsOnTable
    {
        get { return isOnTable; }
        set { isOnTable = value; }
    }
}
