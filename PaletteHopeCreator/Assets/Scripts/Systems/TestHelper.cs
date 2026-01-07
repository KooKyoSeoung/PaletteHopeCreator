using UnityEngine;

/// <summary>
/// 테스트를 위한 헬퍼 스크립트
/// Unity 에디터에서 이 스크립트를 씬에 추가하고 키보드 단축키로 테스트 기능 사용
/// </summary>
public class TestHelper : MonoBehaviour
{
    public bool enableGKey = true;
    
    public bool enableRKey = true;

    void Update()
    {
        if (enableGKey && Input.GetKeyDown(KeyCode.G))
        {
            GachaSystem gacha = GachaSystem.Instance;
            if (gacha != null)
            {
                gacha.GiveTestGachaPoints(1000);
                Debug.Log($"[테스트] 가챠 포인트 1000개 지급됨! 현재 포인트: {gacha.GetGachaPoints()}");
            }
        }

        // R 키: 데이터 리셋 (주의: 모든 저장 데이터 삭제)
        if (enableRKey && Input.GetKeyDown(KeyCode.R))
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                PlayerPrefs.DeleteAll();
                Debug.Log("[테스트] 모든 저장 데이터가 리셋되었습니다!");
                
                // 인벤토리 재로드
                CharacterInventory inventory = CharacterInventory.Instance;
                if (inventory != null)
                {
                    inventory.LoadOwnedCharacters();
                }
            }
        }
    }

    /// <summary>
    /// 버튼에서 호출 가능한 가챠 포인트 지급 함수
    /// </summary>
    public void GiveGachaPoints(int amount = 1000)
    {
        GachaSystem gacha = GachaSystem.Instance;
        if (gacha != null)
        {
            gacha.GiveTestGachaPoints(amount);
            Debug.Log($"[테스트] 가챠 포인트 {amount}개 지급됨! 현재 포인트: {gacha.GetGachaPoints()}");
        }
    }

    /// <summary>
    /// 버튼에서 호출 가능한 데이터 리셋 함수 (Shift+R 대신 버튼으로)
    /// </summary>
    public void ResetAllData()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("[테스트] 모든 저장 데이터가 리셋되었습니다!");
        
        CharacterInventory inventory = CharacterInventory.Instance;
        if (inventory != null)
        {
            inventory.LoadOwnedCharacters();
        }
    }
}

