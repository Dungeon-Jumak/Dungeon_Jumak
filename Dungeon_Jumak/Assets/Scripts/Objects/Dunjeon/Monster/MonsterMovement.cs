using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    public float speed = 2.5f; // ��ֹ��� �������� �ӵ�

    void Update()
    {
        // ��ֹ��� �Ʒ��� �̵�
        transform.position += Vector3.down * speed * Time.deltaTime;

        // ��ֹ��� ȭ�� ������ �������� Ȯ���ϰ�, �������� ����
        if (transform.position.y < -6f) // ȭ�� �ϴ� ��� ����
        {
            Destroy(gameObject); // ���� ���� ������Ʈ(��ֹ�) ����
        }
    }
}