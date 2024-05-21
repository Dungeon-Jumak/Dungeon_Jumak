using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class PlayerInWatingScene : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    //---이동 관련 변수---//
    [SerializeField]
    private Vector3 curPosition;
    [SerializeField]
    private bool isMove = false;
    [SerializeField]
    private float speed;

    [SerializeField]
    private Transform jumakSign;
    [SerializeField]
    private Transform dungeonSign;
    [SerializeField]
    private Transform targetTransform;

    [SerializeField]
    private AudioManager audioManager;
    [SerializeField]
    private string hillWalkSound;

    void Start()
    {
        curPosition = new Vector3(0f, -3.8f);
        isMove = false;
        speed = 3f;

        animator = GetComponent<Animator>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMove)
        {
            MoveToTarget();
        }
    }

    private void MoveToTarget()
    {
        curPosition = transform.position;

        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(curPosition, targetTransform.position, step);
    }

    //주막 방향으로 이동
    public void MoveJumak()
    {
        isMove = true;

        if (DataManager.Instance.data.isSound)
            audioManager.Play(hillWalkSound);

        animator.SetInteger("DirX", -1);
        targetTransform = jumakSign.transform;
    }

    //던전 방향으로 이동
    public void MoveDungeon()
    {
        isMove = true;

        if (DataManager.Instance.data.isSound)
            audioManager.Play(hillWalkSound);

        animator.SetInteger("DirX", 1);
        DataManager.Instance.data.playerHP = 3;
        DataManager.Instance.data.runningTime = 0f;
        targetTransform = dungeonSign.transform;
    }

    //상점 방향으로 이동
    public void MoveMarket()
    {
        //isMove = true;
        if (DataManager.Instance.data.isSound)
            audioManager.Play(hillWalkSound);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Target_Jumak")) GameManager.Scene.LoadScene(Define.Scene.Jumak);
        else if(col.CompareTag("Target_Dunjeon")) GameManager.Scene.LoadScene(Define.Scene.Map);
        //else if(col.CompareTag("Target_Market")) GameManager.Scene.LoadScene(Define.Scene.Market);
    }
}