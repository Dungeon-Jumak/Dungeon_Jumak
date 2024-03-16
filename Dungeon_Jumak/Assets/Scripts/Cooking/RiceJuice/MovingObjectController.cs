using UnityEngine;

public class MovingObjectController : MonoBehaviour
{
    public GameObject InputObject; // 이동 범위를 나타내는 오브젝트
    public float moveSpeed = 3f; // 오브젝트의 이동 속도
    public float stopTime = 1f; // 멈춰있는 시간

    private bool isMoving = false; // 이동 중인지 여부를 나타내는 플래그
    private float targetX; // 목표 X 좌표
    private float inputLeftEdge; // InputObject의 왼쪽 끝 x값
    private float inputRightEdge; // InputObject의 오른쪽 끝 x값
    private Vector3 startPosition; // 시작 위치
    private float startTime; // 이동 시작 시간

    void Start()
    {
        // InputObject의 왼쪽 끝과 오른쪽 끝을 계산
        float inputObjectWidth = InputObject.transform.localScale.x;
        inputLeftEdge = InputObject.transform.position.x - inputObjectWidth / 2 + 1f; ;
        inputRightEdge = InputObject.transform.position.x + inputObjectWidth / 2 - 1f; ;

        // 시작 위치 설정
        startPosition = transform.position;

        // 처음 이동 시작
        StartMoving();
    }

    void Update()
    {
        // 이동 중이라면
        if (isMoving)
        {
            // 현재 시간과 이동 시작 시간 간의 차이 계산
            float journeyLength = Mathf.Abs(targetX - startPosition.x);
            float distCovered = (Time.time - startTime) * moveSpeed;
            float fracJourney = distCovered / journeyLength;

            // 목표 위치로 이동
            transform.position = Vector3.Lerp(startPosition, new Vector3(targetX, transform.position.y, transform.position.z), fracJourney);

            // 목표 위치에 도달하면 멈추기
            if (fracJourney >= 1f)
            {
                isMoving = false;
                Invoke("StartMoving", stopTime); // 멈춘 후 다시 이동 시작
            }
        }
    }

    // 이동 시작 함수
    void StartMoving()
    {
        // 랜덤한 위치 설정
        targetX = Random.Range(inputLeftEdge, inputRightEdge);

        // 이동 시작
        isMoving = true;
        startTime = Time.time;
        startPosition = transform.position;
    }
}
