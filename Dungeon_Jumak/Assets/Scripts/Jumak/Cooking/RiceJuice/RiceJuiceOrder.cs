//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;

[DisallowMultipleComponent]
public class RiceJuiceOrder : MonoBehaviour
{
    #region Variables

    //Black Panel Object
    [Header("Home Panel에서 블랙 패널 오브젝트의 위치")]
    [SerializeField] private int blackPanelIndex;

    [Header("식혜 미니게임 부모 오브젝트")]
    [SerializeField] private GameObject miniGameParent;

    //Rice juice prefab object
    [Header("식혜 프리팹")]
    public GameObject riceJuicePrefab;

    //Table Index on Collision
    [Header("충돌한 테이블의 인덱스")]
    [SerializeField] private int tableIdx; //부딪힌 테이블의 인덱스

    //Sign Player In Collider
    private bool playerInCollider = false;

    //Circle Collider for Detect Player
    private CapsuleCollider2D capsuleCollider;

    //Player Serving Script
    private PlayerServing player;

    //Data
    private Data data;
    #endregion

    void Start()
    {
        blackPanelIndex = 7;

        //Get Component
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        player = FindObjectOfType<PlayerServing>();

        //Get Data
        data = DataManager.Instance.data;
    }

    void Update()
    {
        //If can play minigame, start minigame
        StartGame();

        //If success mini game, place rice juice on table
        PlaceRiceJuice();
    }

    //Check Start Game
    private void StartGame()
    {
        //If Play is Staying Collider
        if (playerInCollider)
        {
            //Check Input
            if (Input.GetMouseButtonDown(0))
            {
                //Update mouse position
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                //Set z position
                mousePosition.z = transform.position.z;

                //is not playing minigame and click minigame area
                if (capsuleCollider.OverlapPoint(mousePosition) && !data.isMiniGame)
                {
                    //Debug.Log
                    Debug.Log("감지");

                    //Convert mini game sign
                    data.isMiniGame = true;

                    //Get Customer GameObject
                    GameObject customer = transform.parent.gameObject;

                    //Find Seat Index
                    for (int i = 0; i < data.onTables.Length; i++)
                    {
                        if (i == customer.GetComponent<CustomerMovement>().seatIndex)
                            //Convert table MiniGame Sign
                            data.tableMiniGame[i] = true;
                    }

                    //Get Mini game Parent
                    miniGameParent = GameObject.Find("Rice Juice Manager").transform.GetChild(0).gameObject;

                    //Active Mini Game
                    miniGameParent.SetActive(true);

                    //Active Black Panel
                    GameObject.Find("[Panel] Jumak").transform.GetChild(blackPanelIndex).gameObject.SetActive(true);
                }
            }
        }
    }

    //Place Rice Juice On Table
    private void PlaceRiceJuice()
    {
        //if suceess mini game
        if (data.successRiceJuiceMiniGame)
        {
            //avoid duplication
            data.successRiceJuiceMiniGame = false;

            //loop for length
            for (int i = 0; i < data.tableMiniGame.Length; i++)
            {
                //Find Table Playing MiniGame
                if (data.tableMiniGame[i])
                {
                    //Convert tableMiniGame Sign for next minigame
                    data.tableMiniGame[i] = false;

                    //Instantiate prefab on table
                    GameObject instance = Instantiate(riceJuicePrefab, player.tables[i].transform.GetChild(0));

                    //Reset Location
                    instance.transform.localPosition = Vector3.zero;

                    //Convert onTables Sign for check food on table
                    data.onTables[i] = true;
                }

            }

        }
    }

    //On Trigger Stay
    private void OnTriggerStay2D(Collider2D other)
    {
        //When player is staying collider
        if (other.CompareTag("Player"))
            //Convert Sign
            playerInCollider = true;
    }

    //On Trigger Exit
    void OnTriggerExit2D(Collider2D other)
    {
        //When player exit collider
        if (other.CompareTag("Player"))
            //Conver Sign
            playerInCollider = false;
    }
}
