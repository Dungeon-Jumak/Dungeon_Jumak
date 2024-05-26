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
        //---렌더링 변경---//
        if (transform.position.y < GameObject.Find("Chr_Player").transform.position.y) //캐릭터보다 아래에 있다면
            spriteRenderer.sortingLayerName = "UpThanPlayer"; //플레이어보다 위에 렌더링
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
