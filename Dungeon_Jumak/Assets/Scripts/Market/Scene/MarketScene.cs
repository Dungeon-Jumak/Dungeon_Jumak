using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MarketScene : MonoBehaviour
{
    //BGMManager bgmManager;

    private void Start()
    {
        //bgmManager = FindObjectOfType<BGMManager>();

        //bgmManager.Play(7);
    }


    public void MoveScene()
    {
        SceneManager.LoadScene("WaitingScene");
    }
}
