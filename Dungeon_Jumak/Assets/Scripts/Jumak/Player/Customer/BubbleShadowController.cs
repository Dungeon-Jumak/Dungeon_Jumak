using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleShadowController : MonoBehaviour
{
    public bool isStop;
    public float fadeInDuration; //�׸��ڰ� ��Ÿ���� �ð�

    [SerializeField]
    private SpriteRenderer shadowRenderer; //�׸����� SpriteRenderer ������Ʈ

    [SerializeField]
    private OrderMenu orderMenu;

    private float startY; //���� Y ��ġ
    private float endY; // ���� Y ��ġ

    private float timer;

    private void Start()
    {
        //--- ���� �ʱ�ȭ ---//
        isStop = false;

        fadeInDuration = 20f;

        shadowRenderer = GetComponent<SpriteRenderer>();

        startY = -0.8f;
        endY = 0f;

        timer = 0f;

        //--- �⺻ ��ġ �ʱ�ȭ ---//
        transform.localPosition
            = new Vector3(transform.localPosition.x, startY, transform.localPosition.z);

        //--- �׸��ڷ� ���� ��ǳ�� ���� ���� ���� SpriteMask ������Ʈ �߰� ---//
        SpriteMask spriteMask = GetComponentInParent<SpriteMask>();

        if(spriteMask != null)
            shadowRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
    }

    void Update()
    {
        //--- Ÿ�̸� ---//
        if(!isStop) timer += Time.deltaTime;

        //--- ���� �ð��� �°� �׸����� ��ġ�� ������ ---//
        float newY = Mathf.Lerp(startY, endY, timer / fadeInDuration);
        transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);

        //--- Ÿ�̸��� �ð��� �� �Ǿ��Ƹ� Ÿ�� �ƿ� �Լ��� ȣ���ϰ� �� ��ũ��Ʈ�� �ʱ�ȭ ��---//
        if (timer >= fadeInDuration)
        {
            orderMenu.TimeOut();
            Initialize();
        }
    }

    //---��ǳ�� �׸��� �ʱ�ȭ �Լ�---//
    public void Initialize()
    {
        isStop = false;
        timer = 0;
        transform.localPosition = new Vector3(transform.localPosition.x, startY, transform.localPosition.z);    //�׸��� ���� ��ġ �ʱ�ȭ
    }


}
