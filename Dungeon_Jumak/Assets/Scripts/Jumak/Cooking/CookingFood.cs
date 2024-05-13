using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� ���� �� ������ ������ �����ϰ� �����ϱ� ���� ��ũ��Ʈ
/// </summary>
public class CookingFood : MonoBehaviour
{
    public string category;    // ������ �޴� ī�װ�

    //�޴� �˾�
    [SerializeField]
    private GameObject gukbabPopup;
    [SerializeField]
    private GameObject pajeonPopup;
    [SerializeField]
    private GameObject riceJuicePopup;

    private void Start()
    {
        category = "";
    }

    public void ChooseGukbab()
    {
        category = "Gukbab";

        //���� �˾� ���� ������ Setactive = false
        gukbabPopup.SetActive(true);
        pajeonPopup.SetActive(false);
        riceJuicePopup.SetActive(false);
    }

    public void ChoosePajeon()
    {
        category = "Pajeon";

        //���� �˾� ���� ������ Setactive = false
        gukbabPopup.SetActive(false);
        pajeonPopup.SetActive(true);
        riceJuicePopup.SetActive(false);
    }

    public void ChooseRiceJuice()
    {
        category = "RiceJuice";

        //���� �˾� ���� ������ Setactive = false
        gukbabPopup.SetActive(false);
        pajeonPopup.SetActive(false);
        riceJuicePopup.SetActive(true);
    }

    
}
