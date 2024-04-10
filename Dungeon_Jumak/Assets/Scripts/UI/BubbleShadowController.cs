using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleShadowController : MonoBehaviour
{
    public float fadeInDuration = 10f; //�׸��ڰ� ��Ÿ���� �ð�
    public float startY = -10f; //���� Y ��ġ
    public float endY = 0f; // ���� Y ��ġ

    [SerializeField]
    private SpriteRenderer shadowRenderer; //�׸����� SpriteRenderer ������Ʈ
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
        //�׸��� ���� ��ġ �ʱ�ȭ
        transform.localPosition = new Vector3(transform.localPosition.x, startY, transform.localPosition.z);
    }


}
