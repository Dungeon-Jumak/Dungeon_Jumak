using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Define : MonoBehaviour
{
    //---summary---//

    // Define���� �̺�Ʈ Scene�� ���� ������ ���� ���������� ������ ��!
    //Scene ���� ����� ���⿡ �̸� �߰����ֱ�. �׷��� SceneManager���� ��� ����
    // �� ��ȯ�� �� GameManager.Scene.LoadScene(Define.Scene.���ϴ¾��̸�); �ڵ� �߰��ϸ� �� -> SceneManagement �Ⱥҷ��͵� ��


    public enum Scene
    {
        Jumak,
        StartScene,
        WaitingScene,
        Map,
        MainDunjeon,
        Market,
    }

    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount,//��ݰ� ȿ�������� ��� ����
    }

    //---UI Ŭ���� �巡�׸� �����ϱ� ���� Define.cs UIEvent enum�� �߰�---//
    public enum UIEvent
    {
        Click,
        Drag,
    }

    //---���콺 �̺�Ʈ�� �����ϱ� ���� Define.cs MouseEvent enum�� �߰�---//
    public enum MouseEvent
    {
        Press,
        Click,
    }

    public enum CameraMode
    {
        QuaterView,
    }


}
