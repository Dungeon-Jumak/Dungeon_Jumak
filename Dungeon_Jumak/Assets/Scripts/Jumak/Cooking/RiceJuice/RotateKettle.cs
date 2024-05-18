using UnityEngine;

public class RotateKettle : MonoBehaviour
{
    [SerializeField] private float minRotation = 80f;
    [SerializeField] private float maxRotation = 180f;
    [SerializeField] private float rotationDuration = 3f; //===ȸ���� �ɸ��� �ð�===//

    private float currentRotation; //===���� ȸ�� ����===//
    private bool rotatingForward = true; //===���� ȸ�� ����===//

    void Start()
    {
        currentRotation = minRotation;
    }

    void Update()
    {
        float rotationAmount = (maxRotation - minRotation) / rotationDuration * Time.deltaTime;

        if (rotatingForward)
        {
            currentRotation += rotationAmount;

            //===�ִ� ������ �����ϸ� ȸ�� ���� ����===//
            if (currentRotation >= maxRotation)
            {
                currentRotation = maxRotation;
                rotatingForward = false;
            }
        }
        else
        {
            currentRotation -= rotationAmount;

            //===�ּ� ������ �����ϸ� ȸ�� ���� ����===//
            if (currentRotation <= minRotation)
            {
                currentRotation = minRotation;
                rotatingForward = true;
            }
        }

        //===ȸ�� ����===//
        transform.rotation = Quaternion.Euler(0f, 0f, currentRotation);
    }
}
