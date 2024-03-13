using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private FloatingJoystick joystick;

    [SerializeField]
    private float moveSpeed; // �̵��ӵ�

    private Rigidbody2D playerRb;

    private Vector2 moveVector; // �̵�����

    [SerializeField]
    private bool checkFood = false; // ���� ��� �ִ��� Ȯ��

    public GameObject hand; // �÷��̾� �� ��ġ

    private Queue<GameObject> foodQueue = new Queue<GameObject>(); // �浹�� food ������Ʈ�� �����ϴ� Queue

    public CookGukbap cookGukbap; // ���� ī��Ʈ ���� ��ũ��Ʈ

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

        // Rigidbody2D�� ����Ͽ� ��ġ�� ������Ʈ
        playerRb.MovePosition(playerRb.position + moveVector);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // ���� �浹 Ȯ��
        if (other.gameObject.CompareTag("Food"))
        {
            FoodScript foodScript = other.GetComponent<FoodScript>();

            if (!checkFood && foodScript.IsOnTable == false)
            {
                foodQueue.Enqueue(other.gameObject);// �浹�� ���� queue ����
                other.transform.parent = hand.transform;// �÷��̾� �� ������Ʈ ������ �̵�
                other.transform.localPosition = Vector3.zero;
                checkFood = true;

                cookGukbap.gukbapCount--;
            }
        }
    }



    private void OnCollisionEnter2D(Collision2D other)
    {
        //���̺� ���� �浹 Ȯ��
        if (other.gameObject.CompareTag("Table_L") || other.gameObject.CompareTag("Table_R"))
        {
            Transform secondChild = other.transform.GetChild(0);

            if (checkFood && secondChild.childCount == 0)
            {
                checkFood = false;
                GameObject food = foodQueue.Dequeue();
                FoodScript foodScript = food.GetComponent<FoodScript>();

                food.transform.parent = secondChild;

                // Food ������Ʈ�� ���̺� ��ġ�� ����
                food.transform.localPosition = Vector3.zero;

                // ���¸� ������Ʈ�Ͽ� ���� �浹�� ����
                foodScript.IsOnTable = true;
            }
        }
    }
}
