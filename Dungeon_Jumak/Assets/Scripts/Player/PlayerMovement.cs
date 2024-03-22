using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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

    //---���� ����---//
    [SerializeField]
    private int menuNumsOfHand;

    //---�Ҹ� ����---//
    [SerializeField]
    private AudioManager audioManager;
    [SerializeField]
    private string walkSound;
    [SerializeField]
    private string servingSound;
    [SerializeField]
    private string pickUpSound;
    [SerializeField]
    private string cookingSound;
    [SerializeField]
    private string trashCanSound;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        data = DataManager.Instance.data;
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Update()
    {
        //�丮�ϴ� ���� ��� ���
        if (!audioManager.isPlaying(cookingSound))
        {
            audioManager.Play(cookingSound);
            audioManager.SetLoop(cookingSound);
            audioManager.Setvolume(cookingSound, 0.1f);
        }

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
            if (!audioManager.isPlaying(walkSound))
            {
                audioManager.Setvolume(walkSound, 0.2f);
                audioManager.Play(walkSound);
            }


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
            audioManager.Stop(walkSound);
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
                //���� ��� ���� ���
                audioManager.Play(pickUpSound);

                // ���� ��� ���� srpite renderer ���̾� Food_Up���� ����
                SpriteRenderer otherSpriteRenderer = other.GetComponent<SpriteRenderer>();
                otherSpriteRenderer.sortingLayerName = "Food_Up";

                other.gameObject.layer = 7;
                foodQueue.Enqueue(other.gameObject); // �浹�� ������ Queue�� ����
                other.transform.parent = hand.transform; // �÷��̾� �� �Ʒ��� �̵�
                other.transform.localPosition = Vector3.zero;
                isCarryingFood = true;

                //�տ� �� ���� �޴��� ����
                menuNumsOfHand = 1;

                cookGukbap.gukbapCount--;
            }
        }

        if (other.gameObject.CompareTag("Pajeon"))
        {
            other.gameObject.layer = 7;

            FoodScript foodScript = other.GetComponent<FoodScript>();

            if (!isCarryingFood && !foodScript.IsOnTable)
            {
                // ���� ��� ���� srpite renderer ���̾� Food_Up���� ����
                SpriteRenderer otherSpriteRenderer = other.GetComponent<SpriteRenderer>();
                otherSpriteRenderer.sortingLayerName = "Food_Up";

                foodQueue.Enqueue(other.gameObject); // �浹�� ������ Queue�� ����
                other.transform.parent = hand.transform; // �÷��̾� �� �Ʒ��� �̵�
                other.transform.localPosition = Vector3.zero;
                isCarryingFood = true;

                //�տ� �� ���� �޴��� ����
                menuNumsOfHand = 3;
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
                        // --- ���� ���� �� �մ� ���̺� �ִ��� Ȯ�� --- //
                        if (data.isCustomer[i] && data.menuNums[i] == menuNumsOfHand)
                        {
                            data.onTables[i] = true;
                            isCarryingFood = false;

                            //���� ���� ���� ���
                            audioManager.Play(servingSound);

                            GameObject food = foodQueue.Dequeue();
                            FoodScript foodScript = food.GetComponent<FoodScript>();

                            //���� Sprite Renderer ���̾� Food_Down���� ����
                            SpriteRenderer foodSpriteRenderer = food.GetComponent<SpriteRenderer>();
                            foodSpriteRenderer.sortingLayerName = "Food_Down";

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

                audioManager.Play(trashCanSound);
                GameObject food = foodQueue.Dequeue();

                Destroy(food);

            }
        }

        //---���Ƿ� �Ѿ��. ���߿� ������ ������ �� �����ؾ� �ҵ�?---//
        if (other.gameObject.CompareTag("Door_Ju"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("ComingSoon");
            DataInitialize();
        }
        else if (other.gameObject.CompareTag("Door_Shop"))
        {
            DataInitialize();
            UnityEngine.SceneManagement.SceneManager.LoadScene("ComingSoon");
        }
    }

    //���� ����� �� ���� ������ �� �ʱ�ȭ
    private void DataInitialize()
    {
        Debug.Log("��� ���̺��� �ʱ�ȭ �˴ϴ�.");
        data.curSeatSize = 0;
        
        for (int i = 0; i < data.isAllocated.Length; i++)
        {
            data.isAllocated[i] = false;
            data.isCustomer[i] = false;
            data.onTables[i] = false;
            data.isFinEat[i] = false;

            data.menuNums[i] = 0;
        }
    }
}
