//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;

[DisallowMultipleComponent]
public class FireFurnace : MonoBehaviour
{
    //Fire MiniGame Speech Box
    [Header("불 미니게임 말풍선")]
    [SerializeField] private GameObject fireSpeechBox;

    //Fire MiniGame Popup
    [Header("불 미니게임 팝업")]
    [SerializeField] private GameObject fireMiniGamePopup;

    //Black Panel
    [Header("블랙 패널")]
    [SerializeField] private GameObject blackPanel;

    //Jumak Scene
    private JumakScene jumakScene;

    [SerializeField] private CustomerSpawner customerSpawner;


    private void Start()
    {
        jumakScene = FindObjectOfType<JumakScene>();
    }

    #region Button Method

    //Start MiniGame
    public void StartFireMiniGame()
    {
        //Active Black Panel
        blackPanel.SetActive(true);

        customerSpawner.gameObject.SetActive(false);

        jumakScene.pause = true;

        //Active Popup
        fireMiniGamePopup.SetActive(true);
    }

    public void ExitFireMiniGame()
    {
        //Inactive Black Panel
        blackPanel.SetActive(false);

        customerSpawner.gameObject.SetActive(true);

        jumakScene.pause = false;

        //Inactive Popup
        fireMiniGamePopup.SetActive(false);
    }
    #endregion

    #region Trigger
    //OnTriggerStay : if detect player, show speech box
    private void OnTriggerStay2D(Collider2D collision)
    {
        //Detect Player Stay in Collider
        if(collision.CompareTag("Player") && jumakScene.start)
            //Active Speech box
            fireSpeechBox.gameObject.SetActive(true);
    }

    //OnTriggerExix : if is not detected player, can't see speech box
    private void OnTriggerExit2D(Collider2D collision)
    {
        //Detect Player Exit in Collider
        if (collision.CompareTag("Player") && jumakScene.start)
            //Inactive Speech box
            fireSpeechBox.gameObject.SetActive(false);
    }
    #endregion
}
