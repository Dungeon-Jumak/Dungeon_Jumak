// System
using System.Collections.Generic;
using System;

// Unity
using UnityEngine.SceneManagement;
using static Unity.VisualScripting.Member;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// This is Script For Player Serving
[DisallowMultipleComponent]
public class PlayerServing : MonoBehaviour
{
    #region Variables
    //Check Carry Food
    [Header("������ ��� �ִ��� Ȯ���ϱ� ���� ����")]
    public bool isCarryingFood = false;

    //To Stop Player Move
    [Header("�÷��̾��� �������� �����ϱ� ���� ����")]
    public bool moveStop;

    //Table Array
    [Header("��� ���̺� �迭")]
    public Transform[] tables;

    //Player Movement Component
    private PlayerMovement playerMovement;

    //CookGukBab Component
    [Header("���� �ڵ� ���� ���� ��ũ��Ʈ")]
    [SerializeField] private CookGukbap cookGukbap;

    //Categori of Menu
    [Header("���� ī�װ�")]
    [SerializeField] private string menuCategori;
    //Value of Menu
    [Header("���� ����")]
    [SerializeField] private int menuValue;

    //Queue for Collision Food Object
    [Header("���� ��� ���� �Ͱ� ���õ� ť")]
    [SerializeField] private Queue<GameObject> foodQueue = new Queue<GameObject>();

    //Animator Componet
    private Animator animator;//�ִϸ�����

    //Data
    private Data data;
    #endregion

    private void Start()
    {
        //Add Component
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();

        //Assignment Data
        data = DataManager.Instance.data;
    }

    //Method of DataInitialize
    public void DataInitialize()
    {
        //Debug Log Initial Table
        Debug.Log("��� ���̺��� �ʱ�ȭ �˴ϴ�.");

        //Initialize Customer Count 
        data.customerHeadCount = 0;

        //Initialize All Data
        for (int i = 0; i < tables.Length; i++)
        {
            //variable for allocate seat
            data.isAllocated[i] = false;

            //variable for check customer sit
            data.isCustomer[i] = false;

            //variable for check on table
            data.onTables[i] = false;

            //variable for check customer eating
            data.isFinEat[i] = false;

            //variable for categori of menu
            data.menuCategories[i] = "";

            //variable for level of menu
            data.menuLV[i] = 0;
        }
    }

    #region Methods of PickUp Food
    //Base Method for PickUp Food
    public void PickUpFood(GameObject foodObject)
    {
        //Enable Move Stop when Pickup fdood
        moveStop = true;

        //Activate IsCarrying
        isCarryingFood = true;

        //Play Serving Animation
        animator.SetBool("isServing", true);

        //Change Layer When PickUpFood
        foodObject.layer = 7;
        
        //Enque foodObject in foodQueue
        foodQueue.Enqueue(foodObject);

        //Set Parent foodObject transform to Player hand's transform
        foodObject.transform.parent = playerMovement.hand.transform; 
        foodObject.transform.localPosition = Vector3.zero;
    }

    //Method for PickUp GukBab at Serving Table
    public void PickUpGukBab(GameObject foodObject)
    {
        FoodScript foodScript = foodObject.GetComponent<FoodScript>();

        //if is not carrying food
        if (!isCarryingFood && !foodScript.IsOnTable)
        {
            //Call PickUpFood Method
            PickUpFood(foodObject);

            //Switch about gukbab's tag for add value
            switch (foodObject.tag)
            {
                case "Gukbab":
                    menuCategori = "Gukbab";
                    menuValue = 1; 
                    break;
                case "PigGukbab":
                    menuCategori = "Gukbab";
                    menuValue = 2;
                    break;
                    //add other gukbab!
            }

            //Decrease Gukbab Count
            if (foodObject.tag.Contains("Gukbab")) cookGukbap.gukbapCount--;
        }
    }

    //Method for PickUp Pajeon at MiniGame
    public void PickUpPajeon(GameObject foodObject)
    {
        FoodScript foodScript = foodObject.GetComponent<FoodScript>();

        //if is not carrying food
        if (!isCarryingFood && !foodScript.IsOnTable)
        {
            //Call PickUpFood Method
            PickUpFood(foodObject);

            //Switch about pajeon's tag for add value
            switch (foodObject.tag)
            {
                case "Pajeon":
                    menuCategori = "Pajeon";
                    menuValue = 1;
                    break;
                    //add other paejon!
            }
        }
    }
    #endregion

    #region Methods of Hand off Food
    // Method PlaceFood On Table
    public void PlaceFoodOnTable(GameObject tableObject)
    {
        // if is carry food
        if (isCarryingFood)
        {
            // loop on tables length
            for (int i = 0; i < tables.Length; i++)
            {
                // find collision table and check table is empty,
                // check customer is sitting table and check categori of menu
                if (tableObject.transform == tables[i] && !data.onTables[i] && data.isCustomer[i] && menuCategori.Contains(data.menuCategories[i]))
                {
                    // Stop Player Movement
                    moveStop = true;

                    // Update onTables
                    data.onTables[i] = true;

                    // Update MenuValue
                    data.menuLV[i] = menuValue;

                    // Disenable isCarryingfood -> player's hand is empty
                    isCarryingFood = false;

                    // Stop Player Serving Animation
                    animator.SetBool("isServing", false);

                    // Deque foodQueue -> hand off first pick food
                    GameObject food = foodQueue.Dequeue();

                    // Get SpriteRenderer of food
                    SpriteRenderer foodSpriteRenderer = food.GetComponent<SpriteRenderer>();

                    // Update foodSprite renderer
                    foodSpriteRenderer.sortingLayerName = "Food_Down";

                    // Get food script
                    FoodScript foodScript = food.GetComponent<FoodScript>();

                    // Actiove IsOnTable -> Avoid Duplication
                    foodScript.IsOnTable = true;

                    // Update food's parent
                    food.transform.parent = tables[i].transform.GetChild(0);

                    // Fix food location to zero vector
                    food.transform.localPosition = Vector3.zero;
                }
            }
        }
    }

    // Method of ThrowAwayFood
    public void ThrowAwayFood()
    {
        //if player is carrying food and 
        if (isCarryingFood && foodQueue.Count != 0)
        {
            //Inactive isCarryingFood
            isCarryingFood = false;

            //Deque foodQueue
            GameObject food = foodQueue.Dequeue();

            //Destroy food
            Destroy(food);
        }
    }
    #endregion
}
