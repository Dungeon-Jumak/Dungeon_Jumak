using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleShadowController : MonoBehaviour
{
    public bool isStop;
    public float fadeInDuration; //그림자가 나타나는 시간

    [SerializeField]
    private SpriteRenderer shadowRenderer; //그림자의 SpriteRenderer 컴포넌트

    [SerializeField]
    private OrderMenu orderMenu;

    private float startY; //시작 Y 위치
    private float endY; // 종료 Y 위치

    private float timer;

    private void Start()
    {
        //--- 변수 초기화 ---//
        isStop = false;

        fadeInDuration = 20f;

        shadowRenderer = GetComponent<SpriteRenderer>();

        startY = -0.8f;
        endY = 0f;

        timer = 0f;

        //--- 기본 위치 초기화 ---//
        transform.localPosition
            = new Vector3(transform.localPosition.x, startY, transform.localPosition.z);

        //--- 그림자로 기존 말풍선 위를 덮기 위한 SpriteMask 컴포넌트 추가 ---//
        SpriteMask spriteMask = GetComponentInParent<SpriteMask>();

        if(spriteMask != null)
            shadowRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
    }

    void Update()
    {
        //--- 타이머 ---//
        if(!isStop) timer += Time.deltaTime;

        //--- 현재 시간에 맞게 그림자의 위치를 재조정 ---//
        float newY = Mathf.Lerp(startY, endY, timer / fadeInDuration);
        transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);

        //--- 타이머의 시간이 다 되었아면 타임 아웃 함수를 호출하고 본 스크립트를 초기화 함---//
        if (timer >= fadeInDuration)
        {
            orderMenu.TimeOut();
            Initialize();
        }
    }

    //---말풍선 그림자 초기화 함수---//
    public void Initialize()
    {
        isStop = false;
        timer = 0;
        transform.localPosition = new Vector3(transform.localPosition.x, startY, transform.localPosition.z);    //그림자 시작 위치 초기화
    }


}
