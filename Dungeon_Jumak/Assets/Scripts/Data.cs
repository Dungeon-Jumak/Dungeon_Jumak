using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class Data
{
    //������ ����
    //���� ���࿡ �־� ���������� �ʿ��� �����ʹ� Data.cs�� public���� �����ڷ� �����μ� �� ��
    //ex) ����, ���� �� ���
    //�����Ϳ� �ִ� ���� �ٸ� ��ũ��Ʈ�� ����ϱ� ���ؼ��� �̱������μ� ����ϸ� ��
    //ex) Data data = DataManager.Instance.data; => �̸� ���� Data.cs�� �ִ� �������� ����� �� ����


    // --- �մ� �̵� ���� ������ --- //
    public Transform[] seats;
    public bool[] isAllocated; //�ڸ� �Ҵ� ����
}
