using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    public float speed = 1.5f; // ��ֹ��� �������� �ӵ�

    void Update()
    {
        // ��ֹ��� �Ʒ��� �̵�
        transform.position += Vector3.left * speed * Time.deltaTime;

        // ��ֹ��� ȭ�� ������ �������� Ȯ���ϰ�, �������� ����
        if (transform.position.y < -6f) // ȭ�� �ϴ� ��� ����
        {
            Destroy(gameObject); // ���� ���� ������Ʈ(��ֹ�) ����
        }
    }
}
