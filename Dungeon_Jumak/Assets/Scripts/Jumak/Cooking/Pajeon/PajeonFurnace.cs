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
    [Header("���� �̴ϰ��� ��ǳ��")]
    [SerializeField] private GameObject pajeonSpeechBox;

    //Pajeon MiniGame Popup
    [Header("���� �̴ϰ��� �˾�")]
    [SerializeField] private GameObject pajeonMiniGamePopup;

    //Player Serving Sctipt
    [Header("�÷��̾� ���� ��ũ��Ʈ")]
    [SerializeField] private PlayerServing player;

    //Animator for notice, if is carrying any food
    [Header("������ ��� ���� ��� ��� ��Ƽ�� �ִϸ�����")]
    [SerializeField] private Animator noticeAnim;

    //Black Panel
    [Header("���� �г�")]
    [SerializeField] private GameObject blackPanel;

    //Jumak Scene
    [Header("�ָ� �� ��ũ��Ʈ")]
    [SerializeField] private JumakScene jumakScene;

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

            //Pause
            jumakScene.isPause = true;
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
    }

    //OnTriggerStay
    private void OnTriggerStay2D(Collider2D collision)
    {
        //Active Pajeon MiniGame Speech Box when player is staying
        if (collision.CompareTag("Player"))
            pajeonSpeechBox.gameObject.SetActive(true);
    }

    //OnTriggerExit
    private void OnTriggerExit2D(Collider2D collision)
    {
        //InActive Pajeon MiniGame Speech Box when player exit
        if (collision.CompareTag("Player"))
            pajeonSpeechBox.gameObject.SetActive(false);
    }

}