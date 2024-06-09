// System
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;          // 시스템 런타임 컴파일러 서비스 사용
using UnityEditor.Animations;
using UnityEditor.PackageManager.Requests;

// Unity
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class CustomerMovement : MonoBehaviour
{
    #region Variables
    //Index of Assigned Seat
    [Header("배정받은 자리 인덱스(Don't touch!)")]
    public int seatIndex;

    //Customer Move Speed
    [Header("손님의 이동속도")]
    [SerializeField] private float speed = 3f;

    //Speech Box GameObject
    [Header("사람이 가득 찼을 때 나올 말풍선 프리팹")]
    [SerializeField] private GameObject speech_Box_Full;

    [Header("랜덤으로 등장하는 손님 종류의 수")]
    [SerializeField] private int numberOfCustomerType;

    //Bool to use sign
    private bool isFull = false;       
    private bool isArrive = false;     
    private bool isReturn = false;      
    private bool isJustreturn = false;  

    //Waypoint index of Destination
    private int wayPointIndex;     

    ///////////////////////////////////////////////////////////////////
    ///                                                             ///
    ///                      [Serving Table]                        ///
    ///                                                             ///
    ///     Seat0 [Dansang1] Seat1      Seat2 [Dansang2] Seat3      ///
    ///                                                             ///
    ///     Seat4 [Dansang3] Seat5      Seat6 [Dansang4] Seat7      ///
    ///                                                             ///
    ///     Seat8 [Dansang5] Seat9     Seat10 [Dansang6] Seat11     ///
    ///                                                             ///
    ///////////////////////////////////////////////////////////////////

    //List of WayPoints
    [SerializeField] private List<Transform> Route1_Left;        //Seat0
    [SerializeField] private List<Transform> Route1_Right;       //Seat1

    [SerializeField] private List<Transform> Route2_Left;        //Seat2
    [SerializeField] private List<Transform> Route2_Right;       //Seat3

    [SerializeField] private List<Transform> Route3_Left;        //Seat4
    [SerializeField] private List<Transform> Route3_Right;       //Seat5

    [SerializeField] private List<Transform> Route4_Left;        //Seat6
    [SerializeField] private List<Transform> Route4_Right;       //Seat7

    [SerializeField] private List<Transform> Route5_Left;        //Seat8
    [SerializeField] private List<Transform> Route5_Right;       //Seat9

    [SerializeField] private List<Transform> Route6_Left;        //Seat10
    [SerializeField] private List<Transform> Route6_Right;       //Seat11

    //List of Routes
    private List<List<Transform>> RouteList = new List<List<Transform>>();

    //Final Route Lise
    private List<Transform> FinRoute;

    //Special WayPoint
    private Transform StartPoint;
    private Transform StopPoint; // 자리가 꽉찼을 경우 멈출 포인트

    //Data
    private Data data;

    //OrederMenu Script for Oreder Menu
    private OrderMenu orderMenu;

    //Sprite Renderer
    private SpriteRenderer spriteRenderer;

    //Animator
    private Animator animator;

    //Jumak Scene
    private JumakScene jumakScene;

    //Vector for Compute Location
    private Vector3 curPosition;        //손님의 현재 위치 (방향 계산을 위함)
    private Vector3 lastPosition;       //손님의 가장 최근 위치 (방향 계산을 위함)
    private Vector3 currentDir;         //현재 손님의 방향 (애니메이션을 위함)

    #endregion

    private void Start()
    {
        //Initialize Customer System's Variables
        seatIndex = 0;

        isFull = false;
        isArrive = false;
        isReturn = false;
        isJustreturn = false;

        wayPointIndex = 0;

        //Initialize List Related Routes
        InitializeRoute();

        FinRoute = new List<Transform>() { null, null, null };

        StartPoint = GameObject.Find("StartPoint").transform;  
        StopPoint = GameObject.Find("StopPoint").transform;     

        //Load Data
        data = DataManager.Instance.data;

        //Get Component
        orderMenu = GetComponent<OrderMenu>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        jumakScene = GameObject.Find("@Scene").GetComponent<JumakScene>();

        //Random Animator Controller
        ChangeCustomerAnimatorController();

        //Initialize Last Position for Set Direction
        lastPosition = transform.position;

        //For Set New Seat
        SetNewSeat();                                          
    }

    private void Update()
    {
        //Customer Moving System
        CustomerMovingSystem();
    }

    //Initialze Variable
    private void Initialize()
    {
        //Initialize Customer Movement's Variables
        isFull = false;
        isArrive = false;
        isReturn = false;
        isJustreturn = false;

        wayPointIndex = 0;

        //Initialize Other Component Variables
        orderMenu.isEat = false;

        //Set New Animator Controller
        ChangeCustomerAnimatorController();

        //Set New Seat
        SetNewSeat();

        //ObjectPool -> Return Object
        ObjectPool.ReturnObject(this);
    }

    //Change Customer Sprite
    private void ChangeCustomerAnimatorController()
    {
        //Get Random Index
        int randomIndex = Random.Range(1, numberOfCustomerType + 1);

        //Debug.Log
        Debug.Log(randomIndex + "번째 애니메이션 컨트롤러");

        //Get Path (Animatior Override Controller)
        var path = "Animator Override Controller/[A] Customer0" + randomIndex.ToString();

        //Get Animator Component
        var animator = transform.GetComponent<Animator>();

        //Overriding Animator in path
        animator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load(path);

    }

    #region Methods Related Route

    //For Initialze Route
    private void InitializeRoute()
    {
        //Route1 = Table1 Left
        Route1_Left.Add(GameObject.Find("Left_4").transform);
        Route1_Left.Add(GameObject.Find("Left_1").transform);
        Route1_Left.Add(GameObject.Find("Seat_L1").transform);

        //Route2 = Table1 Right
        Route1_Right.Add(GameObject.Find("Center_4").transform);
        Route1_Right.Add(GameObject.Find("Center_1").transform);
        Route1_Right.Add(GameObject.Find("Seat_R1").transform);

        //Route3 = Table2 Left
        Route2_Left.Add(GameObject.Find("Center_4").transform);
        Route2_Left.Add(GameObject.Find("Center_1").transform);
        Route2_Left.Add(GameObject.Find("Seat_L2").transform);

        //Route4 = Table2 Right
        Route2_Right.Add(GameObject.Find("Right_4").transform);
        Route2_Right.Add(GameObject.Find("Right_1").transform);
        Route2_Right.Add(GameObject.Find("Seat_R2").transform);

        //Route5 = Table3 Left
        Route3_Left.Add(GameObject.Find("Left_4").transform);
        Route3_Left.Add(GameObject.Find("Left_2").transform);
        Route3_Left.Add(GameObject.Find("Seat_L3").transform);

        //Route6 = Table3 Right
        Route3_Right.Add(GameObject.Find("Center_4").transform);
        Route3_Right.Add(GameObject.Find("Center_2").transform);
        Route3_Right.Add(GameObject.Find("Seat_R3").transform);

        //Route7 = Table4 Left
        Route4_Left.Add(GameObject.Find("Center_4").transform);
        Route4_Left.Add(GameObject.Find("Center_2").transform);
        Route4_Left.Add(GameObject.Find("Seat_L4").transform);

        //Route8 = Table4 Right
        Route4_Right.Add(GameObject.Find("Right_4").transform);
        Route4_Right.Add(GameObject.Find("Right_2").transform);
        Route4_Right.Add(GameObject.Find("Seat_R4").transform);

        //Route9 = Table5 Left
        Route5_Left.Add(GameObject.Find("Left_4").transform);
        Route5_Left.Add(GameObject.Find("Left_3").transform);
        Route5_Left.Add(GameObject.Find("Seat_L5").transform);

        //Route10 = Table5 Right
        Route5_Right.Add(GameObject.Find("Center_4").transform);
        Route5_Right.Add(GameObject.Find("Center_3").transform);
        Route5_Right.Add(GameObject.Find("Seat_R5").transform);

        //Route11 = Table6 Left
        Route6_Left.Add(GameObject.Find("Center_4").transform);
        Route6_Left.Add(GameObject.Find("Center_3").transform);
        Route6_Left.Add(GameObject.Find("Seat_L6").transform);

        //Route12 = Table6 Right
        Route6_Right.Add(GameObject.Find("Right_4").transform);
        Route6_Right.Add(GameObject.Find("Right_3").transform);
        Route6_Right.Add(GameObject.Find("Seat_R6").transform);

        //Add WayPoint Lists(Routes) To RouteList
        RouteList.Add(Route1_Left);
        RouteList.Add(Route1_Right);
        RouteList.Add(Route2_Left);
        RouteList.Add(Route2_Right);
        RouteList.Add(Route3_Left);
        RouteList.Add(Route3_Right);
        RouteList.Add(Route4_Left);
        RouteList.Add(Route4_Right);
        RouteList.Add(Route5_Left);
        RouteList.Add(Route5_Right);
        RouteList.Add(Route6_Left);
        RouteList.Add(Route6_Right);
    }

    //For Set New Seat
    private void SetNewSeat()
    {
        //Loop Count
        int count = 0; 

        //Check Full
        for (int i = 0; i < data.maxSeatSize; i++)
        {
            if (data.isAllocated[i])
                count++;
        }

        if (count == data.maxSeatSize)
        {
            isFull = true;
            return;
        }
        else isFull = false;

        //Descision Random Index of Seat
        int randomSeat = Random.Range(0, data.maxSeatSize);

        //Find do not allocate seat
        while (data.isAllocated[randomSeat])
        {
            randomSeat = Random.Range(0, data.maxSeatSize);
        }

        //If find Empty Seat, allocate seat
        seatIndex = randomSeat;
        data.isAllocated[seatIndex] = true;

        //Update Final Route
        for (int i = 0; i < 3; i++)
        {
            FinRoute[i] = RouteList[seatIndex][i];
        }
    }

    #endregion

    #region Methods Related Move and Eating

    //Base Method for Customer Move
    private void CustomerMove(Transform TargetTransform)
    {
        //Moving
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(curPosition, TargetTransform.position, step);
    }

    //Customer Moving System Method
    private void CustomerMovingSystem()
    {
        //Initialize When GameOver
        if (!jumakScene.start) Initialize();

        //Set Direction for Animation
        SetDirection();

        //Move if is not full
        if (!isFull)
        {
            //If remain Waypoint and do not arrive
            if (wayPointIndex < FinRoute.Count && !isArrive)
            {
                //Customer Move
                CustomerMove(FinRoute[wayPointIndex]);

                //Increase Index of waypoint, if customer arrive waypoint
                if (Vector3.Distance(FinRoute[wayPointIndex].position, curPosition) == 0f)
                    wayPointIndex++;
            }
            else if (!isArrive) //Arrive Final WayPoint
            {
                //Avoid Duplication
                isArrive = true;

                //Play Animation
                animator.SetBool("isSit", true);

                //Active isCustomer : mean to sit customer on seat index
                data.isCustomer[seatIndex] = true;

                //Oreder New Menu (at orderMenu Script)
                orderMenu.OrderNewMenu();
            }

            //if customer finish to eat
            if (isReturn)
            {
                //Reverse
                if (wayPointIndex >= 0)
                {
                    CustomerMove(FinRoute[wayPointIndex]);

                    if (Vector3.Distance(FinRoute[wayPointIndex].position, curPosition) == 0f)
                        wayPointIndex--;

                }
                //If customer reverse all waypoints
                else
                {
                    //Move to Start Point
                    CustomerMove(StartPoint);

                    //Initialize when arrive start point
                    if (Vector3.Distance(StartPoint.position, curPosition) == 0f) Initialize();
                }
            }
        }
        //If seat is full
        else if (isFull && !isArrive)
        {
            //Move to Stop Point
            CustomerMove(StopPoint);

            //If customer arrive stopPoint
            if (Vector3.Distance(StopPoint.position, curPosition) == 0f)
            {
                //Avoid Duplication
                isArrive = true;

                //Play Animation
                animator.SetBool("isStop", true);

                //Show Speech Box related Full
                GameObject.Instantiate(speech_Box_Full, GameObject.Find("Speech_Boxes").transform);

                //JustOut
                Invoke("JustOut", 2f);
            }
        }

        //If JustOut
        if (isJustreturn)
        {
            //Move to Start Point
            CustomerMove(StartPoint);

            //Initialize when arrive start point
            if (Vector3.Distance(StartPoint.position, curPosition) == 0f) Initialize();
        }
    }

    //Set Direction and Play Animation
    private void SetDirection()
    {
        //Load current position
        curPosition = transform.position;

        //Normalized direction vector
        currentDir = (curPosition - lastPosition).normalized;

        //if is not zero vector
        if (currentDir != Vector3.zero)
        {
            //Play Animation
            animator.SetFloat("dirX", currentDir.x);
            animator.SetFloat("dirY", currentDir.y);

            //Update LastPosition
            lastPosition = curPosition;
        }
        //Play base Animation when zero vector
        else animator.SetFloat("dirY", -1f);


        //Change Rendering according to player and customer location
        if (curPosition.y < GameObject.Find("Chr_Player").transform.position.y)     
            spriteRenderer.sortingLayerName = "UpThanPlayer";                       
        else                                                                        
            spriteRenderer.sortingLayerName = "Player";                             
    }

    //Method for Eat Food
    public void EatFood()
    {
        //Play Eating Animation
        animator.SetBool("isEat", true);

        //Leave 3 seconds later when finish to eat
        Invoke("EatAndLeave", 3f);
    }

    //Finish Eating and Leave
    public void EatAndLeave()
    {
        //Play Stand Animation on seat
        animator.SetBool("isEat", false);
        animator.SetBool("isSit", false);
        animator.SetFloat("dirX", -currentDir.x);

        //Active return
        isReturn = true;

        //Deallocate seat
        data.isAllocated[seatIndex] = false;

        //Decrease WayPoint for Return
        wayPointIndex--;

        //Decrease Head Count of Customer
        data.customerHeadCount--;

        //InActive isCustomer for Detect Customer
        data.isCustomer[seatIndex] = false;

        //InActive onTables for Detect Food on table
        data.onTables[seatIndex] = false;

        //Active isFinEat for Detect to finish eating
        data.isFinEat[seatIndex] = true;
    }

    //Just Return when all seat is full
    private void JustOut()
    {
        //Inactive Stop Animation for Move to start point
        animator.SetBool("isStop", false);

        //Destroy Speech box related full
        Destroy(GameObject.Find("UI_Speech_Box_Full(Clone)"));

        //Active isJustreturn for return
        isJustreturn = true;
    }

    //Time Out
    public void TimeOut()
    {
        //Play Animation -> Stand
        animator.SetBool("isSit", false);
        animator.SetFloat("dirX", currentDir.x * -1);

        //Active isReturn for Return
        isReturn = true;

        //Decrease headcount for customer
        data.customerHeadCount--;

        //Deallocate seat
        data.isAllocated[seatIndex] = false;

        //Decrease Waypoint
        wayPointIndex--;

        //Inactive isCustomer for detect customer on seat
        data.isCustomer[seatIndex] = false;
    }

    #endregion
}
