// System
using System.Collections.Generic;
using System;

// Unity
using UnityEngine.SceneManagement;
using static Unity.VisualScripting.Member;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// PathFinding
using Pathfinding;
using Unity.VisualScripting.Antlr3.Runtime.Tree;


//This Script For Player Moving
[DisallowMultipleComponent]
public class PlayerMovement : MonoBehaviour
{
    #region Variables

    //Player's Hand
    [Header("플레이어 손 오브젝트")]
    public Transform hand;

    [Header("플레이어 쟁반 게임 오브젝트")]
    [SerializeField] private SpriteRenderer jangbanSpriteRenderer;

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
    public Animator animator;

    //Player Serving Component for Food Serving
    private PlayerServing playerServing;

    //Tracking Object For Click Moving 
    [Header("플레이어가 자동으로 따라갈 타겟")]
    [SerializeField] private Transform target;

    //Direction Decision
    private Vector3 direction;
    private Vector3 curPos;
    private Vector3 lastPos;

    private bool isPlayingSound;

    #endregion

    private void Start()
    {
        //Initialize Variables
        m_gr = m_canvas.GetComponent<GraphicRaycaster>();
        m_ped = new PointerEventData(null);

        hit.point = transform.position;

        //Add Component
        animator = GetComponent<Animator>();
        playerServing = GetComponent<PlayerServing>();

        //Initialize Location Variables
        target.transform.position = this.transform.position;

        curPos = transform.position;
        lastPos = transform.position;

        //Initialize Start Animation
        animator.SetFloat("dirX", 0f);
        animator.SetFloat("dirY", -1f);

        //Sound Setting
        GameManager.Sound.Play("[S] Walk Sound1", Define.Sound.Effect, true);
        GameManager.Sound.Pause("[S] Walk Sound1", Define.Sound.Effect);

        isPlayingSound = false;
    }

    private void Update()
    {
        //Checking touch HomePanel
        CheckTouchPanel();

        //Moving (Change Target Transform)
        //Player track target to auto
        MoveControl();

        //Method for Decision Direction
        SetDirection();
    }

    //this is method for checking to touch panel
    private void CheckTouchPanel()
    {
        if (Input.GetMouseButtonDown(0))
        {
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
            if (results[0].gameObject.name == "[Panel] Jumak")
            {
                //Play Animation
                clickAnim.SetTrigger("click");

                //Target for tracking 's positon change to hit point
                target.transform.position = hit.point;
            }
            else return;
        }
    }

    //Method for moving control
    void MoveControl()
    {
        if (playerServing.moveStop)
        {
            playerServing.moveStop = false;
            target.transform.position = transform.position;
            animator.SetBool("isWalk", false);
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
            //Sound Play
            if (!isPlayingSound)
            {
                isPlayingSound = true;

                GameManager.Sound.Resume("[S] Walk Sound1", Define.Sound.Effect);
            }


            //Active isWalk
            animator.SetBool("isWalk", true);

            //up-down move
            if (Mathf.Abs(direction.x) + 0.2f < Mathf.Abs(direction.y))
            {
                //To up
                if (direction.y > 0f)
                { 
                    //Play Moving Up Animation
                    animator.SetFloat("dirX", 0f);
                    animator.SetFloat("dirY", 1f);

                    //Change hand position
                    if (hand.localPosition.x < 0f)
                        hand.localPosition = new Vector3(hand.localPosition.x * -1, hand.localPosition.y, hand.localPosition.z);

                    jangbanSpriteRenderer.sortingLayerName = "Food_Down";
                    
                }
                //To down
                else if (direction.y <= 0f)
                {
                    //Play Moving Down Animaion
                    animator.SetFloat("dirX", 0f);
                    animator.SetFloat("dirY", -1f);

                    //Change hand position
                    if (hand.localPosition.x < 0f)
                        hand.localPosition = new Vector3(hand.localPosition.x * -1, hand.localPosition.y, hand.localPosition.z);

                    jangbanSpriteRenderer.sortingLayerName = "Food_Up";
                }
            }
            //left-right move
            else if (Mathf.Abs(direction.x) >= Mathf.Abs(direction.y) + 0.2f)
            {
                //To right
                if (direction.x >= 0f)
                {
                    //Play Moving Right Animation
                    animator.SetFloat("dirX", 1f);
                    animator.SetFloat("dirY", 0f);

                    //Change hand position
                    if (hand.localPosition.x > 0f)
                        hand.localPosition = new Vector3(hand.localPosition.x * -1, hand.localPosition.y, hand.localPosition.z);
                }
                //To left
                else if (direction.x < 0f)
                {
                    //Play Moving Left Animation
                    animator.SetFloat("dirX", -1f);
                    animator.SetFloat("dirY", 0f);

                    //Change hand postion
                    if (hand.localPosition.x < 0f)
                        hand.localPosition = new Vector3(hand.localPosition.x * -1, hand.localPosition.y, hand.localPosition.z);
                }
            }
        }
        else
        {
            GameManager.Sound.Pause("[S] Walk Sound1", Define.Sound.Effect);

            isPlayingSound = false;
        }

        //To stop around target
        if (Vector3.Distance(transform.position, target.transform.position) < 0.3f)
        {
            //Changet target position to stop player
            target.transform.position = transform.position;

            //Inactiove Walk Animation
            animator.SetBool("isWalk", false);
        }
    }



}
