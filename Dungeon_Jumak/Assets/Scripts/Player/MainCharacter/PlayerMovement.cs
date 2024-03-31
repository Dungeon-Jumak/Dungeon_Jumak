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

    private Animator animator;//�ִϸ�����
    private SpriteRenderer spriter;//flip�� ���� player SpriteRenderer

    [SerializeField]
    private Transform[] tables;

    [SerializeField]
    private Data data; // Data ��ũ��Ʈ

    //---���� ����---//
    [SerializeField]
    private int menuNumsOfHand;

    //---�Ҹ� ����---//
    [SerializeField]
    private AudioManager audioManager;
    [SerializeField]
    private string walkSound1;
    [SerializeField]
    private string walkSound2;
    [SerializeField]
    private string walkSound3;
    [SerializeField]
    private string walkSound4;
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
        audioManager = FindObjectOfType<AudioManager>();

        data = DataManager.Instance.data;
    }

    private void Update()
    {
        PlayCookingSound();
        MovePlayer();
    }

    //---cookingSound ���� �Լ�---//
    private void PlayCookingSound()
    {
        if (!audioManager.IsPlaying(cookingSound))
        {
            audioManager.Play(cookingSound);
            audioManager.SetLoop(cookingSound);
            audioManager.Setvolume(cookingSound, 0.2f);
        }
    }

    //---Player �̵� ���� �Լ�---//
    private void MovePlayer()
    {
        moveVector = GetMoveVector();
        PlayFootstepSound(moveVector);
        UpdateAnimator(moveVector);

        // Rigidbody2D�� ����Ͽ� ��ġ�� ������Ʈ
        playerRb.MovePosition(playerRb.position + moveVector);
    }

    private Vector2 GetMoveVector()
    {
        Vector2 moveVector = Vector2.zero;
        moveVector.x = joystick.Horizontal * moveSpeed * Time.deltaTime;
        moveVector.y = joystick.Vertical * moveSpeed * Time.deltaTime;
        return moveVector;
    }

    private void PlayFootstepSound(Vector2 moveVector)
    {
        if (moveVector != Vector2.zero)
        {
            int randNum = Random.Range(1, 5);

            if (!audioManager.IsPlaying(walkSound1) && !audioManager.IsPlaying(walkSound2)
                && !audioManager.IsPlaying(walkSound3) && !audioManager.IsPlaying(walkSound4))
            {
                string soundToPlay = "";

                switch (randNum)
                {
                    case 1: soundToPlay = walkSound1; break;
                    case 2: soundToPlay = walkSound2; break;
                    case 3: soundToPlay = walkSound3; break;
                    case 4: soundToPlay = walkSound4; break;
                }

                if (!string.IsNullOrEmpty(soundToPlay))
                {
                    float volume = randNum == 3 ? 0.5f : 0.2f;
                    audioManager.Setvolume(soundToPlay, volume);
                    audioManager.Play(soundToPlay);
                }
            }
        }
        else
        {
            StopFootstepSounds();
        }
    }

    private void StopFootstepSounds()
    {
        audioManager.Stop(walkSound1);
        audioManager.Stop(walkSound2);
        audioManager.Stop(walkSound3);
        audioManager.Stop(walkSound4);
    }

    private void UpdateAnimator(Vector2 moveVector)
    {
        float angle = Mathf.Atan2(moveVector.y, moveVector.x) * Mathf.Rad2Deg;

        if (angle > -45f && angle <= 45f)
        {
            animator.SetFloat("Horizontal", 1f);
            animator.SetFloat("Vertical", 0f);
        }
        else if (angle > 45f && angle <= 135f)
        {
            animator.SetFloat("Horizontal", 0f);
            animator.SetFloat("Vertical", 1f);
        }
        else if (angle > 135f || angle <= -135f)
        {
            animator.SetFloat("Horizontal", -1f);
            animator.SetFloat("Vertical", 0f);
        }
        else if (angle > -135f && angle <= -45f)
        {
            animator.SetFloat("Vertical", -1f);
            animator.SetFloat("Horizontal", 0f);
        }

        animator.SetFloat("isWalk", moveVector != Vector2.zero ? 1f : -1f);
    }

    // ������ ���� �Լ�
    public void PickUpFood(GameObject foodObject)
    {
        FoodScript foodScript = foodObject.GetComponent<FoodScript>();

        if (!isCarryingFood && !foodScript.IsOnTable)
        {
            //���� ��� ���� ���
            audioManager.Play(pickUpSound);

            // ���� ��� ���� srpite renderer ���̾� Food_Up���� ����
            SpriteRenderer otherSpriteRenderer = foodObject.GetComponent<SpriteRenderer>();
            otherSpriteRenderer.sortingLayerName = "Food_Up";

            foodObject.layer = 7;
            foodQueue.Enqueue(foodObject); // �浹�� ������ Queue�� ����
            foodObject.transform.parent = hand.transform; // �÷��̾� �� �Ʒ��� �̵�
            foodObject.transform.localPosition = Vector3.zero;
            isCarryingFood = true;

            switch (foodObject.tag)
            {
                case "Gukbap":
                    menuNumsOfHand = 1;
                    if (foodObject.CompareTag("Gukbap"))
                        cookGukbap.gukbapCount--;
                    break;
                case "Pajeon":
                    menuNumsOfHand = 3;
                    break;
                // �߰����� ���� �±װ� �ִٸ� ���⿡ �߰�
                default:
                    Debug.LogWarning("Unhandled food tag: " + foodObject.tag);
                    break;
            }
        }
    }

    // ������ ���̺� ���� �Լ�
    public void PlaceFoodOnTable(GameObject tableObject)
    {
        Transform tableChild = tableObject.transform.GetChild(0);

        if (isCarryingFood && tableChild.childCount == 0)
        {
            for (int i = 0; i < tables.Length; i++)
            {
                if (tableObject.transform == tables[i])
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

    // ������ ������ �Լ�
    public void ThrowAwayFood()
    {
        if (isCarryingFood && hand.transform.childCount > 0)
        {
            isCarryingFood = false;
            audioManager.Play(trashCanSound);
            GameObject food = foodQueue.Dequeue();
            Destroy(food);
        }
    }

    //���� ����� �� ���� ������ �� �ʱ�ȭ
    public void DataInitialize()
    {
        audioManager.Stop(walkSound1);
        audioManager.Stop(walkSound2);
        audioManager.Stop(walkSound3);
        audioManager.Stop(walkSound4);
        audioManager.Stop(cookingSound);


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
