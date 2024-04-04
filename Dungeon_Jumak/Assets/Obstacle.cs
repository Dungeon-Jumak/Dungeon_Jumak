using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float speed = 0.6f; // 장애물이 내려오는 속도

    void Update()
    {
        // 장애물을 아래로 이동
        transform.position += Vector3.down * speed * Time.deltaTime;

        // 장애물이 화면 밖으로 나갔는지 확인하고, 나갔으면 삭제
        if (transform.position.y < -6f) // 화면 하단 경계 설정
        {
            Destroy(gameObject); // 현재 게임 오브젝트(장애물) 삭제
        }
    }
}
