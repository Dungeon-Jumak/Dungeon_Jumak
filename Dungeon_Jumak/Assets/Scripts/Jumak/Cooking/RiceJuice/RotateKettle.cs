using UnityEngine;

public class RotateKettle : MonoBehaviour
{
    [SerializeField] private float minRotation = 80f;
    [SerializeField] private float maxRotation = 180f;
    [SerializeField] private float rotationDuration = 3f; //===회전에 걸리는 시간===//

    private float currentRotation; //===현재 회전 각도===//
    private bool rotatingForward = true; //===현재 회전 방향===//

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

            //===최대 각도에 도달하면 회전 방향 변경===//
            if (currentRotation >= maxRotation)
            {
                currentRotation = maxRotation;
                rotatingForward = false;
            }
        }
        else
        {
            currentRotation -= rotationAmount;

            //===최소 각도에 도달하면 회전 방향 변경===//
            if (currentRotation <= minRotation)
            {
                currentRotation = minRotation;
                rotatingForward = true;
            }
        }

        //===회전 적용===//
        transform.rotation = Quaternion.Euler(0f, 0f, currentRotation);
    }
}
