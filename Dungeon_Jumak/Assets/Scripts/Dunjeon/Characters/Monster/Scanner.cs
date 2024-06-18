//System
using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;

//Unity
using UnityEngine;

[DisallowMultipleComponent]
public class Scanner : MonoBehaviour
{
    [Header("��ĵ ����")]
    [SerializeField] private float scanRange;

    [Header("���̾� ����ũ")]
    [SerializeField] private LayerMask targetLayer;

    [Header("���� ĳ��Ʈ �� �迭")]
    [SerializeField] private RaycastHit2D[] targets;

    [Header("���� ����� Ÿ��")]
    [SerializeField] private Transform nearestTarget;

    private void FixedUpdate()
    {
        //Set Targets
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);

        //Search nearest target
        nearestTarget = GetNearestTarget();
    }

    //Search nearest target
    private Transform GetNearestTarget()
    {
        Transform result = null;
        float lastDistance = 100f;

        foreach (RaycastHit2D target in targets)
        {
            //Player's pos
            Vector3 playerPos = transform.position;

            //Target's pos
            Vector3 targetPos = target.transform.position;

            //Compute Distance
            float curDistance = Vector3.Distance(playerPos, targetPos);

            //Update last distance
            if (curDistance < lastDistance)
            {
                lastDistance = curDistance;
                result = target.transform;
            }
        }

        //return nearest transform
        return result;
    }
}
