//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;

[DisallowMultipleComponent]
public class Grandma : MonoBehaviour
{
    //Speech Box for interact grandma
    [Header("�ҸӴ� ��ȣ�ۿ� ��ǳ��")]
    [SerializeField] private GameObject grandmaSpeechBox;

    [Header("���� ����â ���� ������Ʈ")]
    [SerializeField] private GameObject choosePanel;

    [Header("�ָ� �� ������Ʈ")]
    [SerializeField] private JumakScene jumak;

    [Header("�ҸӴϸ� ����Ű�� ȭ��ǥ ������Ʈ")]
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
