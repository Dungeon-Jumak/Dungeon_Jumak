//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class PortalManager : MonoBehaviour
{
    [Header("¸®ÅÏ ÆË¾÷")]
    [SerializeField] private GameObject returnPopup;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player"))
            return;

        //Return Pop up Active
        returnPopup.SetActive(true);
        
        //Pause
        Time.timeScale = 0f;
    }

    //Return : Continue Game
    public void Continue()
    {
        returnPopup.SetActive(false);

        Time.timeScale = 1f;
    }

    public void StopGame()
    {
        //Move Scene
        GameManager.Scene.LoadScene(Define.Scene.WaitingScene);
    }
}
