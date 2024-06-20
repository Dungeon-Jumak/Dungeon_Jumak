//System
using Pathfinding;
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;

[DisallowMultipleComponent]
public class MonsterController : MonoBehaviour
{
    [Header("플레이어가 근처에 있는지 판단하기 위한 Bool 변수")]
    public bool isTrack;

    [Header("몬스터의 패드롤 이동 속도")]
    [SerializeField] private float patrolSpeed;

    [Header("추적할 타겟의 Rigidbody")]
    [SerializeField] private Rigidbody2D target;

    [Header("몬스터의 추적 이동 속도")]
    [SerializeField] private float trackingSpeed;

    [Header("몬스터의 체력")]
    [SerializeField] private float health;

    [Header("몬스터의 최대 체력")]
    [SerializeField] private float maxHealth;

    private AIDestinationSetter destination;
    private AIPath aiPath;

    //Rigidbody 2D
    private Rigidbody2D rigid;

    //Sprite Renderer
    private SpriteRenderer spriteRenderer;

    //isLive Sign
    private bool isLive;

    //isMove Sign
    private bool isMove;

    //moveVector
    private Vector3 moveVector;

    private void OnEnable()
    {
        //Initialize
        target = FindObjectOfType<PlayerMovementInDungeon>().GetComponent<Rigidbody2D>();

        //isLive true
        isLive = true;

        //Init health
        health = maxHealth;

        isTrack = true;
    }

    private void Start()
    {
        //Get Component
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        destination = GetComponent<AIDestinationSetter>();
        aiPath = GetComponent<AIPath>();

        destination.enabled = false;
        aiPath.enabled = false;

        //Initialize
        isMove = false;

        //Start Patrol Coroutine
        StartCoroutine(RandomPatrol());
    }

    //Fixed Update : For Rigid Controll
    private void FixedUpdate()
    {
        //Run only isLive
        if (!isLive)
            return;

        //To prevent physical speed from affecting movement
        rigid.velocity = Vector2.zero;

        //if monster is tracking player
        if (isTrack)
        {
            //Setting aipath scipts
            destination.enabled = true;
            aiPath.enabled = true;

            //Set Target
            destination.target = target.transform;

            //Set Speed
            aiPath.maxSpeed = trackingSpeed;
        }
        else
        {
            //if isMove is true
            if (isMove)
            {
                Move();
            }
        }
    }

    //Late Update
    private void LateUpdate()
    {
        //Run only isLive
        if (!isLive)
            return;

        //if is tracking
        if (isTrack)
        {
            //Location comparison with target
            if (target.position.x < rigid.position.x)
                spriteRenderer.flipX = false;
            else
                spriteRenderer.flipX = true;
        }
        //if is not tracking
        else
        {
            //Location comparison with direction
            if (moveVector.x > 0)
                spriteRenderer.flipX = true;
            else
                spriteRenderer.flipX = false;
        }
    }

    //Random Patrol Coroutine
    IEnumerator RandomPatrol()
    {
        //if mode is changed, stop all coroutines...
        if (isTrack) StopAllCoroutines();

        isMove = true;

        //0 : Up, 1 : Down, 2 : Right, 3 : Left, 4 : Stop
        int random = Random.Range(0, 5);

        //Set Direction
        switch (random)
        {
            case 0:
                moveVector = Vector2.up;
                break;

            case 1:
                moveVector = Vector2.down;
                break;

            case 2:
                moveVector = Vector2.right;
                break;

            case 3:
                moveVector = Vector2.left;
                break;

            case 4:
                moveVector = Vector2.zero;
                break;

            default:
                break;
        }

        //Random Delay
        yield return new WaitForSeconds(Random.Range(1f, 3f));

        //Convert isMove
        isMove = false;

        yield return new WaitForSeconds(Random.Range(0f, 2f));

        //StartCoroutine : recursion
        StartCoroutine(RandomPatrol());
    }

    private void Move()
    {
        //Set New Vector
        Vector2 newVector = moveVector * patrolSpeed * Time.deltaTime;

        //Rigid Move position
        rigid.MovePosition(rigid.position + newVector);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Attack"))
            return;

        //Damage !
        health -= collision.GetComponent<Skills>().damage;

        if (health > 0)
        {
            //Live
        }
        else
        {
            //Die
            Dead();
        }
    }

    private void Dead()
    {
        gameObject.SetActive(false);
    }
}
