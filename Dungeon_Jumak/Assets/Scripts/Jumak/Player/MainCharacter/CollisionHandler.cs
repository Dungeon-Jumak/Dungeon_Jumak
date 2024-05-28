// System
using System.Collections;
using System.Collections.Generic;

// Unity
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class CollisionHandler : MonoBehaviour
{
    //PlayerServing Component
    private PlayerServing playerServing;

    private void Start()
    {
        //Get Component
        playerServing = GetComponent<PlayerServing>();
    }

    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Contains("Gukbab"))
        {
            //PickUp Gukbab
            playerServing.PickUpGukBab(other.gameObject);
        }
        else if (other.gameObject.tag.Contains("Pajeon"))
        {
            //PickUp Pajeon
            playerServing.PickUpPajeon(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Table_L"))
        {
            //Place Food on Table_L
            playerServing.PlaceFoodOnTable(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Table_R"))
        {
            //Place Food on Table_R
            playerServing.PlaceFoodOnTable(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Trash"))
        {
            //ThrowAway food on player's hand
            playerServing.ThrowAwayFood();
        }
    }

}
