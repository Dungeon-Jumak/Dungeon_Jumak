using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class DataManager : MonoBehaviour
{
    // --- �̱��� ���� --- //
    static DataManager instance;
    public static DataManager Instance
    {
        get
        {
            if (!instance)
            {
                container = new GameObject();
                container.name = "DataManager";
                instance = container.AddComponent(typeof(DataManager)) as DataManager;
                DontDestroyOnLoad(container);
            }
            return instance;
        }
    }

    // --- �����̳� ���� ������Ʈ --- //
    static GameObject container;

    // --- ���� ������ �����̸� ���� ("���ϴ� �̸�.json) --- //
    string gameDataFileName = "GameData.json";

    // --- ����� Ŭ���� ���� --- //
    public Data data = new Data();

    // --- ������ �ε� --- //
    public void LoadGameData()
    {
        string filePath = Application.persistentDataPath + "/" + gameDataFileName;

        // ����� ������ �̹� ���� ���
        if (File.Exists(filePath))
        {
            string FromJsonData = File.ReadAllText(filePath);
            data = JsonUtility.FromJson<Data>(FromJsonData);
        }
    }

    // --- ������ ���� --- //
    public void SaveGameData()
    {
        // Ŭ���� -> Json (true = ������ ���� �ۼ�)
        string ToJsonData = JsonUtility.ToJson(data, true);
        string filePath = Application.persistentDataPath + "/" + gameDataFileName;

        //�̹� ����� ���� �ִٸ� �����, ���ٸ� ���� ���� ����
        File.WriteAllText(filePath, ToJsonData);
    }

    // --- ���� ���� �Լ� --- //
    public void UpdateLevel()
    {
        data.level++;
        GameObject.Find("UI_LevelText").GetComponent<TextMeshProUGUI>().text = data.coin.ToString();
    }

    // --- ���� ���� --- //
    public void UpdateCoin()
    {
        data.coin++;
        GameObject.Find("UI_CoinText").GetComponent<TextMeshProUGUI>().text = data.level.ToString();
    }
}
