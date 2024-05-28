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

    //SpriteRenderer component to change renderer
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        //Get Component
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        //Change renderer according to player and grandma location
        if (transform.position.y < GameObject.Find("Chr_Player").transform.position.y) 
            spriteRenderer.sortingLayerName = "UpThanPlayer"; 
        else
            spriteRenderer.sortingLayerName = "Player";
    }

    //OnTriggerStay
    private void OnTriggerStay2D(Collider2D collision)
    {
        //Active SpeechBox when player stay grandma collider
        if (collision.CompareTag("Player"))
            grandmaSpeechBox.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //InActive SpeechBox when player exit grandma collider
        if (collision.CompareTag("Player"))
            grandmaSpeechBox.SetActive(false);
    }
}
