using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
using static Unity.VisualScripting.Member;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private bool isMove;
    [SerializeField]
    private float delaySecond;

    private RaycastHit2D hit;

    //---텔포 예외 처리---//
    public Canvas m_canvas;
    GraphicRaycaster m_gr;
    PointerEventData m_ped;

    private Animator animator;//애니메이터

    private void Start()
    {
        m_canvas = GameObject.Find("UI_Canvas").GetComponent<Canvas>();
        m_gr = m_canvas.GetComponent<GraphicRaycaster>();
        m_ped = new PointerEventData(null);
    }

    private void Awake()
    {
        isMove = false;
        delaySecond = 0.5f;

        animator = GetComponent<Animator>();
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(0) && !isMove)
        {
            if (isMove == true) return;


            isMove = true;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //레이 방향 설정
            hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero); //hit object 반환

            //---UI Raycast---//
            m_ped.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            m_gr.Raycast(m_ped, results);

            if (results.Count == 0)
            {
                isMove = false;
                return;
            }

            if (results[0].gameObject.name == "Home_Panel") //홈 패널을 클릭한 곳만 이동이 가능하도록 변경
                PlayerMove();
            else isMove = false;

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

}
