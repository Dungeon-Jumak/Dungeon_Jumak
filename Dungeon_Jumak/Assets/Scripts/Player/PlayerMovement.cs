using UnityEngine;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    public bool isPlace = false;
    public bool isCarryingFood = false; // ������ ��� �ִ��� Ȯ��
    public GameObject hand; // �÷��̾� �� ��ġ
    public CookGukbap cookGukbap; // ���� ī��Ʈ ���� ��ũ��Ʈ

    [SerializeField] 
    private FloatingJoystick joystick;
    [SerializeField] 
    private float moveSpeed; // �̵��ӵ�

    private Rigidbody2D playerRb;
    private Vector2 moveVector; // �̵�����

    private Queue<GameObject> foodQueue = new Queue<GameObject>(); // �浹�� Food ������Ʈ�� �����ϴ� Queue

    private Animator animator;
    private SpriteRenderer spriter;

    [SerializeField]
    private Transform[] tables;

    [SerializeField]
    private Data data;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        data = DataManager.Instance.data;
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
            float angle = Mathf.Atan2(moveVector.y, moveVector.x) * Mathf.Rad2Deg;
            // 4���� �������� ������ �ڵ� ������
            if (angle > -45f && angle <= 45f)
            {
                // ������ ����
                // �ִϸ����Ϳ� ������ ������ ��Ÿ���� �Ķ���� ���� ����
                animator.SetFloat("Horizontal", 1f);
                animator.SetFloat("Vertical", 0f);

                animator.SetFloat("isWalk", 1f);
            }
            else if (angle > 45f && angle <= 135f)
            {
                // ���� ����
                // �ִϸ����Ϳ� ���� ������ ��Ÿ���� �Ķ���� ���� ����
                animator.SetFloat("Horizontal", 0f);
                animator.SetFloat("Vertical", 1f);

                animator.SetFloat("isWalk", 1f);
            }
            else if (angle > 135f || angle <= -135f)
            {
                // ���� ����
                // �ִϸ����Ϳ� ���� ������ ��Ÿ���� �Ķ���� ���� ����
                animator.SetFloat("Horizontal", -1f);
                animator.SetFloat("Vertical", 0f);

                animator.SetFloat("isWalk", 1f);
            }
            else if (angle > -135f && angle <= -45f)
            {
                // �Ʒ��� ����
                // �ִϸ����Ϳ� �Ʒ��� ������ ��Ÿ���� �Ķ���� ���� ����
                animator.SetFloat("Vertical", -1f);
                animator.SetFloat("Horizontal", 0f);

                animator.SetFloat("isWalk", 1f);
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
        if (other.gameObject.CompareTag("Gukbap"))
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

        if (other.gameObject.CompareTag("Pajeon"))
        {
            FoodScript foodScript = other.GetComponent<FoodScript>();

            if (!isCarryingFood && !foodScript.IsOnTable)
            {
                foodQueue.Enqueue(other.gameObject); // �浹�� ������ Queue�� ����
                other.transform.parent = hand.transform; // �÷��̾� �� �Ʒ��� �̵�
                other.transform.localPosition = Vector3.zero;
                isCarryingFood = true;
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
                for (int i = 0; i < tables.Length; i++)
                {
                    if (other.transform == tables[i])
                    {
                        data.onTables[i] = true;

                        // --- ���� ���� �� �մ� ���̺� �ִ��� Ȯ�� --- //
                        if (data.isCustomer[i])
                        {
                            isCarryingFood = false;
                            GameObject food = foodQueue.Dequeue();
                            FoodScript foodScript = food.GetComponent<FoodScript>();

                            food.transform.parent = tableChild;

                            // ������ ���̺� ��ġ�� ����
                            food.transform.localPosition = Vector3.zero;

                            // ���¸� ������Ʈ�Ͽ� ���� �浹�� ����
                            foodScript.IsOnTable = true;
                        }
                    }
                }
            }
        }

        if (other.gameObject.CompareTag("Trash"))
        {
            Transform trash = other.transform.GetChild(0);

            if (isCarryingFood && trash.childCount == 0)
            {
                isCarryingFood = false;
                GameObject food = foodQueue.Dequeue();

                Destroy(food);

            }
        }
    }
}
