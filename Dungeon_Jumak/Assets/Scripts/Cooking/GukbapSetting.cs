using System.Collections.Generic;
using UnityEngine; 

public class GukbapSetting : MonoBehaviour 
{
    public GameObject gukbapPrefab; // 생성할 이미지 프리팹을 담는 public 변수

    public List<bool> gukbapList; // 국밥 리스트 (true면 해당 위치에 국밥이 있는 상태)를 담는 public 변수

    public CookGukbap cookGukbap; // 다른 스크립트에 있는 CookGukbap 스크립트의 인스턴스를 담는 public 변수

    [SerializeField] private int previousCount; // 이전 프레임에서의 국밥 카운트를 저장하기 위한 private 변수
    [SerializeField] Transform[] idxs;

    void Start() // 게임 오브젝트가 활성화될 때 호출되는 함수
    {
        previousCount = cookGukbap.gukbapCount; // 이전 프레임의 국밥 카운트를 초기화
        InitializeGukbapList(); // 국밥 리스트 초기화
    }

    void Update() // 매 프레임마다 호출되는 함수
    {
        // 이전 프레임에서의 국밥 카운트와 현재 카운트가 다를 때마다 이미지 프리팹 생성
        if (cookGukbap.gukbapCount != previousCount)
        {
            if (cookGukbap.gukbapCount > previousCount)
            {
                // 새로운 국밥이 추가된 경우에만 실행
                int index = GetNextAvailableIndex(); // 다음으로 추가할 수 있는 국밥의 인덱스를 가져옴
                if (index != -1) // 추가할 수 있는 위치가 있는지 확인
                {
                    Debug.Log("국밥 인덱스 : " + index);
                    GameObject newGukbap = Instantiate(gukbapPrefab, idxs[index].position, Quaternion.identity); // 새로운 국밥을 생성하고 배치
                    newGukbap.transform.parent = transform; // 새로운 국밥을 이 스크립트의 자식으로 설정
                    gukbapList[index] = true; // 국밥 리스트에서 해당 위치의 값을 true로 설정하여 차지한 것으로 표시
                }
            }
            previousCount = cookGukbap.gukbapCount; // 이전 프레임의 국밥 카운트를 업데이트
        }

        CheckGukbapPresence(); // 국밥이 있는지 확인하고 없으면 해당 위치를 false로 만듦
    }

    // 국밥 리스트 초기화
    void InitializeGukbapList()
    {
        for (int i = 0; i < 5; i++)
        {
            gukbapList.Add(false); // 초기에는 모든 위치에 국밥이 없음을 표시
        }
    }

    // 다음으로 추가할 수 있는 국밥의 인덱스 반환
    int GetNextAvailableIndex()
    {
        for (int i = 0; i < gukbapList.Count; i++)
        {
            if (!gukbapList[i]) // 해당 위치에 국밥이 없는지 확인
            {
                return i; // 국밥이 없는 위치의 인덱스 반환
            }
        }
        return -1; // 추가할 수 있는 위치가 없음
    }

    void CheckGukbapPresence()
    {
        for (int i = 0; i < gukbapList.Count; i++)
        {
            Collider2D[] colliders = Physics2D.OverlapPointAll(idxs[i].transform.position);
            bool gukbapPresent = false;
            

            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject.CompareTag("Gukbab"))
                {
                    gukbapPresent = true;

                    break;
                }
            }

            gukbapList[i] = gukbapPresent;
        }
    }
}
