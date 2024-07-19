//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;
using UnityEngine.Audio;

[DisallowMultipleComponent]
public class PajeonFurnace : MonoBehaviour
{
    //Pajeon MiniGame Speech Box
    [Header("파전 미니게임 말풍선")]
    [SerializeField] private GameObject pajeonSpeechBox;

    //Pajeon MiniGame Popup
    [Header("파전 미니게임 팝업")]
    [SerializeField] private GameObject pajeonMiniGamePopup;

    //Player Serving Sctipt
    [Header("플레이어 서빙 스크립트")]
    [SerializeField] private PlayerServing player;

    //Animator for notice, if is carrying any food
    [Header("음식을 들고 있을 경우 띄울 노티스 애니메이터")]
    [SerializeField] private Animator noticeAnim;

    //Black Panel
    [Header("블랙 패널")]
    [SerializeField] private GameObject blackPanel;

    //Jumak Scene
    [Header("주막 씬 스크립트")]
    [SerializeField] private JumakScene jumakScene;

    [Header("손님 스포너")]
    [SerializeField] private CustomerSpawner customerSpawner;

    //To Start Pajeon MiniGame
    public void StartPajeonMiniGame()
    {
        //Active when player isn't carrying any food
        if (!player.isCarryingFood)
        {
            //Active Pajeon MiniGame Popup
            pajeonMiniGamePopup.SetActive(true);

            //Active Black Panel
            blackPanel.SetActive(true);

            customerSpawner.gameObject.SetActive(false);

            jumakScene.pause = true;

            GameManager.Sound.Play("[S] Baking Pajeon", Define.Sound.Effect, true);
        }
        //Play Notice Animation
        else
        {
            noticeAnim.SetTrigger("notice");
        }

    }

    //To Exit Pajeon MiniGame
    public void ExitPajeonMiniGame()
    {
        //InActive Pajen MiniGame Popup
        pajeonMiniGamePopup.gameObject.SetActive(false);

        //InActive Black Panel
        blackPanel.gameObject.SetActive(false);

        customerSpawner.gameObject.SetActive(true);

        jumakScene.pause = false;

        GameManager.Sound.Pause("[S] Baking Pajeon", Define.Sound.Effect);
    }

    //OnTriggerStay
    private void OnTriggerStay2D(Collider2D collision)
    {
        //Active Pajeon MiniGame Speech Box when player is staying
        if (collision.CompareTag("Player") && jumakScene.start)
            pajeonSpeechBox.gameObject.SetActive(true);
    }

    //OnTriggerExit
    private void OnTriggerExit2D(Collider2D collision)
    {
        //InActive Pajeon MiniGame Speech Box when player exit
        if (collision.CompareTag("Player") && jumakScene.start)
            pajeonSpeechBox.gameObject.SetActive(false);
    }

}
