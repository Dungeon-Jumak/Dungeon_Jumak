using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PajeonFurnace : MonoBehaviour
{
    [SerializeField]
    private GameObject pajeonSpeechBox;
    [SerializeField]
    private GameObject pajeonMiniGamePopup;
    [SerializeField]
    private PlayerMovement player;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private GameObject blackPanel;

    public void StartPajeonMiniGame()
    {

        if (!player.isCarryingFood)
        {
            pajeonMiniGamePopup.SetActive(true);
            blackPanel.SetActive(true);
            player.isCarryingFood = true;
        }
        else
        {
            animator.SetTrigger("notice");
        }

    }

    public void ExitPajeonMiniGame()
    {
        pajeonMiniGamePopup.gameObject.SetActive(false);
        blackPanel.gameObject.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        pajeonSpeechBox.gameObject.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        pajeonSpeechBox.gameObject.SetActive(false);
    }
}
