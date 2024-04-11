using UnityEngine;
using System.Collections;

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
            Debug.LogError("Kettle ������Ʈ�� Polygon Collider�� �����ϴ�!");
        }

        if (checkKettleCollider == null)
        {
            Debug.LogError("CheckKettle ������Ʈ�� Polygon Collider�� �����ϴ�!");
        }
    }

    void Update()
    {
        if (inputEnabled && (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)))
        {
            StartCoroutine(ProcessInput());
        }

        //===���� �� ���===//
        if (MatchKettle == 3)
        {
            SuccessRiceJuiceMiniGame();
        }

        //===���� �� ���===//
        if (FailMatchKettle == 2)
        {
            FailRiceJuiceMiniGame();
        }
    }

    IEnumerator ProcessInput()
    {
        inputEnabled = false;

        //===�浹 �Ǻ�===//
        if (kettleCollider.IsTouching(checkKettleCollider))
        {
            Debug.Log("����");
            successAni.SetTrigger("notice");
            audioManager.Play(successSound);
            MatchKettle++;

        }
        else
        {
            Debug.Log("����");
            failureAni.SetTrigger("notice");
            audioManager.Play(failureSound);
            FailMatchKettle++;
        }

        yield return new WaitForSeconds(1f);

        inputEnabled = true;
    }

    void SuccessRiceJuiceMiniGame()
    {
        GameObject[] speechBoxes = GameObject.FindGameObjectsWithTag("SpeechBox");
        GameObject[] shadows = new GameObject[DataManager.Instance.data.onTables.Length];

        for (int i = 0; i < speechBoxes.Length; i++)
        {
            //�׸��ڵ��� �ҷ���
            shadows[i] = speechBoxes[i].transform.GetChild(1).gameObject;
            shadows[i].GetComponent<BubbleShadowController>().isStop = false;
        }

        GameObject[] parentObjects = GameObject.FindGameObjectsWithTag("RiceJuiceMiniGame");

        foreach (GameObject parentObject in parentObjects)
        {
            foreach (Transform child in parentObject.transform)
            {
                DataManager.Instance.data.riceJuiceClear = true;
                child.gameObject.SetActive(false);
                blackPanel.SetActive(false);
            }
        }
    }

    void FailRiceJuiceMiniGame()
    {
        GameObject[] parentObjects = GameObject.FindGameObjectsWithTag("RiceJuiceMiniGame");

        foreach (GameObject parentObject in parentObjects)
        {
            foreach (Transform child in parentObject.transform)
            {
                child.gameObject.SetActive(false);
                blackPanel.SetActive(false);
            }
        }
    }
}
