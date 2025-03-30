using UnityEngine;
using TMPro;

public class DmgText : MonoBehaviour
{
    public TextMeshPro textMesh;

void Awake()
    {


        if (textMesh == null)
        {
            Debug.LogWarning("TextMeshPro 컴포넌트를 찾을 수 없습니다!");
        }
        else
        {
            // TurnManager의 인스턴스를 찾음
            TurnManager turnManager = FindObjectOfType<TurnManager>();
            if (turnManager != null)
            {
                // TextMeshPro 텍스트를 TurnManager의 calculatedDamage로 설정
                textMesh.text = turnManager.calculatedDamage.ToString();
            }
            else
            {
                Debug.LogWarning("TurnManager 인스턴스를 찾을 수 없습니다!");
            }
        }
    }
}
