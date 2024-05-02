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
    //---서빙 관련---//
    [SerializeField]
    private string menuCategori;
    [SerializeField]
    private int menuValue;

    //---소리 관련---//
    [SerializeField]
    private AudioManager audioManager;
    [SerializeField]
    private string servingSound;
    [SerializeField]
    private string pickUpSound;
    [SerializeField]
    private string trashCanSound;

    private Queue<GameObject> foodQueue = new Queue<GameObject>(); // 충돌한 Food 오브젝트를 저장하는 Queue

    private Animator animator;//애니메이터

    public bool isPlace = false;
    public bool isCarryingFood = false; // 음식을 들고 있는지 확인
    public Transform[] tables;

    public GameObject hand; // 플레이어 손 위치
    public CookGukbap cookGukbap; // 국밥 카운트 참조 스크립트

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

        //음식 잡는 사운드 재생
        audioManager.Play(pickUpSound);

        // 음식 드는 순간 srpite renderer 레이어 Food_Up으로 변경
        SpriteRenderer otherSpriteRenderer = foodObject.GetComponent<SpriteRenderer>();
        otherSpriteRenderer.sortingLayerName = "Food_Up";

        foodObject.layer = 7;
        foodQueue.Enqueue(foodObject); // 충돌한 음식을 Queue에 저장
        foodObject.transform.parent = hand.transform; // 플레이어 손 아래로 이동
        foodObject.transform.localPosition = Vector3.zero;
        isCarryingFood = true;
    }

    public void PickUpGukBab(GameObject foodObject)
    {
        FoodScript foodScript = foodObject.GetComponent<FoodScript>();

        //손에 음식을 들고 있지 않다면
        if (!isCarryingFood && !foodScript.IsOnTable)
        {
            PickUpFood(foodObject);

            //국밥의 종류에 따라 벨류를 다르게 설정하기 위함
            switch (foodObject.tag)
            {
                case "Gukbab":
                    menuCategori = "Gukbab";
                    menuValue = 1; //태그별로 음식의 밸류 설정
                    break;

                    //---국밥 카테고리의 음식 확장---//
            }

            //국밥 개수 감소
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


    // 손에 들고 있는 음식을 테이블에 놓는 함수
    public void PlaceFoodOnTable(GameObject tableObject)
    {
        Transform tableChild = tableObject.transform.GetChild(0);

        //음식을 들고 있을 때
        if (isCarryingFood && tableChild.childCount == 0)
        {
            for (int i = 0; i < tables.Length; i++)
            {
                if (tableObject.transform == tables[i])
                {
                    // --- 국밥 놓기 전 손님 테이블에 있는지 확인 --- //
                    if (data.isCustomer[i] && menuCategori.Contains(data.menuCategories[i]))
                    {
                        moveStop = true;

                        data.onTables[i] = true;
                        data.menuLV[i] = menuValue;

                        //손에서 음식이 떨어짐
                        isCarryingFood = false;

                        //음식 놓는 사운드 재생
                        audioManager.Play(servingSound);

                        GameObject food = foodQueue.Dequeue();
                        FoodScript foodScript = food.GetComponent<FoodScript>();

                        //음식 Sprite Renderer 레이어 Food_Down으로 변경
                        SpriteRenderer foodSpriteRenderer = food.GetComponent<SpriteRenderer>();
                        foodSpriteRenderer.sortingLayerName = "Food_Down";

                        food.transform.parent = tableChild;

                        // 음식을 테이블 위치에 고정
                        food.transform.localPosition = Vector3.zero;

                        // 상태를 업데이트하여 다음 충돌을 방지
                        foodScript.IsOnTable = true;
                    }
                }
            }
        }
    }

    // 음식을 버리는 함수
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

    //씬이 변경될 때 현재 데이터 값 초기화
    public void DataInitialize()
    {
        Debug.Log("모든 테이블이 초기화 됩니다.");

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
