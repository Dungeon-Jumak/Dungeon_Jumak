//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;

[DisallowMultipleComponent]
public class PortalRotation : MonoBehaviour
{
    [Header("��Ż ȸ�� �ӵ�(�ܰ���)")]
    public float speed;

    private void Update()
    {
        transform.Rotate(Vector3.back * speed * Time.deltaTime);
    }
}
