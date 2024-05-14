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
        data.gukbapCount= 0;
        data.pajeonCount= 0;
        data.riceJuiceCount= 0;

        data = DataManager.Instance.data;
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Update()
    {
        for (int i = 0; i < tables.Length; i++)
        {
            //---다 먹었으면 table을 초기화---//
            if (data.isFinEat[i])
            {
                data.isFinEat[i] = false;

                //---이중 파괴 오류 수정---//
                if (tables[i].transform.GetChild(0).childCount != 0)
                    Destroy(tables[i].transform.GetChild(0).GetChild(0).gameObject);

                CoinUpdate(i);//코인 지급
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
                        data.gukbapCount++;
                        data.nowGukbapPrice = 5;
                        break;
                    case 2:
                        data.curCoin += 10;
                        data.gukbapCount++;
                        data.nowGukbapPrice = 10;
                        break;
                    case 3:
                        data.curCoin += 15;
                        data.gukbapCount++;
                        data.nowGukbapPrice = 15;
                        break;
                    case 4:
                        data.curCoin += 8;
                        data.gukbapCount++;
                        data.nowGukbapPrice = 8;
                        break;
                    case 5:
                        data.curCoin += 15;
                        data.gukbapCount++;
                        data.nowGukbapPrice = 15;
                        break;
                    case 6:
                        data.curCoin += 20;
                        data.gukbapCount++;
                        data.nowGukbapPrice = 20;
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
                        data.pajeonCount++;
                        data.nowPajeonPrice = 7;
                        break;
                    case 2:
                        data.curCoin += 9;
                        data.pajeonCount++;
                        data.nowPajeonPrice = 9;
                        break;
                    case 3:
                        data.curCoin += 10;
                        data.pajeonCount++;
                        data.nowPajeonPrice = 10;
                        break;
                    case 4:
                        data.curCoin += 13;
                        data.pajeonCount++;
                        data.nowPajeonPrice = 13;
                        break;
                    case 5:
                        data.curCoin += 15;
                        data.pajeonCount++;
                        data.nowPajeonPrice = 15;
                        break;
                    case 6:
                        data.curCoin += 13;
                        data.pajeonCount++;
                        data.nowPajeonPrice = 13;
                        break;
                    case 7:
                        data.curCoin += 15;
                        data.pajeonCount++;
                        data.nowPajeonPrice = 15;
                        break;
                    default:
                        break;
                }
                break;

            case "RiceJuice":
                data.curCoin += 4;
                data.riceJuiceCount++;
                data.nowRiceJuicePrice = 4;
                break;

            default:
                break;
        }



    }


}