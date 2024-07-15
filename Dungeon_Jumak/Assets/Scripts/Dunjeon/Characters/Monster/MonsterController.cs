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
    public float health;

    [Header("몬스터의 최대 체력")]
    public float maxHealth;

    [Header("몬스터의 공격력")]
    public float attackPower;

    [Header("몬스터의 드랍아이템 프리팹 ID")]
    [SerializeField] private int dropItemID;

    [Header("드랍아이템 풀 매니저")]
    [SerializeField] private DropItemPoolManager dropPool;

    [Header("드랍 확률")]
    [SerializeField] private int percent;

    [Header("몬스터의 경험치량")]
    [SerializeField] private float xp;

    private AIDestinationSetter destination;
    private AIPath aiPath;

    //Rigidbody 2D
    private Rigidbody2D rigid;

    //Sprite Renderer
    private SpriteRenderer spriteRenderer;

    //Animator
    private Animator animator;

    //Data
    private Data data;

    //isLive Sign
    public bool isLive;

    //isMove Sign
    private bool isMove;

    //moveVector
    private Vector3 moveVector;

    //For Fire Flooring
    private bool isDamaging = false;

    private void OnEnable()
    {
        //Initialize
        target = FindObjectOfType<PlayerMovement_Dun>().GetComponent<Rigidbody2D>();

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
        animator = GetComponent<Animator>();
        dropPool = FindObjectOfType<DropItemPoolManager>();
        data = DataManager.Instance.data;

        //Initialize
        isMove = false;
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

        //Up than player
        if(target.position.y < rigid.position.y)
            spriteRenderer.sortingOrder = 0;
        else
            spriteRenderer.sortingOrder = 2;

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

    private void Move()
    {
        //Set New Vector
        Vector2 newVector = moveVector * patrolSpeed * Time.deltaTime;

        //Rigid Move position
        rigid.MovePosition(rigid.position + newVector);
    }

    #region For FireBall

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Attack_Ball"))
            return;

        //Damage !
        health -= collision.GetComponent<Skills>().damage;

        //KnockBack
        StartCoroutine(KnockBack(collision.GetComponent<Skills>().knockBack));

        CheckHealth();
    }

    #endregion

    #region For FireShield

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Attack_Shield"))
            return;
        Debug.Log("nucback");


        //Damage !
        health -= collision.gameObject.GetComponent<Skills>().damage;

        //KnockBack
        StartCoroutine(KnockBack(collision.gameObject.GetComponent<Skills>().knockBack));

        CheckHealth();
    }

    #endregion

    #region For FireFlooring

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Attack_Floor"))
            return;

        if (!isDamaging)
        {
            StartCoroutine(RuduceHealthAfterDelay(collision));
        }
    }

    private IEnumerator RuduceHealthAfterDelay(Collider2D collision)
    {
        isDamaging = true;

        yield return new WaitForSeconds(1f);

        // Damage
        health -= collision.GetComponent<Skills>().damage;

        isDamaging = false;

        CheckHealth();
    }

    #endregion

    private void CheckHealth()
    {
        //Live
        if (health > 0)
        {
            animator.SetTrigger("Hit");
        }
        //Dead
        else
        {
            isLive = false;

            int random = Random.Range(0, 100);

            if (random < percent)
            {
                //Item drop
                Drop();
            }

            //Increase XP
            data.curXP += xp;

            Dead();
        }
    }

    IEnumerator KnockBack(float _knockBack)
    {
        Debug.Log("nucback");

        //Delay One Frame
        yield return new WaitForSeconds(0.35f);

        //Get Player Position
        Vector3 playerPos = target.transform.position;

        //Compute Direction Vector
        Vector3 dirVec = transform.position - playerPos;
        
        //For Use Rigidbody
        destination.enabled = false;
        aiPath.enabled = false;

        //Add Force : Knock Back
        rigid.AddForce(dirVec.normalized * _knockBack, ForceMode2D.Impulse);

        yield return new WaitForFixedUpdate();

        destination.enabled = true;
        aiPath.enabled = true;
    }

    private void Drop()
    {
        //Get Pool
        GameObject dropItem = dropPool.Get(dropItemID);

        //Reposition
        dropItem.transform.position = transform.position;
    }

    private void Dead()
    {
        gameObject.SetActive(false);
    }

    #region Damage Effect

    private void DamageEffect()
    {
        StartCoroutine(Effect());
    }

    private IEnumerator Effect()
    {
        // Set transparency to 50%
        SetTransparency(0.5f);

        yield return new WaitForSeconds(0.3f);

        // Set transparency back to 100%
        SetTransparency(1.0f);
    }

    private void SetTransparency(float alpha)
    {
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }

    #endregion
}
