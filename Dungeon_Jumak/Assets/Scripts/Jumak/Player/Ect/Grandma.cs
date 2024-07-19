//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;

[DisallowMultipleComponent]
public class Grandma : MonoBehaviour
{
    //Speech Box for interact grandma
    [Header("할머니 상호작용 말풍선")]
    [SerializeField] private GameObject grandmaSpeechBox;

    [Header("음식 선택창 게임 오브젝트")]
    [SerializeField] private GameObject choosePanel;

    [Header("주막 씬 오브젝트")]
    [SerializeField] private JumakScene jumak;

    [Header("할머니를 가리키는 화살표 오브젝트")]
    [SerializeField] private SpriteRenderer targetArrow;

    //SpriteRenderer component to change renderer
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        //Get Component
        spriteRenderer = GetComponent<SpriteRenderer>();

        GameManager.Sound.Play("[S] Cooking Sound", Define.Sound.Effect, true);
    }

    private void Update()
    {
        //Change renderer according to player and grandma location
        if (transform.position.y < GameObject.Find("Chr_Player").transform.position.y) 
            spriteRenderer.sortingLayerName = "UpThanPlayer"; 
        else
            spriteRenderer.sortingLayerName = "Player";

        if (jumak.start) targetArrow.gameObject.SetActive(false);
    }

    //OnTriggerStay
    private void OnTriggerStay2D(Collider2D collision)
    {
        //Active SpeechBox when player stay grandma collider
        if (collision.CompareTag("Player") && !jumak.start)
            grandmaSpeechBox.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //InActive SpeechBox when player exit grandma collider
        if (collision.CompareTag("Player") && !jumak.start)
            grandmaSpeechBox.SetActive(false);
    }

    public void OnChoosePanel()
    {
        if (!jumak.start)
        {
            choosePanel.SetActive(true);
        }

    }
}
