using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private FloatingJoystick joystick;

    [SerializeField]
    private float moveSpeed; // 이동속도

    private Rigidbody2D playerRb;

    private Vector2 moveVector; // 이동벡터

    private bool checkFood = false; // 음식 들고 있는지 확인
    public GameObject hand;
    private Queue<GameObject> foodQueue = new Queue<GameObject>(); // 충돌한 food 오브젝트를 저장하는 Queue


    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        moveVector = Vector2.zero;
        moveVector.x = joystick.Horizontal * moveSpeed * Time.deltaTime;
        moveVector.y = joystick.Vertical * moveSpeed * Time.deltaTime;

        // Rigidbody2D를 사용하여 위치를 업데이트
        playerRb.MovePosition(playerRb.position + moveVector);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Food"))
        {
            if (!checkFood)
            {
                foodQueue.Enqueue(other.gameObject);
                other.transform.parent = hand.transform;
                other.transform.localPosition = Vector3.zero;
                checkFood = true;
            }
        }

        if (other.gameObject.CompareTag("Table"))
        {
            if (checkFood)
            {
                GameObject food = foodQueue.Dequeue();
                Transform secondChild = other.transform.GetChild(1);

                if (secondChild.childCount > 0)
                {
                    // 다른 Food가 있으면 첫 번째 자식 오브젝트에 놓기
                    Transform firstChild = other.transform.GetChild(0);
                    food.transform.parent = firstChild;
                }
                else
                {
                    food.transform.parent = secondChild;
                }

                // Food 오브젝트를 두 번째 자식 오브젝트의 위치에 고정
                food.transform.localPosition = Vector3.zero;
                checkFood = false;
            }
        }
    }
}
