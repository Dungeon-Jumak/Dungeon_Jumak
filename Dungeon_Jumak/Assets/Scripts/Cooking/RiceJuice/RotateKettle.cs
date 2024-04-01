using UnityEngine;

public class RotateKettle : MonoBehaviour
{
    public float minRotation = -80f; // 최소 회전 각도
    public float maxRotation = 105f; // 최대 회전 각도
    public float rotationDuration = 3f; // 회전에 걸리는 시간

    private float currentRotation; // 현재 회전 각도
    private bool rotatingForward = true; // 현재 회전 방향

    void Start()
    {
        currentRotation = minRotation; // 초기 회전 각도 설정
    }

    void Update()
    {
        // 회전 방향에 따라 각도를 조절하고 시간에 따라 회전량을 계산
        float rotationAmount = (maxRotation - minRotation) / rotationDuration * Time.deltaTime;

        // 현재 회전 방향에 따라 회전량 적용
        if (rotatingForward)
        {
            currentRotation += rotationAmount;
            // 최대 각도에 도달하면 회전 방향 변경
            if (currentRotation >= maxRotation)
            {
                currentRotation = maxRotation;
                rotatingForward = false;
            }
        }
        else
        {
            currentRotation -= rotationAmount;
            // 최소 각도에 도달하면 회전 방향 변경
            if (currentRotation <= minRotation)
            {
                currentRotation = minRotation;
                rotatingForward = true;
            }
        }

        // 회전 적용
        transform.rotation = Quaternion.Euler(0f, 0f, currentRotation);
    }
}
