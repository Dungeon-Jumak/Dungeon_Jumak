using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public bool isPlace = false;
    public bool isCarryingFood = false; // 음식을 들고 있는지 확인
    public GameObject hand; // 플레이어 손 위치
    public CookGukbap cookGukbap; // 국밥 카운트 참조 스크립트

    [SerializeField] 
    private FloatingJoystick joystick;
    [SerializeField] 
    private float moveSpeed; // 이동속도

    private Rigidbody2D playerRb;
    private Vector2 moveVector; // 이동벡터

    private Queue<GameObject> foodQueue = new Queue<GameObject>(); // 충돌한 Food 오브젝트를 저장하는 Queue

    private Animator animator;//애니메이터
    private SpriteRenderer spriter;//flip을 위한 player SpriteRenderer

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

    //---Player 이동 관련 함수---//
    private void MovePlayer()
    {
        moveVector = GetMoveVector();
        PlayFootstepSound(moveVector);
        UpdateAnimator(moveVector);

        // Rigidbody2D를 사용하여 위치를 업데이트
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
        audioManager.Stop(walkSound1);
        audioManager.Stop(walkSound2);
        audioManager.Stop(walkSound3);
        audioManager.Stop(walkSound4);
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
