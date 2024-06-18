//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;

[DisallowMultipleComponent]
public class MonsterController : MonoBehaviour
{
    [Header("플레이어가 근처에 있는지 판단하기 위한 Bool 변수")]
    public bool isTrack;

    [Header("몬스터의 이동 속도")]
    [SerializeField] private float speed;

    [Header("추적할 타겟의 Rigidbody")]
    [SerializeField] private Rigidbody2D target;

    //Rigidbody 2D
    private Rigidbody2D rigid;

    //Sprite Renderer
    private SpriteRenderer spriteRenderer;

    //isLive Sign
    private bool isLive = true;

    //isMove Sign
    private bool isMove;

    private Vector3 moveVector;

    private void Start()
    {
        //Get Component
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

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
            //Check direction vector
            Vector2 dirVec = target.position - rigid.position;

            //Compute vector for move
            Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;

            //Move monster
            rigid.MovePosition(rigid.position + nextVec);
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
        Vector2 newVector = moveVector * speed * Time.deltaTime;

        //Rigid Move position
        rigid.MovePosition(rigid.position + newVector);
    }

}
