using System;

[Serializable]
public class Data
{
    //������ ����
    //���� ���࿡ �־� ���������� �ʿ��� �����ʹ� Data.cs�� public���� �����ڷ� �����μ� �� ��
    //ex) ����, ���� �� ���
    //�����Ϳ� �ִ� ���� �ٸ� ��ũ��Ʈ�� ����ϱ� ���ؼ��� �̱������μ� ����ϸ� ��
    //ex) Data data = DataManager.Instance.data; => �̸� ���� Data.cs�� �ִ� �������� ����� �� ����

    public int curPlayerLV = 1;
    public int maxPlayerLV;

    //---���� ����---//
    public bool isPlayBGM = true;
    public bool isSound = true;
    public bool isPause = false;

    //---CustomerSystem---//
    public int maxSeatSize = 2;
    public int curSeatSize = 0;

    public bool[] isAllocated = new bool[12];
    public bool[] isCustomer = new bool[12];            //�� ���̺� �����ߴ��� üũ�ϱ� ���� ����
    public bool[] onTables = new bool[12];              //���̺� ���� ������ üũ�ϱ� ���� ����
    public bool[] isFinEat = new bool[12];              //�� �Ծ����� �˸��� ����

    public string[] menuCategories = new string[12]; //�� �ڸ��� �ִ� �޴��� ī�װ�
    public int[] menuLV = new int[12];              //�� ���̺� �ִ� �޴��� ����
    public int[] ingredient = new int[5];           //0: �������, 1: ����, 2: �ᳪ��, 3: ��¡��, 4: �Ұ��

    public int curCoin = 0;
    public int maxCoin = 999999;

    //---�ڸ� �ر� ����---//
    public int curUnlockLevel = 1;
    public int maxUnlockLevel = 6;

    //---�޴� �ر� ����---//
    public int curMenuUnlockLevel = 1;
    public int maxMenuUnlockLevel = 3;

    //---�̴ϰ��� ����---//
    public float fireSize = 100f;
    public int tableIndex = 0;
    public bool riceJuiceClear = false; //���� �̴ϰ��� ���� ����

    //---���� ����---//
    public bool isMonster = false;//Monster spawn ���� Ȯ��
    public bool isObstacle = false;//Obstacle spawn ���� Ȯ��
    public float runningTime = 0;//�޸��� ���� ���� �ð�
    public float playerHP = 3;//HP - ��Ʈ 
    public float monsterHP; // ����HP - ��Ʈ 

}
