using UnityEngine;
using System.Collections;

public class PaJeonManager : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private PajeonFurnace furnace;
    private string direction;

    [SerializeField]
    private GameObject pajeonImage;

    [SerializeField]
    private AudioManager audioManager;
    [SerializeField]
    private string successSound;
    [SerializeField]
    private string failureSound;

    [SerializeField]
    private Transform[] locTransform;

    private Vector3 dragStartPosition;
    private int[] correctSequence;
    private int currentIndex = 0;
    private bool hasFailed = true;
    private GameObject[] spawnedPrefabs; // 이전에 생성된 프리팹들을 추적하기 위한 배열


    public GameObject[] directionPrefabs;
    public GameObject PaJeonPopUp;
    public GameObject PaJeonPrefab;

    private void Start()
    {
        //오디오 설정
        audioManager = FindObjectOfType<AudioManager>();
        successSound = "successSound";
        failureSound = "failureSound";
    }

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

        for (int i = 0; i < correctSequence.Length; i++)
        {
            GameObject directionPrefab = Instantiate(directionPrefabs[correctSequence[i]], transform);
            directionPrefab.transform.position = locTransform[i].position;

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
                            direction = "Right";
                            CheckDirection(3);
                        }
                        else
                        {
                            direction = "Left";
                            CheckDirection(2);
                        }
                    }
                    else
                    {
                        if (dragDirection.y > 0)
                        {
                            direction = "Up";
                            CheckDirection(0);
                        }
                        else
                        {
                            direction = "Down";
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
            pajeonImage.transform.rotation = Quaternion.Euler(0, 0, 0);
            switch (direction)
            {
                case "Up":
                    break;
                case "Right":
                    pajeonImage.transform.rotation = Quaternion.Euler(0, 0, 90);
                    break;
                case "Down":
                    pajeonImage.transform.rotation = Quaternion.Euler(0, 0, 180);
                    break;
                case "Left":
                    pajeonImage.transform.rotation = Quaternion.Euler(0, 0, 270);
                    break;
            }
            animator.SetTrigger("isAnim");

            currentIndex++;

            if (currentIndex >= correctSequence.Length)
            {
                audioManager.Play(successSound);

                Debug.Log("성공입니다!");
                hasFailed = true;
                GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
                Instantiate(PaJeonPrefab, playerObject.transform.position, Quaternion.identity);
                PlayerServing playerServing = playerObject.GetComponent<PlayerServing>();
                playerServing.isCarryingFood= false;

                pajeonImage.transform.rotation = Quaternion.Euler(0, 0, 0);

                furnace.ExitPajeonMiniGame();
            }
        }
        else
        {
            audioManager.Play(failureSound);

            Debug.Log("실패입니다.");
            hasFailed = true;

            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            PlayerServing playerServing = playerObject.GetComponent<PlayerServing>();
            playerServing.isCarryingFood = false;

            furnace.ExitPajeonMiniGame();
        }
    }
}
