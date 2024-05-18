using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CollisionDetection : MonoBehaviour
{
    public GameObject kettleObject;
    public GameObject checkKettleObject;

    public GameObject blackPanel;

    private int MatchKettle = 0;
    private int FailMatchKettle = 0;

    private PolygonCollider2D kettleCollider;
    private PolygonCollider2D checkKettleCollider;

    private bool inputEnabled = true;

    private AudioManager audioManager;
    private string successSound;
    private string failureSound;

    [SerializeField]
    private Animator successAni;
    [SerializeField]
    private Animator failureAni;

    [SerializeField]
    private GameObject juiceLiquid;
    [SerializeField]
    private Sprite juiceInCup;

    [SerializeField]
    private SpriteRenderer cup;
    [SerializeField]
    private JumakScene jumakScene;

    void OnEnable()
    {
        MatchKettle = 0;
        FailMatchKettle = 0;
        inputEnabled = true;
        StopAllCoroutines();
    }

    void Start()
    {
        kettleCollider = kettleObject.GetComponent<PolygonCollider2D>();
        checkKettleCollider = checkKettleObject.GetComponent<PolygonCollider2D>();

        audioManager = FindObjectOfType<AudioManager>();

        successSound = "successSound";
        failureSound = "failureSound";

        if (kettleCollider == null)
        {
            Debug.LogError("Kettle 오브젝트에 Polygon Collider가 없습니다!");
        }

        if (checkKettleCollider == null)
        {
            Debug.LogError("CheckKettle 오브젝트에 Polygon Collider가 없습니다!");
        }
    }

    void Update()
    {
        if (inputEnabled && (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)))
        {
            StartCoroutine(ProcessInput());
        }

        //===성공 할 경우===//
        if (MatchKettle == 3)
        {
            SuccessRiceJuiceMiniGame();
            jumakScene.isPause = false;
        }

        //===실패 할 경우===//
        if (FailMatchKettle == 2)
        {
            FailRiceJuiceMiniGame();
            jumakScene.isPause = false;
        }
    }

    IEnumerator PourLiquid()
    {
        juiceLiquid.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        juiceLiquid.SetActive(false);
    }

    IEnumerator ProcessInput()
    {
        inputEnabled = false;

        //===충돌 판별===//
        if (kettleCollider.IsTouching(checkKettleCollider))
        {
            Debug.Log("성공");
            successAni.SetTrigger("notice");
            audioManager.Play(successSound);
            StartCoroutine(PourLiquid());
            cup.sprite = juiceInCup;
            MatchKettle++;

        }
        else
        {
            Debug.Log("실패");
            failureAni.SetTrigger("notice");
            audioManager.Play(failureSound);
            FailMatchKettle++;
        }

        yield return new WaitForSeconds(1f);

        inputEnabled = true;
    }

    void SuccessRiceJuiceMiniGame()
    {
        DataManager.Instance.data.riceJuiceClear = true;

        GameObject[] parentObjects = GameObject.FindGameObjectsWithTag("RiceJuiceMiniGame");

        foreach (GameObject parentObject in parentObjects)
        {
            foreach (Transform child in parentObject.transform)
            {
                child.gameObject.SetActive(false);
                blackPanel.SetActive(false);
            }
        }

        Invoke("DelayNextMiniGame", 0.5f);
    }

    void DelayNextMiniGame()
    {
        DataManager.Instance.data.isMiniGame = false;
    }

    void FailRiceJuiceMiniGame()
    {
        GameObject[] speechBox = GameObject.FindGameObjectsWithTag("SpeechBox");
        GameObject[] shadows = new GameObject[DataManager.Instance.data.onTables.Length];

        for (int i = 0; i < speechBox.Length; i++)
        {
            shadows[i] = speechBox[i].transform.GetChild(1).gameObject;

            if (shadows[i].GetComponent<BubbleShadowController>().isMiniGame)
            {
                shadows[i].GetComponent<BubbleShadowController>().isMiniGame = false;
                shadows[i].GetComponent<BubbleShadowController>().timer =
                    shadows[i].GetComponent<BubbleShadowController>().fadeInDuration;
            }

        }

        GameObject[] parentObjects = GameObject.FindGameObjectsWithTag("RiceJuiceMiniGame");

        foreach (GameObject parentObject in parentObjects)
        {
            foreach (Transform child in parentObject.transform)
            {
                child.gameObject.SetActive(false);
                blackPanel.SetActive(false);
            }
        }

        Invoke("DelayNextMiniGame", 0.5f);
    }
}
