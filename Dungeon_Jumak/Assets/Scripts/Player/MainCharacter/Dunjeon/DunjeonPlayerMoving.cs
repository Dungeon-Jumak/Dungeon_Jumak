using UnityEngine;
using System.Collections.Generic;

public class DunjeonPlayerMoving : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 endPosition;
    private float dragDistance;  // �巡�� �Ÿ��� �Ǵ��ϱ� ���� �ּ� �Ÿ�
    private Animator animator;//�ִϸ�����

    public int laneNumber = 1;  // ���� �÷��̾ �ִ� ���� ��ȣ (0: ����, 1: �߾�, 2: ������)
    public int lanesCount = 3;  // �� ���� ��

    // ���� �� ��ġ ����
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
        transform.position = lanePositions[laneNumber];// ���� ���� �� �÷��̾ 1������ ��ġ�� �ʱ�ȭ
    }

    void Update()
    {
        // ��ġ �Է� ó��
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // ù ��° ��ġ
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
        //���콺 �Է� ó��
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
        // �巡�� �Ÿ� ���
        dragDistance = endPosition.x - startPosition.x;

        if (Mathf.Abs(dragDistance) > 100) // �巡�� �Ÿ��� ����� ū ��쿡�� �̵� ó��
        {
            if (dragDistance > 0 && laneNumber < lanesCount - 1)
            {
                // ���������� �巡���ϸ� ���� ��ȣ ����
                laneNumber++;
            }
            else if (dragDistance < 0 && laneNumber > 0)
            {
                // �������� �巡���ϸ� ���� ��ȣ ����
                laneNumber--;
            }
            MovePlayer();
        }
    }

    void MovePlayer()
    {
        // ������ ���� ��ġ�� �÷��̾� �̵�
        Vector3 newPosition = lanePositions[laneNumber];
        transform.position = newPosition;
    }
    
}