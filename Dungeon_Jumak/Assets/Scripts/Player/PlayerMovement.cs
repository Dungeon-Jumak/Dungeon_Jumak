using UnityEngine;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    public CookGukbap cookGukbap; // ���� ī��Ʈ ���� ��ũ��Ʈ

    public GameObject hand; // �÷��̾� �� ��ġ

    public bool isPlace; //������ �ξ����� �Ǵ��� ����

    [SerializeField] 
    private FloatingJoystick joystick;
    [SerializeField] 
    private float moveSpeed; // �̵��ӵ�

    private Rigidbody2D playerRb;
    private Vector2 moveVector; // �̵�����

    [SerializeField] 
    private bool isCarryingFood = false; // ������ ��� �ִ��� Ȯ��

    private Queue<GameObject> foodQueue = new Queue<GameObject>(); // �浹�� Food ������Ʈ�� �����ϴ� Queue

    private Animator animator;
    private SpriteRenderer spriter;

    //---Order System ����---//

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        moveVector = Vector2.zero;
        moveVector.x = joystick.Horizontal * moveSpeed * Time.deltaTime;
        moveVector.y = joystick.Vertical * moveSpeed * Time.deltaTime;

        // ���̽�ƽ �Է��� ������� �÷��̾��� �̵� ������ ����
        if (moveVector != Vector2.zero)
        {
            // ���̽�ƽ �Է¿� ���� �÷��̾��� �ִϸ��̼��� ����
            if (moveVector.x > 0)
            {
                // ������ �������� �̵�
                // �ִϸ����Ϳ� ������ ������ ��Ÿ���� �Ķ���� ���� ����
                animator.SetFloat("Horizontal", moveVector.x);
                animator.SetFloat("isWalk", 1f);
                spriter.flipX = true;
            }
            else if (moveVector.x < 0)
            {
                // ���� �������� �̵�
                // �ִϸ����Ϳ� ���� ������ ��Ÿ���� �Ķ���� ���� ����
                animator.SetFloat("Horizontal", moveVector.x);
                animator.SetFloat("isWalk", 1f);
                spriter.flipX = false;

            }
        }
        else
        {
            // ���̽�ƽ �Է��� ���� ��, ���� ���·� �ִϸ��̼��� ����
            animator.SetFloat("isWalk", -1f);
        }
        // Rigidbody2D�� ����Ͽ� ��ġ�� ������Ʈ
        playerRb.MovePosition(playerRb.position + moveVector);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // ���� �浹 Ȯ��
        if (other.gameObject.CompareTag("Food"))
        {
            FoodScript foodScript = other.GetComponent<FoodScript>();

            if (!isCarryingFood && !foodScript.IsOnTable)
            {
                foodQueue.Enqueue(other.gameObject); // �浹�� ������ Queue�� ����
                other.transform.parent = hand.transform; // �÷��̾� �� �Ʒ��� �̵�
                other.transform.localPosition = Vector3.zero;
                isCarryingFood = true;

                cookGukbap.gukbapCount--;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // ���̺� �浹 Ȯ��
        if (other.gameObject.CompareTag("Table_L") || other.gameObject.CompareTag("Table_R"))
        {
            Transform tableChild = other.transform.GetChild(0);

            if (isCarryingFood && tableChild.childCount == 0)
            {
                isCarryingFood = false;
                GameObject food = foodQueue.Dequeue();
                FoodScript foodScript = food.GetComponent<FoodScript>();

                food.transform.parent = tableChild;

                //���̺� ���� ������ �ξ���
                //isPlace = true;

                // ������ ���̺� ��ġ�� ����
                food.transform.localPosition = Vector3.zero;

                // ���¸� ������Ʈ�Ͽ� ���� �浹�� ����
                foodScript.IsOnTable = true;
            }
        }
    }


}
