//PathFinding
using Pathfinding;

//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class PlayerMovementInDungeon : MonoBehaviour
{
    #region Variables

    //For Distinguish Click Ui and Object
    //For Click Exception Handilng
    [Header("캔버스")]
    [SerializeField] private Canvas m_canvas;
    //GraphicRaycaster for Click
    private GraphicRaycaster m_gr;
    //PointerEventData for Click
    private PointerEventData m_ped;

    //RayCastHHIt2D
    private RaycastHit2D hit;

    //AiPath For Click Moving
    [Header("자동으로 움직이기 위한 AI Path")]
    [SerializeField] private AIPath aiPath;

    //Click Animation Component
    [Header("클릭했을 때 나오는 커서 애니메이션")]
    [SerializeField] private Animator clickAnim;

    //Player Animator Component
    private Animator animator;

    //Tracking Object For Click Moving 
    [Header("플레이어가 자동으로 따라갈 타겟")]
    [SerializeField] private Transform target;

    [Header("클릭된지 확인할 패널")]
    [SerializeField] private GameObject panel;

    //Direction Decision
    public Vector3 direction;
    private Vector3 curPos;
    private Vector3 lastPos;

    //Player's xPos
    private float xPos;

    //Player's yPos
    private float yPos;

    private float timer;

    #endregion

    private void Start()
    {
        //Initialize Variables
        m_gr = m_canvas.GetComponent<GraphicRaycaster>();
        m_ped = new PointerEventData(null);

        hit.point = transform.position;

        //Add Component
        animator = GetComponent<Animator>();

        //Initialize Location Variables
        target.transform.position = this.transform.position;

        curPos = transform.position;
        lastPos = transform.position;

        xPos = transform.position.x;
        yPos = transform.position.y;

        //Initialize Start Animation
        animator.SetFloat("dirX", 0f);
        animator.SetFloat("dirY", -1f);
    }

    private void Update()
    {
        //Checking touch HomePanel
        CheckTouchPanel();

        //Method for Decision Direction
        SetDirection();
    }

    //this is method for checking to touch panel
    private void CheckTouchPanel()
    {
        if (Input.GetMouseButton(0))
        {
            //Increase timer
            timer += Time.deltaTime;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (timer < 0.2f)
            {
                //Init timer
                timer = 0f;

                //Decision ray direction
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                //Return hit object
                hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                //UI raycasting
                m_ped.position = Input.mousePosition;
                List<RaycastResult> results = new List<RaycastResult>();
                m_gr.Raycast(m_ped, results);

                //Exception handling
                if (results.Count == 0)
                {
                    return;
                }

                //Check Click Panel
                if (results[0].gameObject.name == "[Panel] Stage 1")
                {
                    //Play Animation
                    clickAnim.SetTrigger("click");

                    //Target for tracking 's positon change to hit point
                    target.transform.position = hit.point;
                }
                else return;
            }
            else
                timer = 0f;
        }
    }

    //Method for set direction
    void SetDirection()
    {
        //Update Current location and Direction
        curPos = transform.localPosition;
        direction = (curPos - lastPos).normalized;

        //Assignment curPos to lastPos (Update lastPos)
        lastPos = curPos;

        //Set Animation
        SetAnimation();
    }

    //Method for set animation
    void SetAnimation()
    {
        //direction is not zero vector
        if (direction != Vector3.zero)
        {
            //Active isWalk
            animator.SetBool("isWalk", true);

            //up-down move
            if (Mathf.Abs(direction.x) + 0.6f < Mathf.Abs(direction.y))
            {
                //To up
                if (direction.y > 0f)
                {
                    //Play Moving Up Animation
                    animator.SetFloat("dirX", 0f);
                    animator.SetFloat("dirY", 1f);
                }
                //To down
                else if (direction.y <= 0f)
                {
                    //Play Moving Down Animaion
                    animator.SetFloat("dirX", 0f);
                    animator.SetFloat("dirY", -1f);
                }
            }
            //left-right move
            else
            {
                //To right
                if (direction.x >= 0f)
                {
                    //Play Moving Right Animation
                    animator.SetFloat("dirX", 1f);
                    animator.SetFloat("dirY", 0f);
                }
                //To left
                else if (direction.x < 0f)
                {
                    //Play Moving Left Animation
                    animator.SetFloat("dirX", -1f);
                    animator.SetFloat("dirY", 0f);
                }
            }
        }

        //To stop around target
        if (Vector3.Distance(transform.position, target.transform.position) < 0.1f)
        {
            //Changet target position to stop player
            target.transform.position = transform.position;

            //Inactiove Walk Animation
            animator.SetBool("isWalk", false);
        }
    }


}
