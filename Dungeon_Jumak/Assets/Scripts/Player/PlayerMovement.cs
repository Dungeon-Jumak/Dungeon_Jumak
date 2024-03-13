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

    [SerializeField]
    private bool checkFood = false; // 음식 들고 있는지 확인

    public GameObject hand; // 플레이어 손 위치

    private Queue<GameObject> foodQueue = new Queue<GameObject>(); // 충돌한 food 오브젝트를 저장하는 Queue

    public CookGukbap cookGukbap; // 국밥 카운트 참조 스크립트

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 국밥 충돌 확인
        if (other.gameObject.CompareTag("Food"))
        {
            FoodScript foodScript = other.GetComponent<FoodScript>();

            if (!checkFood && foodScript.IsOnTable == false)
            {
                foodQueue.Enqueue(other.gameObject);// 충돌한 국밥 queue 저장
                other.transform.parent = hand.transform;// 플레이어 손 오브젝트 하위로 이동
                other.transform.localPosition = Vector3.zero;
                checkFood = true;

                cookGukbap.gukbapCount--;
            }
        }
    }



    private void OnCollisionEnter2D(Collision2D other)
    {
        //테이블 좌측 충돌 확인
        if (other.gameObject.CompareTag("Table_L") || other.gameObject.CompareTag("Table_R"))
        {
            Transform secondChild = other.transform.GetChild(0);

            if (checkFood && secondChild.childCount == 0)
            {
                checkFood = false;
                GameObject food = foodQueue.Dequeue();
                FoodScript foodScript = food.GetComponent<FoodScript>();

                food.transform.parent = secondChild;

                // Food 오브젝트를 테이블 위치에 고정
                food.transform.localPosition = Vector3.zero;

                // 상태를 업데이트하여 다음 충돌을 방지
                foodScript.IsOnTable = true;
            }
        }
    }
}
