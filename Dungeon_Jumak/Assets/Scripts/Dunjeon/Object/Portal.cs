//System
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;

[DisallowMultipleComponent]
public class Portal : MonoBehaviour
{
    [Header("포탈 회전 속도(외관용)")]
    [SerializeField] private float speed;

    private void Update()
    {
        transform.Rotate(Vector3.back * speed * Time.deltaTime);
    }

}
