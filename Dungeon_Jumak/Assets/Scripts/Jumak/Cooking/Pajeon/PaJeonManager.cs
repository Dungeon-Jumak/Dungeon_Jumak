//System
using System.Collections;
using Unity.VisualScripting;

//Unity
using UnityEngine;

[DisallowMultipleComponent]
public class PaJeonManager : MonoBehaviour
{
    #region Variables
    //Pajeon Animator
    [Header("파전 뒤집는 애니메이터")]
    [SerializeField] private Animator animator;

    //Pajeon Furnace Script
    [Header("파전 화로 스크립트")]
    [SerializeField] private PajeonFurnace furnace;

    //Pajeon Image
    [Header("파전 이미지")]
    [SerializeField] private GameObject pajeonImage;

    //Pajeon Prefab
    [Header("손에 들 기본 파전 프리팹")]
    [SerializeField] GameObject basePaJeonPrefab;

    [Header("손에 들 파전 프리팹")]
    public GameObject paJeonPrefab;

    [Header("남은 파전 요리의 갯수")]
    public int curRemainPajeonCount;

    //Location Transform Array for place arrowDirections
    [Header("화살표 배치를 위한 위치 트랜스폼 배열")]
    [SerializeField] private Transform[] locTransform;

    //Player's Hand
    [Header("파전을 들 플레이어 손 오브젝트")]
    [SerializeField] private Transform playerHand;

    //Drage Start Position
    private Vector3 dragStartPosition;

    //arrow prefabs array
    public GameObject[] arrowPrefabs;

    //arrow direction string
    private string arrowDirection;

    //Array to save arrow numbers
    private int[] arrowNums;

    //current arrowDirection indx
    private int currentIndex = 0;

    //isPlaying sign
    private bool isPlaying = false;
    #endregion

    void Update()
    {
        //Start game through button
        //ChckDrag
        CheckDrag();
    }

    #region Mehtods Related Start

    //For Asstignment Button
    public void StartButton()
    {
        if(playerHand.transform.childCount == 0)
            StartCoroutine(StartGameInDelay());
    }

    //For Delay
    IEnumerator StartGameInDelay()
    {
        yield return new WaitForSeconds(0.1f);
        StartGame();
    }

    //Start Game
    public void StartGame()
    {
        isPlaying = true;

        //Initialize Current Index
        currentIndex = 0;

        //Initialize Drag Start Position
        dragStartPosition = Vector3.zero;

        //Instantiate pajeon prefab

        //Random Gen
        RandomArrowGenerator();
    }

    #endregion

    #region Mehtods Related Checking

    //Check Arrow
    void CheckArrow(int arrowNumber)
    {
        //if current arrow equal to arrow number
        if (arrowNums[currentIndex] == arrowNumber)
        {
            //Initialize rotation of pajeon
            pajeonImage.transform.rotation = Quaternion.Euler(0, 0, 0);

            //rotation according to arrow direction
            switch (arrowDirection)
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

            //play flip animation
            animator.SetTrigger("isAnim");

            //increase index
            currentIndex++;

            //if success
            if (currentIndex >= arrowNums.Length)
            {

                if (curRemainPajeonCount <= 0)
                    paJeonPrefab = basePaJeonPrefab;

                curRemainPajeonCount--;

                GameManager.Sound.Play("SFX/Jumak/[S] MiniGame Success", Define.Sound.Effect, false);

                isPlaying = false;

                GameObject newPajeonPrefab = Instantiate(paJeonPrefab, playerHand.position, Quaternion.identity);

                //Get PlayerServing
                PlayerServing playerServing = GameObject.Find("Chr_Player").GetComponent<PlayerServing>();

                //Pick Up Pajeon
                playerServing.PickUpPajeon(newPajeonPrefab);

                //isCarrying true
                playerServing.isCarryingFood = true;

                //Initialize PajeonImage rotation
                pajeonImage.transform.rotation = Quaternion.Euler(0, 0, 0);

                //Remove Arrow
                RemoveArrow();

                //Exit Pajeon MiniGame
                furnace.ExitPajeonMiniGame();
            }
        }
        //if fail
        else
        {
            Fail();
        }
    }

