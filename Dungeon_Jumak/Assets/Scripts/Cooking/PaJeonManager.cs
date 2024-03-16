using UnityEngine;

public class PaJeonManager : MonoBehaviour
{
    public GameObject[] directionPrefabs;

    private Vector3 dragStartPosition;
    private int[] correctSequence;
    private int currentIndex = 0;
    private bool hasFailed = false;

    void Start()
    {
        correctSequence = new int[4];
        for (int i = 0; i < correctSequence.Length; i++)
        {
            correctSequence[i] = Random.Range(0, directionPrefabs.Length);
        }

        float xOffset = -1.6f;
        for (int i = 0; i < correctSequence.Length; i++)
        {
            GameObject directionPrefab = Instantiate(directionPrefabs[correctSequence[i]], transform); // 스크립트를 가지고 있는 오브젝트의 하위에 생성
            directionPrefab.transform.position = new Vector3(xOffset, 3.86f, 0f);
            xOffset += directionPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
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
            }
        }
        else
        {
            Debug.Log("실패입니다.");
            hasFailed = true;
        }
    }
}
