using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grandma : MonoBehaviour
{
    [SerializeField]
    private GameObject grandmaSpeechBox;

    [SerializeField]
    private AudioManager audioManager;

    [SerializeField]
    private string cookingSound;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        //PlayCookingSound();
        Invoke("PlayCookingSound", 0.5f);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            grandmaSpeechBox.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            grandmaSpeechBox.SetActive(false);
    }

    //---cookingSound 관련 함수---//
    private void PlayCookingSound()
    {
        audioManager.Play(cookingSound);
        audioManager.SetLoop(cookingSound);
        audioManager.Setvolume(cookingSound, 0.2f);
    }
}
