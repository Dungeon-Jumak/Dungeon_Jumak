using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFurnace : MonoBehaviour
{
    [SerializeField]
    private GameObject fireSpeechBox;
    [SerializeField]
    private GameObject fireMiniGamePopup;
    [SerializeField]
    private PlayerMovement player;

    [SerializeField]
    private GameObject blackPanel;
    [SerializeField]
    private JumakScene jumakScene;

    public void StartFireMiniGame()
    {
         blackPanel.SetActive(true);
         fireMiniGamePopup.SetActive(true);
         jumakScene.isPause = true;
    }

    public void ExitFireMiniGame()
    {
        blackPanel.SetActive(false);
        fireMiniGamePopup.SetActive(false);
        jumakScene.isPause = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
            fireSpeechBox.gameObject.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            fireSpeechBox.gameObject.SetActive(false);
    }
}
