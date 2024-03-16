using UnityEngine;

public class MovingObjectController : MonoBehaviour
{
    public GameObject InputObject; // �̵� ������ ��Ÿ���� ������Ʈ
    public float moveSpeed = 3f; // ������Ʈ�� �̵� �ӵ�
    public float stopTime = 1f; // �����ִ� �ð�

    private bool isMoving = false; // �̵� ������ ���θ� ��Ÿ���� �÷���
    private float targetX; // ��ǥ X ��ǥ
    private float inputLeftEdge; // InputObject�� ���� �� x��
    private float inputRightEdge; // InputObject�� ������ �� x��
    private Vector3 startPosition; // ���� ��ġ
    private float startTime; // �̵� ���� �ð�

    void Start()
    {
        // InputObject�� ���� ���� ������ ���� ���
        float inputObjectWidth = InputObject.transform.localScale.x;
        inputLeftEdge = InputObject.transform.position.x - inputObjectWidth / 2 + 1f; ;
        inputRightEdge = InputObject.transform.position.x + inputObjectWidth / 2 - 1f; ;

        // ���� ��ġ ����
        startPosition = transform.position;

        // ó�� �̵� ����
        StartMoving();
    }

    void Update()
    {
        // �̵� ���̶��
        if (isMoving)
        {
            // ���� �ð��� �̵� ���� �ð� ���� ���� ���
            float journeyLength = Mathf.Abs(targetX - startPosition.x);
            float distCovered = (Time.time - startTime) * moveSpeed;
            float fracJourney = distCovered / journeyLength;

            // ��ǥ ��ġ�� �̵�
            transform.position = Vector3.Lerp(startPosition, new Vector3(targetX, transform.position.y, transform.position.z), fracJourney);

            // ��ǥ ��ġ�� �����ϸ� ���߱�
            if (fracJourney >= 1f)
            {
                isMoving = false;
                Invoke("StartMoving", stopTime); // ���� �� �ٽ� �̵� ����
            }
        }
    }

    // �̵� ���� �Լ�
    void StartMoving()
    {
        // ������ ��ġ ����
        targetX = Random.Range(inputLeftEdge, inputRightEdge);

        // �̵� ����
        isMoving = true;
        startTime = Time.time;
        startPosition = transform.position;
    }
}
