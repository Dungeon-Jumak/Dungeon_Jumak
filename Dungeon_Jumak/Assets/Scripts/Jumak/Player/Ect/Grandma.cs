using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grandma : MonoBehaviour
{
    [SerializeField]
    private GameObject grandmaSpeechBox;

    [SerializeField]
    private string cookingSound;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        //---������ ����---//
        if (transform.position.y < GameObject.Find("Chr_Player").transform.position.y) //ĳ���ͺ��� �Ʒ��� �ִٸ�
            spriteRenderer.sortingLayerName = "UpThanPlayer"; //�÷��̾�� ���� ������
        else
            spriteRenderer.sortingLayerName = "Player";
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
}
