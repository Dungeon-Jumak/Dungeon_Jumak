//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;

[DisallowMultipleComponent]
public class Portal : MonoBehaviour
{
    [Header("��Ż ȸ�� �ӵ�(�ܰ���)")]
    [SerializeField] private float speed;

    private void Update()
    {
        transform.Rotate(Vector3.back * speed * Time.deltaTime);
    }

}
