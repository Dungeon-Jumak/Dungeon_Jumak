using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
using static Unity.VisualScripting.Member;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerServing : MonoBehaviour
{
    [SerializeField]
    private Data data;
    //---���� ����---//
    [SerializeField]
    private string menuCategori;
    [SerializeField]
    private int menuValue;

    //---�Ҹ� ����---//
    [SerializeField]
    private AudioManager audioManager;
    [SerializeField]
    private string servingSound;
    [SerializeField]
    private string pickUpSound;
    [SerializeField]
    private string trashCanSound;

    private Queue<GameObject> foodQueue = new Queue<GameObject>(); // �浹�� Food ������Ʈ�� �����ϴ� Queue

    private Animator animator;//�ִϸ�����

    public bool isPlace = false;
    public bool isCarryingFood = false; // ������ ��� �ִ��� Ȯ��
    public Transform[] tables;

    public GameObject hand; // �÷��̾� �� ��ġ
    public CookGukbap cookGukbap; // ���� ī��Ʈ ���� ��ũ��Ʈ

    public bool moveStop;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioManager = FindObjectOfType<AudioManager>();

        data = DataManager.Instance.data;
    }

    public void PickUpFood(GameObject foodObject)
    {
        moveStop = true;

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
    }

    public void PickUpGukBab(GameObject foodObject)
    {
        FoodScript foodScript = foodObject.GetComponent<FoodScript>();

        //�տ� ������ ��� ���� �ʴٸ�
        if (!isCarryingFood && !foodScript.IsOnTable)
        {
            PickUpFood(foodObject);

            //������ ������ ���� ������ �ٸ��� �����ϱ� ����
            switch (foodObject.tag)
            {
                case "Gukbab":
                    menuCategori = "Gukbab";
                    menuValue = 1; //�±׺��� ������ ��� ����
                    break;

                    //---���� ī�װ��� ���� Ȯ��---//
            }

            //���� ���� ����
            if (foodObject.tag.Contains("Gukbab"))
                cookGukbap.gukbapCount--;
        }
    }

    public void PickUpPajeon(GameObject foodObject)
    {
        FoodScript foodScript = foodObject.GetComponent<FoodScript>();

        if (!isCarryingFood && !foodScript.IsOnTable)
        {
            PickUpFood(foodObject);

            switch (foodObject.tag)
            {
                case "Pajeon":
                    menuCategori = "Pajeon";
                    menuValue = 1;
                    break;
            }
        }
    }


    // �տ� ��� �ִ� ������ ���̺� ���� �Լ�
    public void PlaceFoodOnTable(GameObject tableObject)
    {
        Transform tableChild = tableObject.transform.GetChild(0);

        //������ ��� ���� ��
        if (isCarryingFood && tableChild.childCount == 0)
        {
            for (int i = 0; i < tables.Length; i++)
            {
                if (tableObject.transform == tables[i])
                {
                    // --- ���� ���� �� �մ� ���̺� �ִ��� Ȯ�� --- //
                    if (data.isCustomer[i] && menuCategori.Contains(data.menuCategories[i]))
                    {
                        moveStop = true;

                        data.onTables[i] = true;
                        data.menuLV[i] = menuValue;

                        //�տ��� ������ ������
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
        Debug.Log("��� ���̺��� �ʱ�ȭ �˴ϴ�.");

        data.curSeatSize = 0;

        for (int i = 0; i < data.isAllocated.Length; i++)
        {
            data.isAllocated[i] = false;
            data.isCustomer[i] = false;
            data.onTables[i] = false;
            data.isFinEat[i] = false;

            data.menuCategories[i] = "";
            data.menuLV[i] = 0;
        }
    }
}
