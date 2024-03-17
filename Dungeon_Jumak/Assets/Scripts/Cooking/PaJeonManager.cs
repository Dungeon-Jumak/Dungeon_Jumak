using UnityEngine;
using System.Collections;

public class PaJeonManager : MonoBehaviour
{
    public GameObject[] directionPrefabs;

    private Vector3 dragStartPosition;
    private int[] correctSequence;
    private int currentIndex = 0;
    private bool hasFailed = true;
    private GameObject[] spawnedPrefabs; // 이전에 생성된 프리팹들을 추적하기 위한 배열
    public GameObject PaJeonPopUp;

    void OnEnable()
    {
        StartCoroutine(StartGameAfterDelay(0.1f));
    }

    IEnumerator StartGameAfterDelay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        RestartGame();
    }

    public void RestartGame()
    {
        currentIndex = 0;
        dragStartPosition = Vector3.zero;

        // 이전에 생성된 프리팹들 제거
        if (spawnedPrefabs != null)
        {
            foreach (GameObject prefab in spawnedPrefabs)
            {
                Destroy(prefab);
            }
        }

        RandomGen();

        hasFailed = false;
    }

    public void RandomGen()
    {
        correctSequence = new int[4];
        spawnedPrefabs = new GameObject[4]; // 생성된 프리팹들을 추적하기 위한 배열 초기화

        for (int i = 0; i < correctSequence.Length; i++)
        {
            correctSequence[i] = Random.Range(0, directionPrefabs.Length);
        }

        float xOffset = -1.6f;
        for (int i = 0; i < correctSequence.Length; i++)
        {
            GameObject directionPrefab = Instantiate(directionPrefabs[correctSequence[i]], transform);
            directionPrefab.transform.position = new Vector3(xOffset, 3.86f, 0f);
            xOffset += directionPrefab.GetComponent<SpriteRenderer>().bounds.size.x;

            // 생성된 프리팹들을 추적하기 위해 배열에 추가
            spawnedPrefabs[i] = directionPrefab;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (!hasFailed)
            {
                if (Input.touchCount > 0)
                {
                    dragStartPosition = Input.GetTouch(0).position;
                }
                else
                {
                    dragStartPosition = Input.mousePosition;
                }
            }
        }
        else if (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            if (!hasFailed)
            {
                Vector3 dragEndPosition;

                if (Input.touchCount > 0)
                {
                    dragEndPosition = Input.GetTouch(0).position;
                }
                else
                {
                    dragEndPosition = Input.mousePosition;
                }

                Vector3 dragDirection = dragEndPosition - dragStartPosition;
                float dragThreshold = 20f;

                if (dragDirection.magnitude > dragThreshold)
                {
                    if (Mathf.Abs(dragDirection.x) > Mathf.Abs(dragDirection.y))
                    {
                        if (dragDirection.x > 0)
                        {
                            CheckDirection(3);
                        }
                        else
                        {
                            CheckDirection(2);
                        }
                    }
                    else
                    {
                        if (dragDirection.y > 0)
                        {
                            CheckDirection(0);
                        }
                        else
                        {
                            CheckDirection(1);
                        }
                    }
                }
            }
        }
    }

    void CheckDirection(int directionIndex)
    {
        if (correctSequence[currentIndex] == directionIndex)
        {
            currentIndex++;

            if (currentIndex >= correctSequence.Length)
            {
                Debug.Log("성공입니다!");
                hasFailed = true;
                PaJeonPopUp.gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.Log("실패입니다.");
            hasFailed = true; 
            PaJeonPopUp.gameObject.SetActive(false);
        }
    }
}
