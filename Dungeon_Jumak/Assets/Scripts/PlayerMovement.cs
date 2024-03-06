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

    private bool checkFood = false; // ���� ��� �ִ��� Ȯ��
    public GameObject hand;
    private Queue<GameObject> foodQueue = new Queue<GameObject>(); // �浹�� food ������Ʈ�� �����ϴ� Queue


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
                    // �ٸ� Food�� ������ ù ��° �ڽ� ������Ʈ�� ����
                    Transform firstChild = other.transform.GetChild(0);
                    food.transform.parent = firstChild;
                }
                else
                {
                    food.transform.parent = secondChild;
                }

                // Food ������Ʈ�� �� ��° �ڽ� ������Ʈ�� ��ġ�� ����
                food.transform.localPosition = Vector3.zero;
                checkFood = false;
            }
        }
    }
}
