using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private FloatingJoystick joystick; 

    [SerializeField]
    private float moveSpeed; //�̵��ӵ�

    private Rigidbody2D PlayerRb; 

    private Vector2 moveVector; //�̵�����

    private void Awake()
    {
        PlayerRb = GetComponent<Rigidbody2D>();
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

        PlayerRb.MovePosition(PlayerRb.position + moveVector);
    }
}
