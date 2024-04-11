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

    //---���� ���� ó��---//
    public Canvas m_canvas;
    GraphicRaycaster m_gr;
    PointerEventData m_ped;

    private Animator animator;//�ִϸ�����

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

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //���� ���� ����
            hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero); //hit object ��ȯ

            //---UI Raycast---//
            m_ped.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            m_gr.Raycast(m_ped, results);

            if (results.Count == 0)
            {
                isMove = false;
                return;
            }

            if (results[0].gameObject.name == "Home_Panel") //Ȩ �г��� Ŭ���� ���� �̵��� �����ϵ��� ����
                PlayerMove();
            else isMove = false;

        }
    }

    //�÷��̾� �̵� �� �ִϸ��̼� �۵�
    private void PlayerMove()
    {
        animator.SetTrigger("tpTrigger"); //Ʈ���� �۵�

        Invoke("Teleport", 0.2f); //�ִϸ��̼� �۵� �� Ƽ��
        Invoke("MoveDelay", delaySecond);
    }

    //�÷��̾� ����
    private void Teleport()
    {
        transform.position = hit.point;
    }

    //���� ��Ÿ���� ���� Invoke�� �����ϱ� ���� �Լ�
    private void MoveDelay()
    {
        isMove = false;
    }

}
