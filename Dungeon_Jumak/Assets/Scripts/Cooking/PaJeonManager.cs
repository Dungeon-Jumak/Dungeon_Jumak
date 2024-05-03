using UnityEngine;
using System.Collections;

public class PaJeonManager : MonoBehaviour
{
    public GameObject[] directionPrefabs;
    public GameObject PaJeonPopUp;
    public GameObject PaJeonPrefab;

    private Vector3 dragStartPosition;
    private int[] correctSequence;
    private int currentIndex = 0;
    private bool hasFailed = true;
    private GameObject[] spawnedPrefabs; // ������ ������ �����յ��� �����ϱ� ���� �迭

    [SerializeField]
    private AudioManager audioManager;
    [SerializeField]
    private string successSound;
    [SerializeField]
    private string failureSound;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private PajeonFurnace furnace;
    private string direction;

    [SerializeField]
    private GameObject pajeonImage;

    private void Start()
    {
        //����� ����
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

        // ������ ������ �����յ� ����
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
        spawnedPrefabs = new GameObject[4]; // ������ �����յ��� �����ϱ� ���� �迭 �ʱ�ȭ

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

            // ������ �����յ��� �����ϱ� ���� �迭�� �߰�
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

                Debug.Log("�����Դϴ�!");
                hasFailed = true;
                GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
                Instantiate(PaJeonPrefab, playerObject.transform.position, Quaternion.identity);
                PlayerServing playerServing = playerObject.GetComponent<PlayerServing>();
                playerServing.isCarryingFood = false;

                pajeonImage.transform.rotation = Quaternion.Euler(0, 0, 0);

                furnace.ExitPajeonMiniGame();
            }
        }
        else
        {
            audioManager.Play(failureSound);

            Debug.Log("�����Դϴ�.");
            hasFailed = true;

            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            PlayerServing playerServing = playerObject.GetComponent<PlayerServing>();
            playerServing.isCarryingFood = false;

            furnace.ExitPajeonMiniGame();
        }
    }
}