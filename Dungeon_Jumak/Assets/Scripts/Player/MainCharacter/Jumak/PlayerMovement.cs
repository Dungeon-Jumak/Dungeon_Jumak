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

    [SerializeField]
    private Transform hand;

    private RaycastHit2D hit;
    private Vector3 direction;
    private Animator animator;//애니메이터

    private float xPos;
    private float yPos;


    //---텔포 예외 처리---//
    public Canvas m_canvas;

    public Animator clickAnim;

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

        animator.SetFloat("dirX", 0f);
        animator.SetFloat("dirY", -1f);
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
                clickAnim.SetTrigger("click");
                target.transform.position = hit.point;
            }
        }
    }

    void SetDirection()
    {
        //현재 위치와 방향 업데이트
        curPos = transform.position;
        direction = (curPos - lastPos).normalized;

        lastPos = curPos;
        
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
                    //위로 가는 모션
                    /*
                    animator.SetFloat("dirX", 0f);
                    animator.SetFloat("dirY", 1f);

                    if (hand.localPosition.x < 0f)
                        hand.localPosition = new Vector3(hand.localPosition.x * -1, hand.localPosition.y, hand.localPosition.z);
                    */

                    //뒷 모습을 빼는건 어떤가?
                    if (direction.x >= 0.1f)
                    {
                        animator.SetFloat("dirX", 1f);
                        animator.SetFloat("dirY", 0f);

                        if (hand.localPosition.x > 0f)
                            hand.localPosition = new Vector3(hand.localPosition.x * -1, hand.localPosition.y, hand.localPosition.z);
                    }
                    else if (direction.x < 0.1f)
                    {
                        animator.SetFloat("dirX", -1f);
                        animator.SetFloat("dirY", 0f);

                        if (hand.localPosition.x < 0f)
                            hand.localPosition = new Vector3(hand.localPosition.x * -1, hand.localPosition.y, hand.localPosition.z);
                    }
                }
                else if (direction.y <= 0f)
                {
                    animator.SetFloat("dirX", 0f);
                    animator.SetFloat("dirY", -1f);

                    if (hand.localPosition.x < 0f)
                        hand.localPosition = new Vector3(hand.localPosition.x * -1, hand.localPosition.y, hand.localPosition.z);
                }
            }
            else if (Mathf.Abs(direction.x) >= Mathf.Abs(direction.y))
            {
                if (direction.x >= 0f)
                {
                    animator.SetFloat("dirX", 1f);
                    animator.SetFloat("dirY", 0f);

                    if (hand.localPosition.x > 0f)
                        hand.localPosition = new Vector3(hand.localPosition.x * -1, hand.localPosition.y, hand.localPosition.z);
                }
                else if (direction.x < 0f)
                {
                    animator.SetFloat("dirX", -1f);
                    animator.SetFloat("dirY", 0f);

                    if (hand.localPosition.x < 0f)
                        hand.localPosition = new Vector3(hand.localPosition.x * -1, hand.localPosition.y, hand.localPosition.z);
                }
            }
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
