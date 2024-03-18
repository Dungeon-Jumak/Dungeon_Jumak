using UnityEngine;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    public CookGukbap cookGukbap; // 국밥 카운트 참조 스크립트

    public GameObject hand; // 플레이어 손 위치

    public bool isPlace; //음식을 두었는지 판단할 변수

    [SerializeField] 
    private FloatingJoystick joystick;
    [SerializeField] 
    private float moveSpeed; // 이동속도

    private Rigidbody2D playerRb;
    private Vector2 moveVector; // 이동벡터

    [SerializeField] 
    private bool isCarryingFood = false; // 음식을 들고 있는지 확인

    private Queue<GameObject> foodQueue = new Queue<GameObject>(); // 충돌한 Food 오브젝트를 저장하는 Queue

    private Animator animator;
    private SpriteRenderer spriter;

    //---Order System 관련---//

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
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
            // 조이스틱 입력에 따라 플레이어의 애니메이션을 설정
            if (moveVector.x > 0)
            {
                // 오른쪽 방향으로 이동
                // 애니메이터에 오른쪽 방향을 나타내는 파라미터 값을 설정
                animator.SetFloat("Horizontal", moveVector.x);
                animator.SetFloat("isWalk", 1f);
                spriter.flipX = true;
            }
            else if (moveVector.x < 0)
            {
                // 왼쪽 방향으로 이동
                // 애니메이터에 왼쪽 방향을 나타내는 파라미터 값을 설정
                animator.SetFloat("Horizontal", moveVector.x);
                animator.SetFloat("isWalk", 1f);
                spriter.flipX = false;

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
        if (other.gameObject.CompareTag("Food"))
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
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // 테이블 충돌 확인
        if (other.gameObject.CompareTag("Table_L") || other.gameObject.CompareTag("Table_R"))
        {
            Transform tableChild = other.transform.GetChild(0);

            if (isCarryingFood && tableChild.childCount == 0)
            {
                isCarryingFood = false;
                GameObject food = foodQueue.Dequeue();
                FoodScript foodScript = food.GetComponent<FoodScript>();

                food.transform.parent = tableChild;

                //테이블 위에 음식을 두었음
                //isPlace = true;

                // 음식을 테이블 위치에 고정
                food.transform.localPosition = Vector3.zero;

                // 상태를 업데이트하여 다음 충돌을 방지
                foodScript.IsOnTable = true;
            }
        }
    }


}
