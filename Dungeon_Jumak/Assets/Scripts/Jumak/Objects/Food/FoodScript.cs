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

    private void Start()
    {
        //음식 스케일 조정
        /*
        if (gameObject.CompareTag("Gukbab"))
        {
            float newScale = ((float)Screen.width / Screen.height) * 0.9f;
            transform.localScale = new Vector3(newScale, newScale, transform.localScale.z);
        }
        else if (gameObject.CompareTag("Pajeon"))
        {
            float newScale = ((float)Screen.width / Screen.height) * 0.09f;
            transform.localScale = new Vector3(newScale, newScale, transform.localScale.z);
        }
        */
    }
}
