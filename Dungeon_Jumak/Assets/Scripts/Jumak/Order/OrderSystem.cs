using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderSystem : MonoBehaviour
{
    [SerializeField]
    private Transform[] tables;
    [SerializeField]
    private Data data;
    [SerializeField]
    private AudioManager audioManager;
    [SerializeField]
    private string coinSound;

    private void Start()
    {
        data = DataManager.Instance.data;
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Update()
    {
        for (int i = 0; i < tables.Length; i++)
        {
            //---�� �Ծ����� table�� �ʱ�ȭ---//
            if (data.isFinEat[i])
            {
                data.isFinEat[i] = false;

                //---���� �ı� ���� ����---//
                if (tables[i].transform.GetChild(0).childCount != 0)
                    Destroy(tables[i].transform.GetChild(0).GetChild(0).gameObject);

                CoinUpdate(i);//���� ����
                audioManager.Play(coinSound);
            }

        }
    }

    private void CoinUpdate(int idx)
    {
        switch (data.menuCategories[idx])
        {
            case "Gukbab":
                switch (data.menuLV[idx])
                {
                    case 1:
                        data.curCoin += 5;
                        break;
                    case 2:
                        data.curCoin += 10;
                        break;
                    case 3:
                        data.curCoin += 15;
                        break;
                    case 4:
                        data.curCoin += 8;
                        break;
                    case 5:
                        data.curCoin += 15;
                        break;
                    case 6:
                        data.curCoin += 20;
                        break;
                    default:
                        break;
                }
                break;

            case "Pajeon":
                switch (data.menuLV[idx])
                {
                    case 1:
                        data.curCoin += 7;
                        break;
                    case 2:
                        data.curCoin += 9;
                        break;
                    case 3:
                        data.curCoin += 10;
                        break;
                    case 4:
                        data.curCoin += 13;
                        break;
                    case 5:
                        data.curCoin += 15;
                        break;
                    case 6:
                        data.curCoin += 13;
                        break;
                    case 7:
                        data.curCoin += 15;
                        break;
                    default:
                        break;
                }
                break;

            case "RiceJuice":
                data.curCoin += 4;
                break;

            default:
                break;
        }



    }


}
