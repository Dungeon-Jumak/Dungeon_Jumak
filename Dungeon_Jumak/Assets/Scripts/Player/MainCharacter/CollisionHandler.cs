using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Gukbap") || other.gameObject.CompareTag("Pajeon"))
        {
            playerMovement.PickUpFood(other.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Table_L") || other.gameObject.CompareTag("Table_R"))
        {
            playerMovement.PlaceFoodOnTable(other.gameObject);
        }
        else if (other.gameObject.CompareTag("Trash"))
        {
            playerMovement.ThrowAwayFood();
        }
        else if (other.gameObject.CompareTag("Door_Ju") || other.gameObject.CompareTag("Door_Shop"))
        {
            playerMovement.DataInitialize();
            GameManager.Scene.LoadScene(Define.Scene.ComingSoon);
        }
    }
}
