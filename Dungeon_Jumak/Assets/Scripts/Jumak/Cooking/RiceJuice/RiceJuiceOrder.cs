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

    private JumakScene jumakScene;

    void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        player = FindObjectOfType<PlayerServing>();

        data = DataManager.Instance.data;

        jumakScene = FindObjectOfType<JumakScene>();
    }

    void Update()
    {
        if (data.riceJuiceClear)
        {
            data.riceJuiceClear = false;
            if (data.riceJuiceClear) return;

            GameObject go = transform.parent.gameObject;

            for (int i = 0; i <data.onTables.Length; i++)
            {
                if (data.tableMiniGame[i])
                {
                    data.tableMiniGame[i] = false;

                    GameObject instance = Instantiate(riceJuicePrefab, player.tables[i].transform.GetChild(0));
                    instance.transform.localPosition = Vector3.zero;
                    data.onTables[i] = true;
                }

            }

        }



        if (playerInsideCollider)
        {
            //===마우스 입력 확인===//
            if (Input.GetMouseButtonDown(0) && !player.isCarryingFood)
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = transform.position.z;

                if (circleCollider.OverlapPoint(mousePosition) && !data.isMiniGame)
                {
                    Debug.Log("감지");

                    data.isMiniGame = true;
                    jumakScene.isPause = true;

                    GameObject[] parentObjects = GameObject.FindGameObjectsWithTag("RiceJuiceMiniGame");

                    GameObject go = transform.parent.gameObject;

                    for (int i = 0; i < data.onTables.Length; i++)
                    {
                        if (i == go.GetComponent<CustomerMovement>().seatIndex)
                            data.tableMiniGame[i] = true;
                    }



                    GameObject shadow = transform.GetChild(1).gameObject;
                    shadow.GetComponent<BubbleShadowController>().isStop = true;

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
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = transform.position.z;

                if (circleCollider.OverlapPoint(mousePosition) && !data.isMiniGame)
                {
                    Debug.Log("감지");

                    data.isMiniGame = true;
                    jumakScene.isPause = true;

                    GameObject[] parentObjects = GameObject.FindGameObjectsWithTag("RiceJuiceMiniGame");

                    GameObject go = transform.parent.gameObject;

                    for (int i = 0; i < data.onTables.Length; i++)
                    {
                        if (i == go.GetComponent<CustomerMovement>().seatIndex)
                            data.tableMiniGame[i] = true;
                    }



                    GameObject shadow = transform.GetChild(1).gameObject;
                    shadow.GetComponent<BubbleShadowController>().isStop = true;

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
