using UnityEngine;
using System.Collections;

public class CollisionDetection : MonoBehaviour
{
    public GameObject kettleObject;
    public GameObject checkKettleObject;

    private int MatchKettle = 0;
    private int FailMatchKettle = 0;

    private PolygonCollider2D kettleCollider;
    private PolygonCollider2D checkKettleCollider;

    private bool inputEnabled = true;

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
            MatchKettle++;

        }
        else
        {
            Debug.Log("����");
            FailMatchKettle++;
        }

        yield return new WaitForSeconds(1f);

        inputEnabled = true;
    }

    void SuccessRiceJuiceMiniGame()
    {
        GameObject[] parentObjects = GameObject.FindGameObjectsWithTag("RiceJuiceMiniGame");

        foreach (GameObject parentObject in parentObjects)
        {
            foreach (Transform child in parentObject.transform)
            {
                child.gameObject.SetActive(false);
            }
        }

        //===���̺� ���̴� ��� �߰���===//
    }

    void FailRiceJuiceMiniGame()
    {
        GameObject[] parentObjects = GameObject.FindGameObjectsWithTag("RiceJuiceMiniGame");

        foreach (GameObject parentObject in parentObjects)
        {
            foreach (Transform child in parentObject.transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }
}
