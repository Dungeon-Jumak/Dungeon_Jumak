using UnityEngine;

public class BallController : MonoBehaviour
{
    public GameObject InputObject; // 이동 범위를 나타내는 오브젝트
    public float moveSpeed = 10f; // 공의 이동 속도

    private bool isMovingLeft = false; // 왼쪽으로 이동 중인지 여부를 나타내는 플래그
    private float inputLeftEdge; 
    private float inputRightEdge; 

    void Start()
    {
        float inputObjectWidth = InputObject.transform.localScale.x;
        inputLeftEdge = InputObject.transform.position.x - inputObjectWidth / 2 + 0.5f;
        inputRightEdge = InputObject.transform.position.x + inputObjectWidth / 2 - 0.5f;
    }

    void Update()
    {
        // 마우스 왼쪽 버튼이 눌렸을 때
        if (Input.GetMouseButton(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            isMovingLeft = true;
        }
        else
        {
            isMovingLeft = false;
            // 좌클릭이 떨어졌을 때 오른쪽으로 이동
            MoveBallRight();
        }

        // 공이 왼쪽으로 이동 중이라면
        if (isMovingLeft)
        {
            MoveBallLeft();
        }

        // 공의 위치를 제한
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, inputLeftEdge, inputRightEdge), transform.position.y, transform.position.z);
    }

    // 공을 왼쪽으로 이동시키는 함수
    void MoveBallLeft()
    {
        // 현재 공의 위치에서 moveSpeed만큼 왼쪽으로 이동
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;
    }

    // 공을 오른쪽으로 이동시키는 함수
    void MoveBallRight()
    {
        // 현재 공의 위치에서 moveSpeed만큼 오른쪽으로 이동
        transform.position += Vector3.right * moveSpeed * Time.deltaTime;
    }
}
