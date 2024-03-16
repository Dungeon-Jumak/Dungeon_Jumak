using UnityEngine;
using UnityEngine.UI;

public class CollisionController : MonoBehaviour
{
    public GameObject squareObject; // �׸� ������Ʈ
    public GameObject ballObject; // Ball ������Ʈ
    public Text waterText; // Water ������ ǥ���� UI �ؽ�Ʈ
    public Text riceText; // Rice ������ ǥ���� UI �ؽ�Ʈ
    public int waterIncreaseAmount = 5; // �浹 �� water ���� ������
    public int riceIncreaseAmount = 5; // ���浹 �� rice ���� ������
    public float increaseDuration = 2f; // ���� ���� �ð�

    private int water = 0; // Water ����
    private int rice = 0; // Rice ����
    private bool alreadyReachedMax = false; // �̹� �ִ밪�� �����ߴ��� ����
    private float lastWaterIncreaseTime; // ������ water ���� �ð�
    private float lastRiceIncreaseTime; // ������ rice ���� �ð�

    void Update()
    {
        // �̹� �ִ밪�� �����ߴٸ� �� �̻� ������ ������Ű�� ����
        if (alreadyReachedMax)
            return;

        // �� 100�� ���� �������� �� �� �̻� ������ ������Ű�� �ʵ��� ��
        if (water + rice >= 100)
        {
            alreadyReachedMax = true;
            return;
        }

        // Ball ������Ʈ�� x ��ǥ
        float ballX = ballObject.transform.position.x;
        // �׸� ������Ʈ�� x ��ǥ ������ ���۰�
        float squareXStart = squareObject.transform.position.x - squareObject.transform.localScale.x / 2;
        float squareXEnd = squareObject.transform.position.x + squareObject.transform.localScale.x / 2;

        // Ball ������Ʈ�� �׸� ������Ʈ �ȿ� �ִ��� Ȯ��
        bool isInside = ballX >= squareXStart && ballX <= squareXEnd;

        // Ball ������Ʈ�� �׸� ������Ʈ �ȿ� �ִ� ���
        if (isInside)
        {
            // 2�ʿ� 1�� water ���� ����
            if (Time.time - lastWaterIncreaseTime >= increaseDuration)
            {
                water += waterIncreaseAmount;
                waterText.text = "Water: " + water.ToString();
                lastWaterIncreaseTime = Time.time;
            }
        }
        // Ball ������Ʈ�� �׸� ������Ʈ �ȿ� ���� ���
        else
        {
            // 2�ʿ� 1�� rice ���� ����
            if (Time.time - lastRiceIncreaseTime >= increaseDuration)
            {
                rice += riceIncreaseAmount;
                riceText.text = "Rice: " + rice.ToString();
                lastRiceIncreaseTime = Time.time;
            }
        }
    }
}
