using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiceJuiceOrder : MonoBehaviour
{
    public int blackPanelIdx; //블랙 패널 인덱스
    public GameObject riceJuicePrefab;

    CircleCollider2D circleCollider;

    bool playerInsideCollider = false;

    private PlayerServing player;
    [SerializeField]
    private int idx; //부딪힌 테이블의 인덱스

    Data data;

    void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        player = FindObjectOfType<PlayerServing>();

        data = DataManager.Instance.data;
    }

    void Update()
    {
        if (data.riceJuiceClear)
        {
            data.riceJuiceClear = false;
            if (data.riceJuiceClear) return;

            GameObject instance = Instantiate(riceJuicePrefab, player.tables[data.tableIndex].transform.GetChild(0));
            instance.transform.localPosition = Vector3.zero;

            data.onTables[data.tableIndex] = true;
        }



        if (playerInsideCollider)
        {
            //===마우스 입력 확인===//
            if (Input.GetMouseButtonDown(0) && !player.isCarryingFood)
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = transform.position.z;
                if (circleCollider.OverlapPoint(mousePosition))
                {
                    Debug.Log("감지");
                    GameObject[] parentObjects = GameObject.FindGameObjectsWithTag("RiceJuiceMiniGame");

                    GameObject[] speechBoxes = GameObject.FindGameObjectsWithTag("SpeechBox");
                    GameObject[] shadows = new GameObject[data.onTables.Length];

                    for (int i = 0; i <speechBoxes.Length; i++)
                    {
                        //그림자들을 불러옴
                        shadows[i] =speechBoxes[i].transform.GetChild(1).gameObject;
                        shadows[i].GetComponent<BubbleShadowController>().isStop = true;
                    }

                    foreach (GameObject parentObject in parentObjects)
                    {
                        foreach (Transform child in parentObject.transform)
                        {
                            child.gameObject.SetActive(true);
                            GameObject.Find("Home_Panel").transform.GetChild(blackPanelIdx).gameObject.SetActive(true);
                        }
                    }



                }
            }

            //===터치 입력 확인===//
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                touchPosition.z = transform.position.z;
                if (circleCollider.OverlapPoint(touchPosition))
                {
                    GameObject[] parentObjects = GameObject.FindGameObjectsWithTag("RiceJuiceMiniGame");

                    foreach (GameObject parentObject in parentObjects)
                    {
                        foreach (Transform child in parentObject.transform)
                        {
                            child.gameObject.SetActive(true);
                        }
                    }

                }
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideCollider = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInsideCollider = false;
        }
    }
}
