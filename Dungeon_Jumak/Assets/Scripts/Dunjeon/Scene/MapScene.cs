using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapScene : MonoBehaviour
{
    public GameObject movingPanel;

    public void OpenMovingPanel()
    {
        movingPanel.SetActive(true);
        StartCoroutine(LoadSceneAfterDelay(1)); 
    }

    IEnumerator LoadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);  
        SceneManager.LoadScene("MainDunjeon");  
    }
}
