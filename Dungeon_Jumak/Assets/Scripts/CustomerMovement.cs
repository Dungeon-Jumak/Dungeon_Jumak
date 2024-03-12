using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerMovement : MonoBehaviour
{
    public Transform tempseat;

    NavMeshAgent agent;

    [SerializeField]
    private bool allocateSeat = false; //손님 자리 할당 변수

    [SerializeField]
    private Transform SelectedSeat; //선택된 자리 변수

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Update()
    {
        // --- 자리 할당이 안되었을 경우 --- //
        /*if (!allocateSeat)
        {
            allocateSeat = true;
            SelectSeat();
        }*/
        // --- 에이전트 SetDestination --- //
        agent.SetDestination(tempseat.position);
    }

    /*
    // --- 자리 지정 함수 --- //
    private void SelectSeat()
    {
        for (int i = 0; i < data.seats.Length; i++)
        {
            if (!data.isAllocated[i])
            {
                SelectedSeat = data.seats[i];
                data.isAllocated[i] = true;
            }
        }
        // --- 에이전트 SetDestination --- //
        agent.SetDestination(SelectedSeat.position);
    }
    */
}
