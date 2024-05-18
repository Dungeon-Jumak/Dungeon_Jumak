using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MarketScene : MonoBehaviour
{
    public void MoveScene()
    {
        SceneManager.LoadScene("WaitingScene");
    }
}
