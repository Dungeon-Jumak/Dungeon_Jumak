using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiceJuiceOrder : MonoBehaviour
{
    public int blackPanelIdx; //�� �г� �ε���

    CircleCollider2D circleCollider;

    bool playerInsideCollider = false;

    private PlayerServing player;


    void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        player = FindObjectOfType<PlayerServing>();
    }

    void Update()
    {
        if (playerInsideCollider)
        {
            //===���콺 �Է� Ȯ��===//
            if (Input.GetMouseButtonDown(0) && !player.isCarryingFood)
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = transform.position.z;
                if (circleCollider.OverlapPoint(mousePosition))
                {
                    Debug.Log("����");
                    GameObject[] parentObjects = GameObject.FindGameObjectsWithTag("RiceJuiceMiniGame");

                    foreach (GameObject parentObject in parentObjects)
                    {
                        foreach (Transform child in parentObject.transform)
                        {
                            child.gameObject.SetActive(true);
                            GameObject.Find("Home_Panel").transform.GetChild(blackPanelIdx);
                        }
                    }

                }
            }

            //===��ġ �Է� Ȯ��===//
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                touchPosition.z = transform.position.z;
                if (circleCollider.OverlapPoint(touchPosition))
                {
                    GameObject[] parentObjects = GameObject.FindGameObjectsWithTag("RiceJuiceMiniGame");

                    foreach (GameObject parentObject in parentObjects)
                    {
                        foreach (Transform child in parentObject.transform)
                        {
                            child.gameObject.SetActive(true);
                        }
                    }

                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideCollider = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideCollider = false;
        }
    }
}
