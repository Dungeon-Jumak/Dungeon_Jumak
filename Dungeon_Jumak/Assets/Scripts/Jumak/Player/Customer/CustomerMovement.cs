// System
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;          // �ý��� ��Ÿ�� �����Ϸ� ���� ���

// Unity
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class CustomerMovement : MonoBehaviour
{
    //---�ڸ� ���� ����---//
    [Header("�������� �ڸ� �ε���(Don't touch!)")]
    public int seatIndex;

    [SerializeField, Header("�մ��� �̵��ӵ�")]
    private float speed = 3f;

    //---UI ���� ���� (Speech_Box)---//
    [SerializeField, Header("����� ���� á�� �� ���� ��ǳ�� ������")]
    private GameObject speech_Box_Full; //����� ����á�� �� ���� ��ǳ�� ������ 

    //---�ڸ� ���� boolean �� ---//
    private bool isFull = false;         //�ڸ��� �������ִ��� �ƴ��� �����ϴ� ����
    private bool isArrive = false;      //���� ���� �Ǵ� ���� (�ڸ�)
    private bool isReturn = false;      //�ڸ����� ���ư� �� ����� ���� (�ߺ� ���� ����)
    private bool isJustreturn = false;  //�ڸ��� ������ �׳� ���ư� �� ����� ����

    //---�մ� ������ ���� ����---//
    private int wayPointIndex;      //��� WayPoint�� Index �մ��� ���� �����ӿ� �����ϱ� ����

    //---�� ��η� ���� Waypoints�� ���� ����Ʈ---// 

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

    private List<Transform> Route1_Left;        //Seat0
    private List<Transform> Route1_Right;       //Seat1

    private List<Transform> Route2_Left;        //Seat2
    private List<Transform> Route2_Right;       //Seat3

    private List<Transform> Route3_Left;        //Seat4
    private List<Transform> Route3_Right;       //Seat5

    private List<Transform> Route4_Left;        //Seat6
    private List<Transform> Route4_Right;       //Seat7

    private List<Transform> Route5_Left;        //Seat8
    private List<Transform> Route5_Right;       //Seat9

    private List<Transform> Route6_Left;        //Seat10
    private List<Transform> Route6_Right;       //Seat11

    //---�پ��� ��θ� ������ ��� ����Ʈ---//
    private List<List<Transform>> RouteList = new List<List<Transform>>();

    //---���� ������ Route---//
    private List<Transform> FinRoute;

    //---Ư�� waypoint---//
    private Transform StartPoint;
    private Transform StopPoint; // �ڸ��� ��á�� ��� ���� ����Ʈ

    //---������---//
    private Data data;

    //---�ֹ� ����---//
    private OrderMenu orderMenu;

    //---������ ������ ��������Ʈ������---//
    private SpriteRenderer spriteRenderer;

    //---�մ� �ִϸ�����---//
    private Animator animator;

    //---�ָ� �� ������Ʈ �ҷ����� ���� ����---//
    private JumakScene jumakScene;

    //---��ġ ����� �� ���� ����---//
    private Vector3 curPosition;        //�մ��� ���� ��ġ (���� ����� ����)
    private Vector3 lastPosition;       //�մ��� ���� �ֱ� ��ġ (���� ����� ����)
    private Vector3 currentDir;         //���� �մ��� ���� (�ִϸ��̼��� ����)

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void Start()
    {
        //--- Initialize Customer System's Variables ---//
        seatIndex = 0;

        isFull = false;
        isArrive = false;
        isReturn = false;
        isJustreturn = false;

        wayPointIndex = 0;

        //--- ��θ� �ʱ�ȭ �ϱ� ���� �ζ��� �Լ� ---//
        InitializeRoute();

        StartPoint = GameObject.Find("StartPoint").transform;   //�մ��� ���� ��ġ
        StopPoint = GameObject.Find("StopPoint").transform;     //�մ��� ���� ��ġ (�ڸ��� ���� á�� ��� ���ߴ� Point)

        //--- Load Data ---//
        data = DataManager.Instance.data;

        //--- Get Component ---//
        orderMenu = GetComponent<OrderMenu>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        jumakScene = GameObject.Find("@Scene").GetComponent<JumakScene>();

        //--- ������ �ҷ����� ���� lastPosition�� ���� Transform ��ġ�� �ʱ�ȭ ---//
        lastPosition = transform.position;

        //--- ���ο� ��θ� �ҷ����� ���� �ζ��� �Լ� ---//
        SetNewSeat();                                          
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void Update()
    {
        //--- Customer Moving System ---//
        CustomerMovingSystem();
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    //--- ������ �ʱ�ȭ�ϱ� ���� �ζ��� �Լ� ---//
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Initialize()
    {
        //--- Initialize Customer Movement's Variables ---//
        isFull = false;
        isArrive = false;
        isReturn = false;
        isJustreturn = false;

        wayPointIndex = 0;

        //--- Initialize Other Component Variables ---//
        orderMenu.isEat = false;

        //--- ���ο� �ڸ� �����ϴ� �ζ��� �Լ� ---//
        SetNewSeat();

        //--- �ʱ�ȭ ������ �� Ǯ�� ���� ������Ʈ ---//
        ObjectPool.ReturnObject(this);
    }

    //--- �մԵ��� ���� Waypoints�� �ʱ�ȭ�ϴ� �ζ��� �Լ� ---//
    [MethodImpl(MethodImplOptions. AggressiveInlining)] //AggressiveInlinng : ������ ��� �ζ���
    private void InitializeRoute()
    {
        //Route1 = Table1 ���� �ڸ�
        Route1_Left.Add(GameObject.Find("Left_4").transform);
        Route1_Left.Add(GameObject.Find("Left_1").transform);
        Route1_Left.Add(GameObject.Find("Seat_L1").transform);

        //Route2 = Table1 ������
        Route1_Right.Add(GameObject.Find("Center_4").transform);
        Route1_Right.Add(GameObject.Find("Center_1").transform);
        Route1_Right.Add(GameObject.Find("Seat_R1").transform);

        //Route3 = Table2 ���� �ڸ�
        Route2_Left.Add(GameObject.Find("Center_4").transform);
        Route2_Left.Add(GameObject.Find("Center_1").transform);
        Route2_Left.Add(GameObject.Find("Seat_L2").transform);

        //Route4 = Table2 ������ �ڸ�
        Route2_Right.Add(GameObject.Find("Right_4").transform);
        Route2_Right.Add(GameObject.Find("Right_1").transform);
        Route2_Right.Add(GameObject.Find("Seat_R2").transform);

        //Route5 = Table3 ���� �ڸ�
        Route3_Left.Add(GameObject.Find("Left_4").transform);
        Route3_Left.Add(GameObject.Find("Left_2").transform);
        Route3_Left.Add(GameObject.Find("Seat_L3").transform);

        //Route6 = Table3 ������ �ڸ�
        Route3_Right.Add(GameObject.Find("Center_4").transform);
        Route3_Right.Add(GameObject.Find("Center_2").transform);
        Route3_Right.Add(GameObject.Find("Seat_R3").transform);

        //Route7 = Table4 ���� �ڸ�
        Route4_Left.Add(GameObject.Find("Center_4").transform);
        Route4_Left.Add(GameObject.Find("Center_2").transform);
        Route4_Left.Add(GameObject.Find("Seat_L4").transform);

        //Route8 = Table4 ������ �ڸ�
        Route4_Right.Add(GameObject.Find("Right_4").transform);
        Route4_Right.Add(GameObject.Find("Right_2").transform);
        Route4_Right.Add(GameObject.Find("Seat_R4").transform);

        //Route9 = Table5 ���� �ڸ�
        Route5_Left.Add(GameObject.Find("Left_4").transform);
        Route5_Left.Add(GameObject.Find("Left_3").transform);
        Route5_Left.Add(GameObject.Find("Seat_L5").transform);

        //Route10 = Table5 ������ �ڸ�
        Route5_Right.Add(GameObject.Find("Center_4").transform);
        Route5_Right.Add(GameObject.Find("Center_3").transform);
        Route5_Right.Add(GameObject.Find("Seat_R5").transform);

        //Route11 = Table6 ���� �ڸ�
        Route6_Left.Add(GameObject.Find("Center_4").transform);
        Route6_Left.Add(GameObject.Find("Center_3").transform);
        Route6_Left.Add(GameObject.Find("Seat_L6").transform);

        //Route12 = Table6 ������ �ڸ�
        Route6_Right.Add(GameObject.Find("Right_4").transform);
        Route6_Right.Add(GameObject.Find("Right_3").transform);
        Route6_Right.Add(GameObject.Find("Seat_R6").transform);

        //RouteList�� ������� Add
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

    //--- ���� �ڸ� �����ϴ� �ζ��� �Լ� ---//
    [MethodImpl(MethodImplOptions. AggressiveInlining)]
    private void SetNewSeat()
    {
        int count = 0; //�ݺ� Ƚ���� ���� ����

        //--- �ڸ��� ����á���� �˻� ---//
        for (int i = 0; i < data.maxSeatSize; i++)
        {
            if (data.isAllocated[i])
                count++;
        }

        //--- �ڸ���ŭ �ݺ��� ���� ��� isFull = true �ϰ� ���� ---//
        if (count == data.maxSeatSize)
        {
            isFull = true;
            return;
        }
        else isFull = false;

        //--- �������� �ڸ� ���� ---//
        int randomSeat = Random.Range(0, data.maxSeatSize);

        //--- �Ҵ��� �ȵǾ� �ִ� �ڸ��� ã�� �� ���� �ݺ� ---//
        while (data.isAllocated[randomSeat])
        {
            randomSeat = Random.Range(0, data.maxSeatSize);
        }

        //--- �ڸ��� ã�Ҵٸ� �Ҵ� ---//
        seatIndex = randomSeat;
        data.isAllocated[seatIndex] = true;

        //FinRoute�� �Ҵ�� ��θ� ����
        for (int i = 0; i < FinRoute.Count; i++)
        {
            FinRoute[i] = RouteList[seatIndex][i];
        }
    }

    //---�մ� �⺻ �����ӿ� �����ϴ� �ζ��� �Լ�---//
    [MethodImpl(MethodImplOptions. AggressiveInlining)]
    private void CustomerMove(Transform TargetTransform)
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(curPosition, TargetTransform.position, step);
    }

    //--- �մ� �⺻ ������ �ý����� �Ѱ��ϴ� �ζ��� �Լ� ---//
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void CustomerMovingSystem()
    {
        //--- ���� ������ �ʱ�ȭ ---//
        if (!jumakScene.isStart) Initialize();

        //--- �ִϸ��̼��� �ҷ����� ���� ���� �մ��� ���ϰ� �ִ� ������ �˾ƺ��� ���� �ζ��� �Լ� ---//
        SetDirection();

        //---�ڸ��� ������ ���� �ʴٸ� ������ ����---//
        if (!isFull)
        {
            //--- ���� wayPointIndex�� ���� ����� ���̺��� �۰�, �������� ���� ���¶��---//
            if (wayPointIndex < FinRoute.Count && !isArrive)
            {
                //--- �մ� �̵� '�ζ���' �Լ� ---//
                CustomerMove(FinRoute[wayPointIndex]);

                //--- ��ǥ�ߴ� waypoint�� �����ϸ� �ε��� ���� ---//
                if (Vector3.Distance(FinRoute[wayPointIndex].position, curPosition) == 0f)
                    wayPointIndex++;
            }
            else if (!isArrive) //--- WayPoint Index�� ��� ��ȸ���� ��---//
            {
                //--- �ߺ� ���� ������ ���� bool ���� ���� ---//
                isArrive = true;

                //--- �ִϸ��̼� ���� ---//
                animator.SetBool("isSit", true);

                //--- �մ��� �ɾ����� ǥ���ϴ� isCustomer bool ���� ���� ---//
                data.isCustomer[seatIndex] = true;

                //--- �޴� �ֹ� �Լ� ���� ---//
                orderMenu.OrderNewMenu();
            }

            //--- ������ �� �Ծ��� �� isReturn Ȱ��ȭ ---//
            if (isReturn)
            {
                //--- WayPoint ����ȸ �� ---//
                if (wayPointIndex >= 0)
                {
                    CustomerMove(FinRoute[wayPointIndex]);

                    if (Vector3.Distance(FinRoute[wayPointIndex].position, curPosition) == 0f)
                        wayPointIndex--;

                }
                //--- ��� WayPoint�� ����ȸ ���� �� ---//
                else
                {
                    //--- ���� �������� �ǵ��ư� ---//
                    CustomerMove(StartPoint);

                    //--- ���������� �������� �� �ʱ�ȭ ---//
                    if (Vector3.Distance(StartPoint.position, curPosition) == 0f) Initialize();
                }
            }
        }
        //--- �ڸ��� ���� �� �׳� ���ư��� �б� ---//
        else if (isFull && !isArrive)
        {
            //--- ���� �������� �̵� ---//
            CustomerMove(StopPoint);

            //---���� point�� �Ÿ��� 0�̶�� ---//
            if (Vector3.Distance(StopPoint.position, curPosition) == 0f)
            {
                //--- �ߺ� ���� ���� bool ���� ��ȯ ---//
                isArrive = true;
                //--- �ִϸ��̼� ���� ---//
                animator.SetBool("isStop", true);
                //--- �ڸ��� ���� á���� �˸��� SpeechBox ������ ���� ---//
                GameObject.Instantiate(speech_Box_Full, GameObject.Find("Speech_Boxes").transform);
                //--- 2���� �׳� ���ư��� ���� JustOut Method ���� ---//
                Invoke("JustOut", 2f);
            }
        }

        //--- �׳� ���ư���� ��ȣ�� �ν� �Ǿ��� �� ---//
        if (isJustreturn)
        {
            //--- ���� ������ ���� �̵� ---//
            CustomerMove(StartPoint);

            //--- ���� ������ �������� �� �ʱ�ȭ ---//
            if (Vector3.Distance(StartPoint.position, curPosition) == 0f) Initialize();

        }
    }

    //--- ���� �մ� ������Ʈ�� ���� ���� �� ������ �����ϱ� ���� �ζ��� �Լ� ---//
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void SetDirection()
    {
        //--- ���� ��ġ���� �ҷ��� ---//
        curPosition = transform.position;

        //--- ���� ���� ǥ��ȭ ---//
        currentDir = (curPosition - lastPosition).normalized;

        //--- ���� ������ 0���Ͱ� �ƴ� ��� ���� ���⿡ �°� �ִϸ��̼� ���� �� lastPostion Update ---//
        if (currentDir != Vector3.zero)
        {
            //--- �ִϸ��̼� ���� ---//
            animator.SetFloat("dirX", currentDir.x);
            animator.SetFloat("dirY", currentDir.y);

            //--- ���� ��ġ�� �ֱ� ��ġ�� ������Ʈ ---//
            lastPosition = curPosition;
        }
        //--- 0������ ��� �⺻ �ִϸ��̼� ���� ---//
        else animator.SetFloat("dirY", -1f);


        //---������ ����---//
        if (curPosition.y < GameObject.Find("Chr_Player").transform.position.y)     // �մ��� �÷��̾�� �Ʒ��� �ִٸ�
            spriteRenderer.sortingLayerName = "UpThanPlayer";                       // �÷��̾�� ���� ������
        else                                                                        // �մ��� �÷��̾�� ���� �ִٸ�
            spriteRenderer.sortingLayerName = "Player";                             // �÷��̾�� �Ʒ��� ������
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    //--- ������ �Ա� �������� �� �Լ� --//
    public void EatFood()
    {
        //--- ���� �Դ� �ִϸ��̼� ���� ---//
        animator.SetBool("isEat", true);
        //--- ���� �԰� �ǵ��ư��� EanTandLeave Method 3f���� ���� ---//
        Invoke("EatAndLeave", 3f);
    }

    //--- ������ �� �԰� ������ ���ư��� �Լ� ---//
    public void EatAndLeave()
    {
        //--- �Ͼ�� �ִϸ��̼� ���� ---//
        animator.SetBool("isEat", false);
        animator.SetBool("isSit", false);
        animator.SetFloat("dirX", -currentDir.x);

        //--- ���ư��� ���� bool ���� ��ȯ ---//
        isReturn = true;
        //--- �ڸ� �Ҵ� ���� ---//
        data.isAllocated[seatIndex] = false;
        //--- ���ư��� ���� wayPointIndex 1���� ---//
        wayPointIndex--;
        //--- �մ� �� ���� ---//
        data.customerHeadCount--;
        //--- �ش� ���̺��� �մ� ���� bool ��ȣ ��ȯ ---//
        data.isCustomer[seatIndex] = false;
        //--- ���̺� �� ���� ���� bool ��ȣ ��ȯ ---//
        data.onTables[seatIndex] = false;
        //--- ���� �� ������ �����ϴ� bool ��ȣ ��ȯ ---//
        data.isFinEat[seatIndex] = true;
    }

    //--- �ڸ��� ��� �׳� ������ ���ư��� ���� ��ȣ�� �ٲٴ� �Լ� ---//
    private void JustOut()
    {
        //--- �ִϸ��̼� ����(���� ����) ---//
        animator.SetBool("isStop", false);
        //--- SpeechBox ������ Destroy ---//
        Destroy(GameObject.Find("UI_Speech_Box_Full(Clone)"));
        //--- �׳� ���ư��� ���� bool ���� ��ȯ ---//
        isJustreturn = true;
    }

    //--- �ֹ� ��� �ð��� ��� ������ �� �ѹ� ������ �Լ� ---//
    public void TimeOut()
    {
        //--- �ִϸ��̼� ���� ---//
        animator.SetBool("isSit", false);
        animator.SetFloat("dirX", currentDir.x * -1);

        //--- ����ȸ�� �ϱ� ���� bool ���� ��ȯ ---//
        isReturn = true;
        //--- �մ� �� ���� ---//
        data.customerHeadCount--;
        //--- �ڸ� �Ҵ� ���� ---//
        data.isAllocated[seatIndex] = false;
        //--- wayPointIndex 1 ���� ---//
        wayPointIndex--;
        //--- �ش� ���̺��� �մ� ���� bool ��ȣ ��ȯ ---//
        data.isCustomer[seatIndex] = false;
    } 
}
