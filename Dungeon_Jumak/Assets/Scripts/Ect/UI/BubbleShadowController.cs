using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleShadowController : MonoBehaviour
{
    public bool isStop;
    
    public float fadeInDuration = 10f; //�׸��ڰ� ��Ÿ���� �ð�
    public float startY = -0.8f; //���� Y ��ġ
    public float endY = 0f; // ���� Y ��ġ

    public float timer = 0f;

    public bool isMiniGame;

    [SerializeField]
    private SpriteRenderer shadowRenderer; //�׸����� SpriteRenderer ������Ʈ
   

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
        isMiniGame = false;

        fadeInDuration = 13f;

        shadowRenderer = GetComponent<SpriteRenderer>();
        
        //---���� ����---//
        audioManager = FindObjectOfType<AudioManager>();
        timeOutSound = "timeOut";

        //--- �⺻ ��ġ �ʱ�ȭ ---//
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
        if(!isStop) //�ð� ���� ��� �߰� �ϰ� ���� �� ���
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
        isStop = false;
        timer = 0;
        //�׸��� ���� ��ġ �ʱ�ȭ
        transform.localPosition = new Vector3(transform.localPosition.x, startY, transform.localPosition.z);
    }


}