using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private string jumakSceneName;
    [SerializeField]
    private Transform jumakSign;
    [SerializeField]
    private string dungeonSceneName;
    [SerializeField]
    private Transform dungeonSign;
    [SerializeField]
    private Transform targetTransform;

    [SerializeField]
    private string sceneName;

    [SerializeField]
    private AudioManager audioManager;
    [SerializeField]
    private string hillWalkSound;

    // Start is called before the first frame update
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
            curPosition = transform.position;

            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(curPosition, targetTransform.position, step);

            if (Vector3.Distance(targetTransform.position, curPosition) == 0f)
            {
                isMove = false;
                ConvertScene();
            }
        }
    }

    public void ConvertScene()
    {
        GameManager.Instance.ConvertScene(sceneName);
    }

    //주막 방향으로 이동
    public void MoveJumak()
    {
        isMove = true;
        animator.SetInteger("DirX", -1);
        targetTransform = jumakSign.transform;
        sceneName = jumakSceneName;
    }

    //던전 방향으로 이동
    public void MoveDungeon()
    {
        isMove = true;
        animator.SetInteger("DirX", 1);
        DataManager.Instance.data.playerHP = 3;
        DataManager.Instance.data.runningTime = 0f;
        targetTransform = dungeonSign.transform;
        sceneName = dungeonSceneName;
    }
}
