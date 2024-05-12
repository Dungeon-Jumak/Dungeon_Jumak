using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ComingScene : BaseScene
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
        GameManager.Scene.LoadScene(Define.Scene.Jumak);
    }
}
