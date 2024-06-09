// System
using System.Collections;
using System.Collections.Generic;

// Unity
using UnityEngine;

[DisallowMultipleComponent]
public class BubbleShadowController : MonoBehaviour
{
    //Shadow Duration
    [Header("대기 시간 : 그림자가 차오르는데 걸리는 시간")]
    private float fadeInDuration;

    //Shado Sprite Renderer
    [Header("그림자의 스프라이트 렌더러 컴포넌트")]
    [SerializeField] private SpriteRenderer shadowRenderer;

    //Order Menu
    [Header("OrderMenu")]
    [SerializeField] private OrderMenu orderMenu;

    //Jumak Scene
    private JumakScene jumakScene;

    //Start Y Position
    private float startY;

    //End Y Position
    private float endY;

    //Timer
    private float timer;

    private void Start()
    {
        //Initialize duration
        fadeInDuration = 12f;

        //Get Component : Sprite Renderer
        shadowRenderer = GetComponent<SpriteRenderer>();

        //Get Component : JumakScene
        jumakScene = GameObject.Find("@Scene").transform.GetComponent<JumakScene>();

        //Initialize Y Position
        startY = -0.8f;
        endY = 0f;

        //Initialize Timer
        timer = 0f;

        //Initialize position
        transform.localPosition
            = new Vector3(transform.localPosition.x, startY, transform.localPosition.z);

        //Setting Sprite Mask
        SpriteMask spriteMask = GetComponentInParent<SpriteMask>();

        if(spriteMask != null)
            shadowRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
    }

    void Update()
    {
        //Timer System
        TimerSystem();
    }

    //Timer System
    private void TimerSystem()
    {
        //Increase Timer
        if (!jumakScene.pause) timer += Time.deltaTime;

        //Update Position
        float newY = Mathf.Lerp(startY, endY, timer / fadeInDuration);
        transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);

        //If Time Out
        if (timer >= fadeInDuration)
        {
            //Call OrderMenu's TimeOut() Method
            orderMenu.TimeOut();

            //Initialize
            Initialize();
        }
    }

    //Initialize timer and position
    public void Initialize()
    {
        //Init Timer
        timer = 0;

        //Init Position
        transform.localPosition = new Vector3(transform.localPosition.x, startY, transform.localPosition.z);
    }


}
