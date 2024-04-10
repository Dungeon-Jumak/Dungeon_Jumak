using UnityEngine;
using System.Collections.Generic;

public class DunjeonPlayerMoving : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 endPosition;
    private float dragDistance;  // 드래그 거리를 판단하기 위한 최소 거리
    private Animator animator;//애니메이터

    public int laneNumber = 1;  // 현재 플레이어가 있는 레인 번호 (0: 왼쪽, 1: 중앙, 2: 오른쪽)
    public int lanesCount = 3;  // 총 레인 수

    // 레인 별 위치 설정
    private Vector3[] lanePositions = {
        new Vector3(-3.7f, -1.3f, 13.5f),
        new Vector3(-3.7f, -1.3f, 13f),
        new Vector3(-3.7f, -1.3f, 12.5f)
    };

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetFloat("isWalk", 1f);
        animator.SetFloat("Vertical", 1f);
        transform.position = lanePositions[laneNumber];// 게임 시작 시 플레이어를 1레인의 위치로 초기화
    }

    void Update()
    {
        // 터치 입력 처리
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // 첫 번째 터치
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startPosition = touch.position;
                    break;
                case TouchPhase.Ended:
                    endPosition = touch.position;
                    DragPlayer();
                    break;
            }
        }
        //마우스 입력 처리
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                startPosition = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(0))
            {
                endPosition = Input.mousePosition;
                DragPlayer();
            }
        }
    }

    void DragPlayer()
    {
        // 드래그 거리 계산
        dragDistance = endPosition.x - startPosition.x;

        if (Mathf.Abs(dragDistance) > 100) // 드래그 거리가 충분히 큰 경우에만 이동 처리
        {
            if (dragDistance > 0 && laneNumber < lanesCount - 1)
            {
                // 오른쪽으로 드래그하면 레인 번호 증가
                laneNumber++;
            }
            else if (dragDistance < 0 && laneNumber > 0)
            {
                // 왼쪽으로 드래그하면 레인 번호 감소
                laneNumber--;
            }
            MovePlayer();
        }
    }

    void MovePlayer()
    {
        // 지정된 레인 위치로 플레이어 이동
        Vector3 newPosition = lanePositions[laneNumber];
        transform.position = newPosition;
    }
    
}