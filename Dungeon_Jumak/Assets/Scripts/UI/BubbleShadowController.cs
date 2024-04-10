using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleShadowController : MonoBehaviour
{
    public float fadeInDuration = 10f; //그림자가 나타나는 시간
    public float startY = -10f; //시작 Y 위치
    public float endY = 0f; // 종료 Y 위치

    [SerializeField]
    private SpriteRenderer shadowRenderer; //그림자의 SpriteRenderer 컴포넌트
    [SerializeField]
    private float timer = 0f;

    [SerializeField]
    private OrderMenu orderMenu;

    [SerializeField]
    private Data data;

    [SerializeField]
    private AudioManager audioManager;
    [SerializeField]
    private string timeOutSound;


    private void Start()
    {
        shadowRenderer = GetComponent<SpriteRenderer>();
        
        //---사운드 관련---//
        audioManager = FindObjectOfType<AudioManager>();
        timeOutSound = "timeOut";

        //--- 기본 위치 초기화 ---//
        transform.localPosition
            = new Vector3(transform.localPosition.x, startY, transform.localPosition.z);

        SpriteMask spriteMask = GetComponentInParent<SpriteMask>();
        if(spriteMask != null)
        {
            shadowRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        float newY = Mathf.Lerp(startY, endY, timer / fadeInDuration);
        transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);

        if (timer >= fadeInDuration)
        {
            audioManager.Play(timeOutSound);
            orderMenu.TimeOut();
            Initialize();
        }
    }

    public void Initialize()
    {
        timer = 0;
        //그림자 시작 위치 초기화
        transform.localPosition = new Vector3(transform.localPosition.x, startY, transform.localPosition.z);
    }


}
