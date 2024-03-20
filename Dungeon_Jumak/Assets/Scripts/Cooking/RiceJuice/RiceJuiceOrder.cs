using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiceJuiceOrder : MonoBehaviour
{
    CircleCollider2D circleCollider;

    bool playerInsideCollider = false;


    void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        if (playerInsideCollider)
        {
            // ���콺 �Է� Ȯ��
            if (Input.GetMouseButtonDown(0))
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
                        }
                    }

                }
            }

            // ��ġ �Է� Ȯ��
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
