using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerMovement : MonoBehaviour
{
    //---�� ��η� ���� Waypoints�� ���� ����Ʈ---// 
    public List<Transform> Route1_Left;
    public List<Transform> Route1_Right;

    public List<Transform> Route2_Left;
    public List<Transform> Route2_Right;

    public List<Transform> Route3_Left;
    public List<Transform> Route3_Right;

    public List<Transform> Route4_Left;
    public List<Transform> Route4_Right;

    public List<Transform> Route5_Left;
    public List<Transform> Route5_Right;

    public List<Transform> Route6_Left;
    public List<Transform> Route6_Right;

    //---�پ��� ��θ� ������ ��� ����Ʈ---//
    public List<List<Transform>> RouteList = new List<List<Transform>>();

    //---���� ������ Route---//
    public List<Transform> FinRoute;

    //---�ڸ� ���� ����---//
    public int seatIndex = 0;

    //---Ư�� waypoint---//
    [SerializeField]
    private Transform StartPoint;
    [SerializeField]
    private Transform StopPoint; // �ڸ��� ��á�� ��� ���� ����Ʈ

    //---�ڸ� ���� boolean �� ---//
    [SerializeField]
    private bool isFull = false; //�ڸ��� �������ִ��� �ƴ��� �����ϴ� ����
    [SerializeField]
    private bool isArrive = false; //���� ���� �Ǵ� ���� (�ڸ�)
    [SerializeField]
    private bool isReturn = false; //�ڸ����� ���ư� �� ����� ���� (�ߺ� ���� ����)
    [SerializeField]
    private bool isJustreturn = false;

    //---�մ� ������ ���� ����---//
    [SerializeField]
    private Vector3 CurPosition;
    [SerializeField]
    private int WayPointIndex = 0;
    [SerializeField]
    private float speed = 3f;

    [SerializeField]
    private bool isInitialize = false;

    //---UI ���� ���� (Speech_Box)---//
    [SerializeField]
    private GameObject speech_Box_Full; //����� ����á�� �� ���� ��ǳ�� ������ 

    //---������---//
    [SerializeField]
    private Data data;

    //---�ֹ� ����---//
    [SerializeField]
    private OrderMenu orderMenu;

    //---������ ������ ��������Ʈ������---//
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    //---�մ� �ִϸ�����---//
    private Animator animator;

    //---��ġ ����� �� ���� ����---//
    private Vector3 lastPosition;
    private Vector3 currentDir;

    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip[] audioClips;

    private void Start()
    {


        data = DataManager.Instance.data;
        orderMenu = GetComponent<OrderMenu>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        lastPosition = transform.position;

        StartPoint = GameObject.Find("StartPoint").transform;
        StopPoint = GameObject.Find("StopPoint").transform;

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

        SetNewSeat();

        for (int i = 0; i < FinRoute.Count; i++)
        {
            FinRoute[i] = RouteList[seatIndex][i];
        }
    }

    private void Update()
    {
        if (isInitialize) Initialize();

        SetDirection();

        //---�ڸ��� ������ ���� �ʴٸ� ������ ����---//
        if (!isFull)
        {
            //--- ���� waypointIndex�� ���� ����� ���̺��� �۴ٸ� ---//
            //--- �̵����� �迭�� �ε��� 0���� �迭ũ�� -1���� ---//
            if (WayPointIndex < FinRoute.Count && !isArrive)
            {
                //---�����Ӵ� �̵�---//
                CustomerMove(FinRoute[WayPointIndex]);

                //---waypoint�� �����ϸ� �ε��� ����---//
                if (Vector3.Distance(FinRoute[WayPointIndex].position, CurPosition) == 0f)
                    WayPointIndex++;
            }
            else if (!isArrive)
            {
                isArrive = true;
                data.isCustomer[seatIndex] = true; // �մ��� ���̺� ���� üũ
                animator.SetBool("isSit", true);
                orderMenu.OrderNewMenu();
            }

            //---�������� ��� 3���� ���ư� Invoke Return�� ���� isArrive�� True�� ��ȯ(�Ŀ� �������� �ٲ�ߵ�)---//
            if (isReturn)
            {
                if (WayPointIndex >= 0)
                {
                    CustomerMove(FinRoute[WayPointIndex]);

                    if (Vector3.Distance(FinRoute[WayPointIndex].position, CurPosition) == 0f)
                        WayPointIndex--;

                }
                else
                {
                    CustomerMove(StartPoint);

                    if (Vector3.Distance(StartPoint.position, CurPosition) == 0f)
                    {
                        ObjectPool.ReturnObject(this);
                        isInitialize = true;
                    }
                }
            }
        }
        //---�ڸ��� ���� �� �׳� ���ư�---//
        else if (isFull && !isArrive)
        {
            CustomerMove(StopPoint);

            //---���� point�� �Ÿ��� 0�� �ƴ϶�� ---//
            if (Vector3.Distance(StopPoint.position, CurPosition) == 0f)
            {
                isArrive = true;
                animator.SetBool("isStop", true);
                GameObject.Instantiate(speech_Box_Full, GameObject.Find("Speech_Boxes").transform);
                Invoke("JustOut", 2f);
            }
        }

        if (isJustreturn)
        {
            CustomerMove(StartPoint);

            if (Vector3.Distance(StartPoint.position, CurPosition) == 0f)
            {
                ObjectPool.ReturnObject(this);
                isInitialize = true;
            }

        }
    }

    //---���� �ʱ�ȭ---//
    void Initialize()
    {
        WayPointIndex = 0;

        SetNewSeat();

        for (int i = 0; i < FinRoute.Count; i++)
        {
            FinRoute[i] = RouteList[seatIndex][i];
        }

        //�⺻ ���� �ʱ�ȭ for ������Ʈ Ǯ��
        isInitialize = false;
        isArrive = false;
        isReturn = false;
        isJustreturn = false;
        orderMenu.isEat = false;
    }

    //---�մ� �⺻ ������---//
    void CustomerMove(Transform TargetTransform)
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(CurPosition, TargetTransform.position, step);
    }

    //---������ �� �԰� ������ ���ư���---//
    public void EatAndLeave()
    {
        //���� ����
        audioSource.Stop();

        //�Ͼ�� �ִϸ��̼� �߰�
        animator.SetBool("isEat", false);
        animator.SetBool("isSit", false);
        animator.SetFloat("dirX", -currentDir.x);

        isReturn = true;
        data.isAllocated[seatIndex] = false;
        WayPointIndex--;

        data.curSeatSize--;

        data.onTables[seatIndex] = false;
        data.isFinEat[seatIndex] = true;

        data.isCustomer[seatIndex] = false; // �մ� ���̺��� ������ �� üũ
    }

    //---�ڸ��� ��� �׳� ������ ���ư���---//
    void JustOut()
    {
        animator.SetBool("isStop", false);
        Destroy(GameObject.Find("UI_Speech_Box_Full(Clone)"));
        isJustreturn = true;
    }

    //---���� �ڸ� ����---//
    void SetNewSeat()
    {
        int count = 0; //�ݺ� Ƚ���� ���� ����

        //�ڸ��� ����á���� �˻�
        for (int i = 0; i < data.maxSeatSize; i++)
        {
            if (data.isAllocated[i])
                count++;
        }

        //�ڸ���ŭ �ݺ��� ���� ��� isFull = true
        if (count == data.maxSeatSize)
        {
            isFull = true;
            return;
        }
        else isFull = false;

        //�������� �ڸ� ����
        int randomSeat = Random.Range(0, data.maxSeatSize);

        //�Ҵ��� �ȵǾ� �ִ� �ڸ��� ã�� �� ���� �ݺ�
        while (data.isAllocated[randomSeat])
        {
            randomSeat = Random.Range(0, data.maxSeatSize);
        }

        //�ڸ��� ã�Ҵٸ� �Ҵ�
        seatIndex = randomSeat;
        data.isAllocated[seatIndex] = true;
    }

    //--- Ÿ�� �ƿ� ---//
    public void TimeOut()
    {
        isReturn = true;
        data.curSeatSize--;
        data.isAllocated[seatIndex] = false;
        WayPointIndex--;

        animator.SetBool("isSit", false);
        animator.SetFloat("dirX", currentDir.x * -1);

        data.isCustomer[seatIndex] = false; // �մ� ���̺��� ������ �� üũ
    }
    
    //---���� �մ� ������Ʈ�� ���� ����---//
    void SetDirection()
    {
        //--- ���� ��ġ�� ---//
        CurPosition = transform.position;

        currentDir = (CurPosition - lastPosition).normalized;

        if (currentDir != Vector3.zero)
        {
            animator.SetFloat("dirX", currentDir.x);
            animator.SetFloat("dirY", currentDir.y);
            lastPosition = CurPosition;
        }
        else animator.SetFloat("dirY", -1f);

        //---������ ����---//
        if (CurPosition.y < GameObject.Find("Chr_Player").transform.position.y) //�մ��� �Ʒ��� �ִٸ�
            spriteRenderer.sortingLayerName = "Customer_Up"; //�÷��̾�� ���� ������
        else
            spriteRenderer.sortingLayerName = "Player";
    }

    //---������ �Ա� �������� ��--//
    public void EatFood()
    {
        if (data.menuCategories[seatIndex].Contains("Gukbab") || data.menuCategories[seatIndex].Contains("Pajeon"))
            audioSource.clip = audioClips[0];
        else if (data.menuCategories[seatIndex].Equals("RiceJuice"))
            audioSource.clip = audioClips[1];

        audioSource.loop = true;
        audioSource.Play();

        animator.SetBool("isEat", true);
        Invoke("EatAndLeave", 3f);
    }
}
