using System;

[Serializable]
public class Data
{
    //������ ����
    //���� ���࿡ �־� ���������� �ʿ��� �����ʹ� Data.cs�� public���� �����ڷ� �����μ� �� ��
    //ex) ����, ���� �� ���
    //�����Ϳ� �ִ� ���� �ٸ� ��ũ��Ʈ�� ����ϱ� ���ؼ��� �̱������μ� ����ϸ� ��
    //ex) Data data = DataManager.Instance.data; => �̸� ���� Data.cs�� �ִ� �������� ����� �� ����

    //---CustomerSystem---//
    public int maxSeatSize = 2;
    public int curSeatSize = 0;
    public bool[] isAllocated = new bool[12];
    public bool[] isCustomer = new bool[12]; // �� ���̺� �����ߴ��� üũ�ϱ� ���� ����

    //---�ڸ� �ر� ����---//
    public int curUnlockLevel = 1;
    public int maxUnlockLevel = 6;

    //---�޴� �ر� ����---//
    public int curMenuUnlockLevel = 1;
    public int maxMenuUnlockLevel = 3;

    //---���̺� �迭---//
    public bool[] onTables = new bool[12]; //���̺� ���� ������ üũ�ϱ� ���� ����
    public bool[] isFinEat = new bool[12]; //�� �Ծ����� �˸��� ����

    //---�޴� ����---//
    public int[] menuNums = new int[12];
}
