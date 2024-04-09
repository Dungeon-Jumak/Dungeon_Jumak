using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
using static Unity.VisualScripting.Member;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    public bool isPlace = false;
    public bool isCarryingFood = false; // ������ ��� �ִ��� Ȯ��

    public GameObject hand; // �÷��̾� �� ��ġ
    public CookGukbap cookGukbap; // ���� ī��Ʈ ���� ��ũ��Ʈ

    private Queue<GameObject> foodQueue = new Queue<GameObject>(); // �浹�� Food ������Ʈ�� �����ϴ� Queue

    private Animator animator;//�ִϸ�����

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
    private string servingSound;
    [SerializeField]
    private string pickUpSound;
    [SerializeField]
    private string cookingSound;
    [SerializeField]
    private string trashCanSound;

    private bool isMove;
    [SerializeField]
    private float delaySecond;

    private RaycastHit2D hit;

    private void Awake()
    {
        isMove = false;
        delaySecond = 0.5f;

        animator = GetComponent<Animator>();
        audioManager = FindObjectOfType<AudioManager>();

        data = DataManager.Instance.data;
    }

    private void Update()
    {
        PlayCookingSound();

        if (Input.GetMouseButtonDown(0) && !isMove)
        {
            if (isMove == true) return;

            isMove = true;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //���� ���� ����
            hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero); //hit object ��ȯ
            PlayerMove();       

        }
    }

    //�÷��̾� �̵� �� �ִϸ��̼� �۵�
    private void PlayerMove()
    {
        animator.SetTrigger("tpTrigger"); //Ʈ���� �۵�

        Invoke("Teleport", 0.2f); //�ִϸ��̼� �۵� �� Ƽ��
        Invoke("MoveDelay", delaySecond);
    }

    //�÷��̾� ����
    private void Teleport()
    {
        transform.position = hit.point;
    }

    //���� ��Ÿ���� ���� Invoke�� �����ϱ� ���� �Լ�
    private void MoveDelay()
    {
        isMove = false;
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
