using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CominScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        //---Define.cs enum ���� ���� Scene ����---//
        SceneType = Define.Scene.ComingSoon;
    }

    public override void Clear()
    {

    }

    //---UI �ڵ�ȭ ���̶� ���Ƿ� �������� �Լ�����---//
    public void cangeToJumakScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Jumak");
    }
}
