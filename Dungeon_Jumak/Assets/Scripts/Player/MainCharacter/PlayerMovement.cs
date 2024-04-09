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
    public bool isCarryingFood = false; // 음식을 들고 있는지 확인

    public GameObject hand; // 플레이어 손 위치
    public CookGukbap cookGukbap; // 국밥 카운트 참조 스크립트

    private Queue<GameObject> foodQueue = new Queue<GameObject>(); // 충돌한 Food 오브젝트를 저장하는 Queue

    private Animator animator;//애니메이터

    [SerializeField]
    private Transform[] tables;

    [SerializeField]
    private Data data; // Data 스크립트

    //---서빙 관련---//
    [SerializeField]
    private int menuNumsOfHand;

    //---소리 관련---//
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

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //레이 방향 설정
            hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero); //hit object 반환
            PlayerMove();       

        }
    }

    //플레이어 이동 및 애니메이션 작동
    private void PlayerMove()
    {
        animator.SetTrigger("tpTrigger"); //트리거 작동

        Invoke("Teleport", 0.2f); //애니메이션 작동 후 티피
        Invoke("MoveDelay", delaySecond);
    }

    //플레이어 텔포
    private void Teleport()
    {
        transform.position = hit.point;
    }

    //텔포 쿨타임을 위해 Invoke로 실행하기 위한 함수
    private void MoveDelay()
    {
        isMove = false;
    }

    //---cookingSound 관련 함수---//
    private void PlayCookingSound()
    {
        if (!audioManager.IsPlaying(cookingSound))
        {
            audioManager.Play(cookingSound);
            audioManager.SetLoop(cookingSound);
            audioManager.Setvolume(cookingSound, 0.2f);
        }
    }

    // 음식을 집는 함수
    public void PickUpFood(GameObject foodObject)
    {
        FoodScript foodScript = foodObject.GetComponent<FoodScript>();

        if (!isCarryingFood && !foodScript.IsOnTable)
        {
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
                // 추가적인 음식 태그가 있다면 여기에 추가
                default:
                    Debug.LogWarning("Unhandled food tag: " + foodObject.tag);
                    break;
            }
        }
    }

    // 음식을 테이블에 놓는 함수
    public void PlaceFoodOnTable(GameObject tableObject)
    {
        Transform tableChild = tableObject.transform.GetChild(0);

        if (isCarryingFood && tableChild.childCount == 0)
        {
            for (int i = 0; i < tables.Length; i++)
            {
                if (tableObject.transform == tables[i])
                {
                    // --- 국밥 놓기 전 손님 테이블에 있는지 확인 --- //
                    if (data.isCustomer[i] && data.menuNums[i] == menuNumsOfHand)
                    {
                        data.onTables[i] = true;
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
        audioManager.Stop(cookingSound);

        Debug.Log("모든 테이블이 초기화 됩니다.");

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
