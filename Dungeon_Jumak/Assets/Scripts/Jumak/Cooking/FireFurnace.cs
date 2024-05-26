//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;

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

    //Player
    private PlayerMovement player;

    //Jumak Scene
    private JumakScene jumakScene;


    private void Start()
    {
        //Get Component
        player = FindObjectOfType<PlayerMovement>();
        jumakScene = FindObjectOfType<JumakScene>();
    }

    #region Button Method

    //Start MiniGame
    public void StartFireMiniGame()
    {
        //Active Black Panel
        blackPanel.SetActive(true);

        //Active Popup
        fireMiniGamePopup.SetActive(true);

        //Pause
        jumakScene.isPause = true;
    }

    public void ExitFireMiniGame()
    {
        //Inactive Black Panel
        blackPanel.SetActive(false);

        //Inactive Popup
        fireMiniGamePopup.SetActive(false);

        //Resume
        jumakScene.isPause = false;
    }
    #endregion

    #region Trigger
    //OnTriggerStay : if detect player, show speech box
    private void OnTriggerStay2D(Collider2D collision)
    {
        //Detect Player Stay in Collider
        if(collision.CompareTag("Player"))
            //Active Speech box
            fireSpeechBox.gameObject.SetActive(true);
    }

    //OnTriggerExix : if is not detected player, can't see speech box
    private void OnTriggerExit2D(Collider2D collision)
    {
        //Detect Player Exit in Collider
        if (collision.CompareTag("Player"))
            //Inactive Speech box
            fireSpeechBox.gameObject.SetActive(false);
    }
    #endregion
}