    public void Fail()
    {
        GameManager.Sound.Play("SFX/Jumak/[S] MiniGame Failure", Define.Sound.Effect, false);

        isPlaying = false;

        //Get PlayerServing
        PlayerServing playerServing = GameObject.Find("Chr_Player").GetComponent<PlayerServing>();

        //isCarrying false
        playerServing.isCarryingFood = false;

        //Remove Arrow
        RemoveArrow();

        //Exit Pajeon MiniGame
        furnace.ExitPajeonMiniGame();

    }

    //To Check Drag
    public void CheckDrag()
    {
        //To Detect Touch Down
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            //If is playing
            if (isPlaying)
            {
                //Update drag end position each device
                if (Input.touchCount > 0)
                {
                    //Update drag start position by touch position
                    dragStartPosition = Input.GetTouch(0).position;
                }
                else
                {
                    //Update drag start position by Mouse position
                    dragStartPosition = Input.mousePosition;
                }
            }
        }
        //To Detect Touch Up
        else if (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            //If is playing
            if (isPlaying)
            {
                //drag end position
                Vector3 dragEndPosition;

                //Update drag end position each device
                if (Input.touchCount > 0)
                {
                    //Update drag end position by touch position
                    dragEndPosition = Input.GetTouch(0).position;
                }
                else
                {
                    //Update drag end position by Mouse position
                    dragEndPosition = Input.mousePosition;
                }

                //Update drag arrow direction
                Vector3 dragArrowDirection = dragEndPosition - dragStartPosition;

                //Mimimum drag distance
                float minDragDistance = 20f;

                //if dragArrowDirection's size greater than minimum drag distance
                if (dragArrowDirection.magnitude > minDragDistance)
                {
                    //Compute Distance according to absolute value
                    if (Mathf.Abs(dragArrowDirection.x) > Mathf.Abs(dragArrowDirection.y))
                    {
                        //positive x = right
                        if (dragArrowDirection.x > 0)
                        {
                            //update arrow direction
                            arrowDirection = "Right";

                            //check arrow direction
                            CheckArrow(3);
                        }
                        //negative x = left
                        else
                        {
                            //update arrow direction
                            arrowDirection = "Left";

                            //check arrow direction
                            CheckArrow(2);
                        }
                    }
                    else
                    {
                        //positive y = up
                        if (dragArrowDirection.y > 0)
                        {
                            //update arrow direction
                            arrowDirection = "Up";

                            //check arrow direction
                            CheckArrow(0);
                        }
                        //negative y = down
                        else
                        {
                            //update arrow direction
                            arrowDirection = "Down";

                            //check arrow direction
                            CheckArrow(1);
                        }
                    }
                }
            }
        }
    }

    #endregion

    #region Methods Related Generate and Remove Arrows

    //Random Arrow Generator
    public void RandomArrowGenerator()
    {
        //define new arrow number array
        arrowNums = new int[4];

        //generate random arrow num
        for (int i = 0; i < arrowNums.Length; i++)
        {
            arrowNums[i] = Random.Range(0, arrowPrefabs.Length);
        }

        //instaniate arrow and place arrow
        for (int i = 0; i < arrowNums.Length; i++)
        {
            //Prefab Instaniate
            GameObject newArrowPrefab = Instantiate(arrowPrefabs[arrowNums[i]], transform);

            //Place Arrow
            newArrowPrefab.transform.position = locTransform[i].position;

            //Resize Arrow
            newArrowPrefab.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
        }
    }

    //Remove Arrow
    public void RemoveArrow()
    {
        for (int i = arrowNums.Length - 1; i >= 0; i--)
        {
            //Destroy Arrow Object
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    #endregion
}
