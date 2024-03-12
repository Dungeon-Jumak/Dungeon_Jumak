using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerMovement : MonoBehaviour
{
    public Transform tempseat;

    NavMeshAgent agent;

    [SerializeField]
    private bool allocateSeat = false; //�մ� �ڸ� �Ҵ� ����

    [SerializeField]
    private Transform SelectedSeat; //���õ� �ڸ� ����

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Update()
    {
        // --- �ڸ� �Ҵ��� �ȵǾ��� ��� --- //
        /*if (!allocateSeat)
        {
            allocateSeat = true;
            SelectSeat();
        }*/
        // --- ������Ʈ SetDestination --- //
        agent.SetDestination(tempseat.position);
    }

    /*
    // --- �ڸ� ���� �Լ� --- //
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
        // --- ������Ʈ SetDestination --- //
        agent.SetDestination(SelectedSeat.position);
    }
    */
}
