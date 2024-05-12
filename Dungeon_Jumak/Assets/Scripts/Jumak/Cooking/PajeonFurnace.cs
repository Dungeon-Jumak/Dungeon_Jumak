using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PajeonFurnace : MonoBehaviour
{
    [SerializeField]
    private GameObject pajeonSpeechBox;
    [SerializeField]
    private GameObject pajeonMiniGamePopup;
    [SerializeField]
    private PlayerServing player;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private GameObject blackPanel;

    AudioManager audioManager;
    [SerializeField]
    private string pajeonSound;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    public void StartPajeonMiniGame()
    {

        if (!player.isCarryingFood)
        {
            audioManager.Play(pajeonSound);
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
        audioManager.Stop(pajeonSound);

        pajeonMiniGamePopup.gameObject.SetActive(false);
        blackPanel.gameObject.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            pajeonSpeechBox.gameObject.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            pajeonSpeechBox.gameObject.SetActive(false);
    }

}
