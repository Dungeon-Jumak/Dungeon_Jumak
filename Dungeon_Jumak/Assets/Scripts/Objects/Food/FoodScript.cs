using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodScript : MonoBehaviour
{
    [SerializeField]
    private bool isOnTable = false;

    public bool IsOnTable
    {
        get { return isOnTable; }
        set { isOnTable = value; }
    }
}
