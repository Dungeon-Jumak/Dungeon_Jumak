using UnityEngine;
using System.Collections.Generic;

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

        // 조이스틱 입력을 기반으로 플레이어의 이동 방향을 설정
        if (moveVector != Vector2.zero)
        {
            float angle = Mathf.Atan2(moveVector.y, moveVector.x) * Mathf.Rad2Deg;
            // 4개의 구역으로 나누어 핸들 돌리기
            if (angle > -45f && angle <= 45f)
            {
                // 오른쪽 방향
                // 애니메이터에 오른쪽 방향을 나타내는 파라미터 값을 설정
                animator.SetFloat("Horizontal", 1f);
                animator.SetFloat("Vertical", 0f);

                animator.SetFloat("isWalk", 1f);
            }
            else if (angle > 45f && angle <= 135f)
            {
                // 위쪽 방향
                // 애니메이터에 위쪽 방향을 나타내는 파라미터 값을 설정
                animator.SetFloat("Horizontal", 0f);
                animator.SetFloat("Vertical", 1f);

                animator.SetFloat("isWalk", 1f);
            }
            else if (angle > 135f || angle <= -135f)
            {
                // 왼쪽 방향
                // 애니메이터에 왼쪽 방향을 나타내는 파라미터 값을 설정
                animator.SetFloat("Horizontal", -1f);
                animator.SetFloat("Vertical", 0f);

                animator.SetFloat("isWalk", 1f);
            }
            else if (angle > -135f && angle <= -45f)
            {
                // 아래쪽 방향
                // 애니메이터에 아래쪽 방향을 나타내는 파라미터 값을 설정
                animator.SetFloat("Vertical", -1f);
                animator.SetFloat("Horizontal", 0f);

                animator.SetFloat("isWalk", 1f);
            }
        }
        else
        {
            // 조이스틱 입력이 없을 때, 정지 상태로 애니메이션을 설정
            animator.SetFloat("isWalk", -1f);
        }
        // Rigidbody2D를 사용하여 위치를 업데이트
        playerRb.MovePosition(playerRb.position + moveVector);
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        // 음식 충돌 확인
        if (other.gameObject.CompareTag("Gukbap"))
        {
            FoodScript foodScript = other.GetComponent<FoodScript>();

            if (!isCarryingFood && !foodScript.IsOnTable)
            {
                foodQueue.Enqueue(other.gameObject); // 충돌한 음식을 Queue에 저장
                other.transform.parent = hand.transform; // 플레이어 손 아래로 이동
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
                foodQueue.Enqueue(other.gameObject); // 충돌한 음식을 Queue에 저장
                other.transform.parent = hand.transform; // 플레이어 손 아래로 이동
                other.transform.localPosition = Vector3.zero;
                isCarryingFood = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // 테이블 충돌 확인
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

                        // --- 국밥 놓기 전 손님 테이블에 있는지 확인 --- //
                        if (data.isCustomer[i])
                        {
                            isCarryingFood = false;
                            GameObject food = foodQueue.Dequeue();
                            FoodScript foodScript = food.GetComponent<FoodScript>();

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
