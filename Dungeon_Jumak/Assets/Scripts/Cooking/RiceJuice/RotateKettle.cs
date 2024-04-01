using UnityEngine;

public class RotateKettle : MonoBehaviour
{
    public float minRotation = -80f; // �ּ� ȸ�� ����
    public float maxRotation = 105f; // �ִ� ȸ�� ����
    public float rotationDuration = 3f; // ȸ���� �ɸ��� �ð�

    private float currentRotation; // ���� ȸ�� ����
    private bool rotatingForward = true; // ���� ȸ�� ����

    void Start()
    {
        currentRotation = minRotation; // �ʱ� ȸ�� ���� ����
    }

    void Update()
    {
        // ȸ�� ���⿡ ���� ������ �����ϰ� �ð��� ���� ȸ������ ���
        float rotationAmount = (maxRotation - minRotation) / rotationDuration * Time.deltaTime;

        // ���� ȸ�� ���⿡ ���� ȸ���� ����
        if (rotatingForward)
        {
            currentRotation += rotationAmount;
            // �ִ� ������ �����ϸ� ȸ�� ���� ����
            if (currentRotation >= maxRotation)
            {
                currentRotation = maxRotation;
                rotatingForward = false;
            }
        }
        else
        {
            currentRotation -= rotationAmount;
            // �ּ� ������ �����ϸ� ȸ�� ���� ����
            if (currentRotation <= minRotation)
            {
                currentRotation = minRotation;
                rotatingForward = true;
            }
        }

        // ȸ�� ����
        transform.rotation = Quaternion.Euler(0f, 0f, currentRotation);
    }
}
