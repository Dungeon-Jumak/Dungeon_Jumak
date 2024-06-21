using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class OrderSystem : MonoBehaviour
{
    //Tables Object
    [SerializeField] private Transform[] tables;

    private Data data;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void Start()
    {
        //--- Get Componet ---//
        data = DataManager.Instance.data;

        //--- Initialize data's Variables ---//
        //--- 주문 전표 출력을 위한 카운팅 변수 -> 씬이 시작 될때마다 초기화 되도록 해당 스크립트에서 초기화 함 ---//
        data.gukbapCount= 0;
        data.pajeonCount= 0;
        data.riceJuiceCount= 0;
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void Update()
    {
        //--- 모든 테이블 순회 ---//
        for (int i = 0; i < tables.Length; i++)
        {
            //--- 다 먹은 테이블 감지 ---//
            if (data.isFinEat[i])
            {
                //--- 중복 실행 방지를 위한 bool 변수 변환 ---//
                data.isFinEat[i] = false;

                //---이중 파괴 오류 수정---//
                if (tables[i].transform.GetChild(0).childCount != 0)
                    Destroy(tables[i].transform.GetChild(0).GetChild(0).gameObject);

                //--- 다 먹은 테이블의 경우 카테고리와 벨류에 맞게 코인 지급 ---//
                CoinUpdate(i);
            }

        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    //--- 음식의 카테고리와 벨류에 따라 코인을 달리 얻기 위한 메소드 ---//
    private void CoinUpdate(int idx)
    {
        GameManager.Sound.Play("[S] Get Coin", Define.Sound.Effect, false);

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