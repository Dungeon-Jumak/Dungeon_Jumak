using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class PlayerMovement_Dun : MonoBehaviour
{
    #region Variables

    [Header("캔버스")]
    [SerializeField] private Canvas m_canvas;

    //Tracking Object For Click Moving 
    [Header("플레이어가 자동으로 따라갈 타겟")]
    [SerializeField] private Transform target;

    [Header("클릭된지 확인할 패널")]
    [SerializeField] private GameObject panel;

    [Header("이동 방향")]
    [SerializeField] private Vector3 direction;

    [Header("플레이어 이동 속도")]
    public float moveSpeed = 1.5f;

    private GraphicRaycaster m_gr;
    private PointerEventData m_ped;
    private RaycastHit2D hit;
    private Animator animator;
    private Vector3 curPos;
    private Vector3 lastPos;
    private float xPos;
    private float yPos;
    private float timer;
    private bool isMoving = false;

    #endregion

    private void Start()
    {
        m_gr = m_canvas.GetComponent<GraphicRaycaster>();
        m_ped = new PointerEventData(null);
        hit.point = transform.position;

        //Add Component
        animator = GetComponent<Animator>();

        //---Initialize Location Variables---//
        target.transform.position = this.transform.position;
        curPos = transform.position;
        lastPos = transform.position;
        xPos = transform.position.x;
        yPos = transform.position.y;

        //---Initialize Start Animation---//
        animator.SetFloat("dirX", -1f);
        animator.SetFloat("dirY", 0f);
    }

    private void Update()
    {
        CheckTouchPanel();

        if (isMoving)
        {
            SetDirection();
            MoveTowardsTarget();
        }
    }

    private void CheckTouchPanel()
    {
        if (Input.GetMouseButton(0))
        {
            timer += Time.deltaTime;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (timer < 0.2f)
            {
                timer = 0f;
                isMoving = true;

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                m_ped.position = Input.mousePosition;
                List<RaycastResult> results = new List<RaycastResult>();
                m_gr.Raycast(m_ped, results);

                if (results.Count == 0) return;

                if (results[0].gameObject.name == "[Panel] Stage 1")
                {
                    target.transform.position = hit.point;
                }
            }
            else
            {
                timer = 0f;
            }
        }
    }

    void SetDirection()
    {
        curPos = transform.localPosition;
        direction = (curPos - lastPos).normalized;
        lastPos = curPos;

        SetAnimation();
    }

    void SetAnimation()
    {
        if (direction != Vector3.zero)
        {
            animator.SetBool("isWalk", true);

            // Calculate the angle in degrees
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Check direction based on angle
            if (angle > -90 && angle <= 90)
            {
                // Right
                animator.SetFloat("dirX", 1f);
                animator.SetFloat("dirY", 0f);
            }
            else
            {
                // Left
                animator.SetFloat("dirX", -1f);
                animator.SetFloat("dirY", 0f);
            }
        }
        else
        {
            animator.SetBool("isWalk", false);
        }
    }

    void MoveTowardsTarget()
    {
        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            isMoving = false;
            animator.SetBool("isWalk", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isMoving = false;
        animator.SetBool("isWalk", false);
    }
}
