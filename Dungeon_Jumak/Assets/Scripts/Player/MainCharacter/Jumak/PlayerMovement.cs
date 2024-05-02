using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
using static Unity.VisualScripting.Member;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Pathfinding;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private AIPath aiPath;
    
    [SerializeField]
    private Transform target;

    [SerializeField]
    private float delaySecond;

    private RaycastHit2D hit;
    private Vector3 direction;
    private Animator animator;//애니메이터

    private float xPos;
    private float yPos;

    private bool nextMove;

    //---텔포 예외 처리---//
    public Canvas m_canvas;

    GraphicRaycaster m_gr;
    PointerEventData m_ped;

    Vector3 curPos;
    Vector3 lastPos;

    Rigidbody2D rigidBody;

    PlayerServing playerServing;



    private void Start()
    {
        m_canvas = GameObject.Find("UI_Canvas").GetComponent<Canvas>();
        m_gr = m_canvas.GetComponent<GraphicRaycaster>();
        m_ped = new PointerEventData(null);

        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerServing = GetComponent<PlayerServing>();

        xPos = transform.position.x;
        yPos = transform.position.y;

        hit.point = transform.position;

        lastPos = transform.position;

        target.transform.position = this.transform.position;
        nextMove = false;

        //플레이어 스케일 조정
        /*
        float newScale = ((float)Screen.width / Screen.height) / 4.5f;
        transform.localScale = new Vector3(newScale, newScale, transform.localScale.z);
        */
    }

    private void Update()
    {
        MoveControl();

        SetDirection();
        SetAnimation();

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //레이 방향 설정
            hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero); //hit object 반환

            //---UI Raycast---//
            m_ped.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            m_gr.Raycast(m_ped, results);

            if (results.Count == 0)
            {
                return;
            }

            if (results[0].gameObject.name == "Home_Panel") //홈 패널을 클릭한 곳만 이동이 가능하도록 변경
            {
                target.transform.position = hit.point;
                nextMove = false;
            }
        }
    }

    void SetDirection()
    {
        //현재 위치와 방향 업데이트
        curPos = transform.position;
        direction = (curPos - lastPos).normalized;
    }

    void SetAnimation()
    {
        if (direction != Vector3.zero)
        {

            animator.SetBool("isWalk", true);

            //절댓값 y가 절댓값 x보다 더 크다면 (상하이동)
            if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
            {
                if (direction.y > 0f)
                {
                    animator.SetFloat("dirX", 0f);
                    animator.SetFloat("dirY", 1f);
                }
                else if (direction.y < 0f)
                {
                    animator.SetFloat("dirX", 0f);
                    animator.SetFloat("dirY", -1f);
                }
            }
            else if (Mathf.Abs(direction.x) >= Mathf.Abs(direction.y))
            {
                if (direction.x > 0f)
                {
                    animator.SetFloat("dirX", 1f);
                    animator.SetFloat("dirY", 0f);
                }
                else if (direction.x < 0f)
                {
                    animator.SetFloat("dirX", -1f);
                    animator.SetFloat("dirY", 0f);
                }
            }

            //최근 위치 업데이트
            lastPos = curPos;
        }
        else if (Vector3.Distance(transform.position, target.transform.position) < 0.3f)
        {
            target.transform.position = transform.position;
            animator.SetBool("isWalk", false);
        }


    }

    void MoveControl()
    {
        if (playerServing.moveStop)
        {
            playerServing.moveStop = false;
            target.transform.position = transform.position;
            animator.SetBool("isWalk", false);
        }
    }
}
