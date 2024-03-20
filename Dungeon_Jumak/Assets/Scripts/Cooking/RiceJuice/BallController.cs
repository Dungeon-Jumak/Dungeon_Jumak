using UnityEngine;

public class BallController : MonoBehaviour
{
    public GameObject InputObject; // �̵� ������ ��Ÿ���� ������Ʈ
    public float moveSpeed = 10f; // ���� �̵� �ӵ�

    private bool isMovingLeft = false; // �������� �̵� ������ ���θ� ��Ÿ���� �÷���
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
        // ���콺 ���� ��ư�� ������ ��
        if (Input.GetMouseButton(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            isMovingLeft = true;
        }
        else
        {
            isMovingLeft = false;
            // ��Ŭ���� �������� �� ���������� �̵�
            MoveBallRight();
        }

        // ���� �������� �̵� ���̶��
        if (isMovingLeft)
        {
            MoveBallLeft();
        }

        // ���� ��ġ�� ����
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, inputLeftEdge, inputRightEdge), transform.position.y, transform.position.z);
    }

    // ���� �������� �̵���Ű�� �Լ�
    void MoveBallLeft()
    {
        // ���� ���� ��ġ���� moveSpeed��ŭ �������� �̵�
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;
    }

    // ���� ���������� �̵���Ű�� �Լ�
    void MoveBallRight()
    {
        // ���� ���� ��ġ���� moveSpeed��ŭ ���������� �̵�
        transform.position += Vector3.right * moveSpeed * Time.deltaTime;
    }
}
