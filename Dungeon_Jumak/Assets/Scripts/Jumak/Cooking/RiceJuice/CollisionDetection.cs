//System
using System.Collections;

//Unity
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class CollisionDetection : MonoBehaviour
{
    #region Variables

    //Black Panel
    [Header("�� �г�")]
    [SerializeField] private GameObject blackPanel;

    //Rice Juice Game Manager
    [Header("���� �̴ϰ��� �ֻ��� ������Ʈ")]
    [SerializeField] private GameObject miniGameParent;

    //Collider for Kettle inlet
    [Header("������ �ݶ��̴�")]
    [SerializeField] private PolygonCollider2D kettleCollider;

    //Collider for Checking
    [Header("üŷ ���� �ݶ��̴�")]
    [SerializeField] private PolygonCollider2D checkAreaCollider;

    //Success Animation
    [Header("���� �ִϸ��̼�")]
    [SerializeField] private Animator successAni;

    //Failure Animation
    [Header("���� �ִϸ��̼�")]
    [SerializeField] private Animator failureAni;

    //Juice Liquid Object
    [Header("�����ڿ��� �������� ���� ������Ʈ")]
    [SerializeField] private GameObject juiceLiquid;

    //Sprite Renderer of Cup Object
    [Header("���� �̹����� �ٲٱ� ���� ��������Ʈ ������")]
    [SerializeField] private SpriteRenderer cup;

    //Sprite for Juice in Cup
    [Header("�žȿ� ������ �ִ� ��������Ʈ")]
    [SerializeField] private Sprite juiceInCup;

    //Sprite for empty Cup
    [Header("��� �ִ� �� ��������Ʈ")]
    [SerializeField] private Sprite emptyCup;

    //Jumak Scene
    [Header("�ָ���")]
    [SerializeField] private JumakScene jumakScene;

    //Input Enabled Sign
    private bool canTouch = true;

    //Success Count
    private int successCount = 0;

    //Fail Count
    private int failCount = 0;

    #endregion

    //When On Enable This Object : Initialize
    void OnEnable()
    {
        //Initialize Variables
        successCount = 0;
        failCount = 0;

        //Empty Cup
        cup.sprite = emptyCup;

        //GameStart Sign
        canTouch = true;

        //Stop All Coroutines -> Avoid Duplication
        StopAllCoroutines();
    }

    void Update()
    {
        //MiniGame System
        MiniGameSystem();
    }

    #region Methods

    //MiniGame System
    private void MiniGameSystem()
    {
        //When can touch and touch down
        if (canTouch && (Input.GetMouseButtonDown(0)))
        {
            //Check Success
            StartCoroutine(CheckingCollider());
        }

        //If Success Count is three
        if (successCount == 3)
        {
            successCount = 0;
            //Success Coroutine
            StartCoroutine(Success());
        }


        //If Fail Count is one
        if (failCount == 1)
        {
            failCount = 0;
            //Failure Coroutine
            StartCoroutine(Failure());
        }

    }

    //Pour Liquid Coroutine
    IEnumerator PourLiquid()
    {
        //Active Liquid Object
        juiceLiquid.SetActive(true);

        //Delay 0.5f seconds
        yield return new WaitForSeconds(0.5f);

        //Inactive Liquid Object
        juiceLiquid.SetActive(false);
    }

    //Coroutine For Check Collider
    IEnumerator CheckingCollider()
    {
        //Can't touch
        canTouch = false;

        //Check Collision : Correct
        if (kettleCollider.IsTouching(checkAreaCollider))
        {
            GameManager.Sound.Play("[S] MiniGame Success", Define.Sound.Effect, false);

            //Success Animation
            successAni.SetTrigger("notice");

            //Pour Liquid Coroutine
            StartCoroutine(PourLiquid());

            //Change Cup Sprite : -> juice in cup
            cup.sprite = juiceInCup;

            //Increase Success Count
            successCount++;
        }
        //Check Collision : InCorrect
        else
        {
            GameManager.Sound.Play("[S] MiniGame Failure", Define.Sound.Effect, false);

            //Failure Animation
            failureAni.SetTrigger("notice");

            //Increase Failure Count
            failCount++;
        }

        //Delay 1f seconds
        yield return new WaitForSeconds(1f);

        //Can touch
        canTouch = true;
    }

    //Success Coroutine
    IEnumerator Success()
    {
        //Delay 1f seconds
        yield return new WaitForSeconds(1f);

        //Conver Sign related minigame successing
        DataManager.Instance.data.successRiceJuiceMiniGame = true;

        //Inactive Black Panel
        blackPanel.SetActive(false);

        //Inactive MiniGame Manager
        miniGameParent.SetActive(false);

        //avoid duplication
        Invoke("DelayNextMiniGame", 0.5f);
    }

    //Failure Coroutine
    IEnumerator Failure()
    {
        //Delay 1f seconds
        yield return new WaitForSeconds(1f);

        //Inactive Balck Panel
        blackPanel.SetActive(false);

        //Inactive MiniGame Manager
        miniGameParent.SetActive(false);

        //avoid duplication
        Invoke("DelayNextMiniGame", 0.5f);
    }

    //Delay Next Mini Game for avoid duplication
    void DelayNextMiniGame()
    {
        DataManager.Instance.data.isMiniGame = false;
    }

    #endregion
}
