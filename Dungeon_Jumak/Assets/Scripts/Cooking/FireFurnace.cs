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

    public void StartFireMiniGame()
    {
         blackPanel.SetActive(true);
         fireMiniGamePopup.SetActive(true);
    }

    public void ExitFireMiniGame()
    {
        blackPanel.SetActive(false);
        fireMiniGamePopup.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        fireSpeechBox.gameObject.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        fireSpeechBox.gameObject.SetActive(false);
    }
}
